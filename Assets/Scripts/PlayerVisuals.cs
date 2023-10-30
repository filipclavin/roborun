using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
    private List<Color> defaultColor = new List<Color>();
    private MeshRenderer batteryMeshRenderer;
    private float batteryAnimTimePassed = 0f;
    private bool updatingVisualBattery = false;
    private float batteryLastFrame = -1f;
    private float maxBattery;
    private float batteryDamagedPercent;
    private readonly int batteryDamageDivide = 3;
    private BatteryController batteryController;
    private float colorDuration;
    private Color currentRobotColor;

    [SerializeField] private float batteryAnimTime = 0.5f;
    [SerializeField] private Transform visualBattery;
    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material damagedMaterial;

    private void Start()
    {
        batteryController = GetComponent<BatteryController>();
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

        maxBattery = batteryController.currentBattery;
        batteryDamagedPercent = maxBattery / batteryDamageDivide;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetColors();
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

    private void ChangeVisualBattery()
    {
        if (batteryController.currentBattery < batteryDamagedPercent)
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

    private void ResetColors()
    {
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            SkinnedMeshRenderer mesh = meshRenderers[i];
            mesh.material.color = defaultColor[i];
        }
    }

    private void ChangeColorMesh()
    {
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            SkinnedMeshRenderer mesh = meshRenderers[i];
            mesh.material.color = currentRobotColor;
        }
    }

    private IEnumerator ChangeColor()
    {
        ChangeColorMesh();
        yield return new WaitForSeconds(colorDuration);
        ResetColors();
    }

    public void ChangeColors(Color color, float duration)
    {
        StopCoroutine(ChangeColor());
        ResetColors();
        currentRobotColor = color;
        colorDuration = duration;
        StartCoroutine(ChangeColor());
    }

    /*
    public void ChangeColors(Material material)
    {

    }

    private IEnumerator ChangeMaterial()
    {
        for
    }
    */
}
