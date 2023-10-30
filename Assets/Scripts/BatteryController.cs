using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    private GameTimer gameTimer;
    private List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
    private List<Color> defaultColor = new List<Color>();
    private bool invisActive = false;
    private bool updatingVisualBattery = false;
    private float invisTimer = 0;
    private float invisDuration = 0;
    private float batteryAnimTimePassed = 0f;
    private float batteryLastFrame = -1f;
    private float maxBattery;
    private float colorDuration;
    private Color currentRobotColor;

    [Header("Visual Battery")]
    private float batteryDamagedPercent;
    private readonly int batteryDamageDivide = 3;
    private MeshRenderer batteryMeshRenderer;
    [SerializeField] private Transform visualBattery;
    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material damagedMaterial;
    [Space]
    [SerializeField] private Color hitColor;
    [SerializeField] private float currentBattery = 100;
    [SerializeField] private float damageInvis = 1f;
    [SerializeField] private float batteryAnimTime;

    /*
        If we ever bring back batterycharge
        [Header("Charge values")]
        [SerializeField] private float batteryCharge = 0.50f;
    */


    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
        batteryMeshRenderer = visualBattery.GetComponentInChildren<MeshRenderer>();
        meshRenderers.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            SkinnedMeshRenderer renderer = meshRenderers[i];

            if (renderer == batteryMeshRenderer)
            {
                meshRenderers.Remove(renderer);
                i--;
                continue;
            } 


            defaultColor.Add(renderer.material.color);
        }
        maxBattery = currentBattery;
        batteryDamagedPercent = maxBattery / batteryDamageDivide;
        UIManager.Instance.StartUI(currentBattery, maxBattery);
        UIManager.Instance.UpdateBatteryBar(currentBattery);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            SkinnedMeshRenderer mesh = meshRenderers[i];
            mesh.material.color = defaultColor[i];
        }
    }

    private void FixedUpdate()
    {
        //ChargeBattery(batteryCharge);
        if (invisActive)
        {
            invisTimer += Time.fixedDeltaTime;
            if (invisTimer >= invisDuration)
            {
                invisTimer = 0;
                invisActive = false;
            }
        }
    }

    private void Update()
    {
        if (batteryLastFrame != -1f && batteryLastFrame != currentBattery)
        {
            ChangeVisualBattery();
        }

        if (updatingVisualBattery)
        {
            TransitionBatteryScale();
        }

        batteryLastFrame = currentBattery;
    }

    private void TransitionBatteryScale()
    {
        Vector3 targetScale = new Vector3(visualBattery.localScale.x, currentBattery / maxBattery, visualBattery.localScale.z);
        visualBattery.transform.localScale = Vector3.Slerp(visualBattery.localScale, targetScale, batteryAnimTimePassed / batteryAnimTime);

        if (batteryAnimTimePassed >= batteryAnimTime) {
            updatingVisualBattery = false;
        } else
        {
            batteryAnimTimePassed += Time.deltaTime;
        }
    }

    private void ChangeVisualBattery()
    {
        if (currentBattery < batteryDamagedPercent)
        {
            batteryMeshRenderer.material = damagedMaterial;
        }
        else
        {
            batteryMeshRenderer.material = healthyMaterial;
        }

        batteryAnimTimePassed = 0f;
        updatingVisualBattery = true;
    }

    public bool ChargeBattery(float rechargeValue)
    {
        if (gameTimer.goingOn)
        {
            if (currentBattery == maxBattery)
            {
                UIManager.Instance.UpdateBatteryBar(currentBattery);
                return true;
            }
            currentBattery += rechargeValue;
            if(currentBattery > maxBattery)
            {
                currentBattery = maxBattery;
            }
            UIManager.Instance.UpdateBatteryBar(currentBattery);
        }
        return false;
    }

    public void ObstacleHit(float drain)
    {
        if (invisActive == false)
        {
            BatteryDrain(drain);
            StartCoroutine(InvisTime(damageInvis));
        }
    }

    private IEnumerator InvisTime(float seconds)
    {
        float blinkingTime = 0f;
        float blinkOne = 0.2f;
        float blinkTwo = 0.1f;
        SetInvis(seconds);
        while (blinkingTime <= invisDuration)
        {
            ChangeColors(hitColor, blinkOne);
            yield return new WaitForSeconds(blinkOne + blinkTwo);
            blinkingTime += blinkOne + blinkTwo;
            if (blinkingTime >= seconds)
            {
                invisActive = false;
            }
        }
    }

    private void BatteryDrain(float drain)
    {
        currentBattery -= drain;
        PlayerFXManager.Instance.DamageEffect();
        if (currentBattery <= 0)
        {
            currentBattery = 0;
            gameTimer.EndGame(false);
        }
        UIManager.Instance.UpdateBatteryBar(currentBattery);
    }
    private IEnumerator ChangeColor()
    {
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            SkinnedMeshRenderer mesh = meshRenderers[i];
            mesh.material.color = currentRobotColor;
        }

        yield return new WaitForSeconds(colorDuration);

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            SkinnedMeshRenderer mesh = meshRenderers[i];
            mesh.material.color = defaultColor[i];
        }
    }

    public void ChangeColors(Color color, float duration)
    {
        currentRobotColor = color;
        colorDuration = duration;
        StopCoroutine(ChangeColor());
        StartCoroutine(ChangeColor());
    }

    public void SetInvis(float duration)
    {
        invisDuration = duration;
        if (invisActive)
        {
            invisTimer = 0;
        }
        else
        {
            invisActive = true;
            StartCoroutine(InvisTime(duration));
        }
    }
}
