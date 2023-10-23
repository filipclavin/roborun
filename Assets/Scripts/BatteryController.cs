using System.Collections;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    private GameTimer gameTimer;
    private MeshRenderer[] meshRenderers;
    private Color defaultColor;
    private bool invisActive = false;
    private float invisTimer = 0;
    private float invisDuration = 0;

    private float maxBattery;

    [SerializeField] private Color hitColor;
    [SerializeField] private float currentBattery = 100;
    [SerializeField] private float damageInvis = 1f;

    [Header("Charge values")]
    [SerializeField] private float batteryCharge = 0.50f;



    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        defaultColor = Color.white;
        maxBattery = currentBattery;
        UIManager.Instance.StartUI(currentBattery, maxBattery);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.material.color = defaultColor;
        }
    }

    private void FixedUpdate()
    {
        ChargeBattery(batteryCharge);
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

    public bool ChargeBattery(float rechargeValue)
    {
        if (gameTimer.goingOn)
        {
            currentBattery += rechargeValue;
            if (currentBattery >= maxBattery)
            {
                currentBattery = maxBattery;
                UIManager.Instance.UpdateBatteryBar(currentBattery);
                return true;
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
        float blinkOne = 0.3f;
        float blinkTwo = 0.2f;

        invisActive = true;
        while (blinkingTime < seconds)
        {
            ChangeColors(hitColor, blinkOne);
            yield return new WaitForSeconds(blinkOne);
            ChangeColors(defaultColor, blinkTwo);
            yield return new WaitForSeconds(blinkTwo);
            blinkingTime += blinkOne + blinkTwo;
        }
        invisActive = false;
    }

    private void BatteryDrain(float drain)
    {
        currentBattery -= drain;
        if (currentBattery <= 0)
        {
            currentBattery = 0;
            gameTimer.EndGame(false);
        }
        UIManager.Instance.UpdateBatteryBar(currentBattery);
    }
    private IEnumerator ChangeColor(Color color, float duration)
    {
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.material.color = color;
        }

        yield return new WaitForSeconds(duration);

        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.material.color = defaultColor;
        }
    }

    public void ChangeColors(Color color, float duration)
    {
        StartCoroutine(ChangeColor(color, duration));
    }

    public void SetInvis(float duration)
    {
        if (invisActive)
        {
            invisTimer = 0;
        }
        else
        {
            invisActive = true;   
        }
        invisDuration = duration;
    }
}
