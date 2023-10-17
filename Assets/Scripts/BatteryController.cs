using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    [SerializeField] private float currentBattery = 100;
    private float maxBattery;

    [Header("Drain values")]
    [SerializeField] private float currentBatteryDrain = 0.50f;
    [SerializeField] private float invisTime = 1.5f;

    private void Start()
    {
        maxBattery = currentBattery;
    }

    private void FixedUpdate()
    {
        BatteryDrain(currentBatteryDrain);
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
        currentBattery -= drain;
        if (currentBattery <= 0)
        {
            // Game over
        }
    }

}
