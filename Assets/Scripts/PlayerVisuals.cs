using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
    private float batteryAnimTimePassed = 0f;
    private bool updatingVisualBattery = false;
    private float batteryLastFrame = -1f;
    private float maxBattery;
    private float batteryDamagedPercent;
    private readonly int batteryDamageDivide = 3;
    private BatteryController batteryController;
    private float materialDuration = 0;
    private float changeMaterialTimer = 0;
    private Material currentRobotColor;
    private float blinkOne = 0.3f;
    private float blinkTwo = 0.2f;


    [SerializeField] private Material defaultMaterial;
    [SerializeField] private SkinnedMeshRenderer batteryRenderer;
    [SerializeField] private float batteryAnimTime = 0.5f;
    [SerializeField] private Transform visualBattery;
    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material damagedMaterial;
    
    private void Start()
    {
        batteryController = GetComponent<BatteryController>();
        meshRenderers.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            SkinnedMeshRenderer renderer = meshRenderers[i];

            if (renderer == batteryRenderer)
            {
                meshRenderers.Remove(renderer);
                break;
            }

        }

        maxBattery = batteryController.currentBattery;
        batteryDamagedPercent = maxBattery / batteryDamageDivide;
    }

    private void OnDisable()
    {
        ResetMaterial();
    }

    private void Update()
    {
        if (batteryLastFrame != -1f && batteryLastFrame != batteryController.currentBattery)
        {
            ChangeVisualBattery();
        }

        if (updatingVisualBattery)
        {
            TransitionBatteryScale();
        }
        batteryLastFrame = batteryController.currentBattery;

        if (materialDuration > 0)
        {
            changeMaterialTimer += Time.deltaTime;
            if (changeMaterialTimer >= materialDuration)
            {
                ResetMaterial();
                materialDuration = 0;
            }
		}
    }
    
    private void TransitionBatteryScale()
    {
        Vector3 targetScale = new Vector3(visualBattery.localScale.x, batteryController.currentBattery / maxBattery, visualBattery.localScale.z);
        visualBattery.transform.localScale = Vector3.Slerp(visualBattery.localScale, targetScale, batteryAnimTimePassed / batteryAnimTime);

        if (batteryAnimTimePassed >= batteryAnimTime)
        {
            updatingVisualBattery = false;
        }
        else
        {
            batteryAnimTimePassed += Time.deltaTime;
        }
    }

    public void ChangeVisualBattery()
    {
        if (batteryController.currentBattery < batteryDamagedPercent)
        {
            batteryRenderer.material = damagedMaterial;
        }
        else
        {
            batteryRenderer.material = healthyMaterial;
        }
        
        batteryAnimTimePassed = 0f;
        updatingVisualBattery = true;
    }

    private void ChangeCurrentMaterial()
    {
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            SkinnedMeshRenderer mesh = meshRenderers[i];
            mesh.material = currentRobotColor;
        }
    }

    private void ResetMaterial()
    {
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            SkinnedMeshRenderer mesh = meshRenderers[i];
            mesh.material = defaultMaterial;
        }
    }

    private IEnumerator Blinking(float blinkStart)
    {
        yield return new WaitForSeconds(blinkStart);
        while (changeMaterialTimer <= materialDuration)
        {
            ChangeCurrentMaterial();
            yield return new WaitForSeconds(blinkOne);
            ResetMaterial();
            yield return new WaitForSeconds(blinkTwo);
        }
    }

    public void ChangeColors(Material material, float duration, float blinkStart)
    {
        if (duration > 0)
        {
		    ResetMaterial();
        }
        changeMaterialTimer = 0;
        currentRobotColor = material;
        materialDuration = duration;
        ChangeCurrentMaterial();
        if (blinkStart > -1)
        {
            StartCoroutine(Blinking(blinkStart));
        }
    }

    public void ChangeColors(Material material, float duration)
    {
        ChangeColors(material, duration, -1);
    }
}
