using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    private GameTimer gameTimer;

    [SerializeField] private float currentBattery = 100;
    private float maxBattery;

    [Header("Drain values")]
    [SerializeField] private float currentBatteryDrain = 0.50f;
    [SerializeField] private float invisTime = 1.5f;

    [Header("TempUI")]
    [SerializeField] private TMP_Text batteryText;

    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
        maxBattery = currentBattery;
    }

    private void FixedUpdate()
    {
        if (gameTimer.goingOn)
        {
            BatteryDrain(currentBatteryDrain);
        
            if (batteryText != null ) // Remove when we implement new UI
            {
                batteryText.text = "Battery " + Mathf.RoundToInt(currentBattery) + "/" + maxBattery;
            }
        }
    }

    public void ChargeBattery(float rechargeValue)
    {
        currentBattery += rechargeValue;
        if (currentBattery > maxBattery)
        {
            currentBattery = maxBattery;
        }
    }

    public void BatteryDrain(float drain)
    {
        if (currentBattery > 0)
        {
            currentBattery -= drain;
        }
        if (currentBattery <= 0)
        {
            gameTimer.EndGame();
        }
    }

    public void ObstacleHit(float drain)
    {
        BatteryDrain(drain);
    }

}
