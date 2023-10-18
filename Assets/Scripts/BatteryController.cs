using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    private GameTimer gameTimer;
    private MeshRenderer meshRenderer;
    private Color defaultColor;


    private bool invisActive = false;
    private float maxBattery;

    [SerializeField] private Color hitColor;
    [SerializeField] private float currentBattery = 100;
    [SerializeField] private float invisTime = 1.5f;

    [Header("Drain values")]
    [SerializeField] private float batteryCharge = 0.50f;

    [Header("TempUI")]
    [SerializeField] private TMP_Text batteryText;

    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
        maxBattery = currentBattery;
        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
    }

    private void FixedUpdate()
    {
        if (gameTimer.goingOn)
        {
            ChargeBattery(batteryCharge);
        
            if (batteryText != null ) // Remove when we implement new UI
            {
                batteryText.text = "Battery " + Mathf.RoundToInt(currentBattery) + "/" + maxBattery;
            }
        }
    }

    public bool ChargeBattery(float rechargeValue)
    {
        currentBattery += rechargeValue;
        if (currentBattery > maxBattery)
        {
            currentBattery = maxBattery;
            return true;
        }
        return false;
    }

    public void ObstacleHit(float drain)
    {
        if (invisActive == false)
        {
            BatteryDrain(drain);
            StartCoroutine(ChangeColorOnHit());
        }
    }

    private IEnumerator ChangeColorOnHit()
    {
        meshRenderer.material.color = hitColor;
        yield return new WaitForSeconds(0.2f);
        meshRenderer.material.color = defaultColor;
        yield return new WaitForSeconds(0.1f);
    }

    private void BatteryDrain(float drain)
    {
        currentBattery -= drain;
        if (currentBattery <= 0)
        {
            gameTimer.EndGame();
        }
    }
}
