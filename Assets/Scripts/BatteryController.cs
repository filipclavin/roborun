using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryController : MonoBehaviour
{
    private GameTimer gameTimer;
    private MeshRenderer meshRenderer;
    private Color defaultColor;


    private bool invisActive = false;
    private float maxBattery;

    [SerializeField] private Color hitColor;
    [SerializeField] private float currentBattery = 100;
    [SerializeField] private float invisTime = 1f;

    [Header("Charge values")]
    [SerializeField] private float batteryCharge = 0.50f;



    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        maxBattery = currentBattery;
        TempUI.Instance.StartUI(currentBattery, maxBattery);
    }

    private void FixedUpdate()
    {
        ChargeBattery(batteryCharge);
    }

    public bool ChargeBattery(float rechargeValue)
    {
        if (gameTimer.goingOn)
        {
            currentBattery += rechargeValue;
            if (currentBattery > maxBattery)
            {
                currentBattery = maxBattery;
                TempUI.Instance.UpdateBatteryBar(currentBattery);
                return true;
            }
            TempUI.Instance.UpdateBatteryBar(currentBattery);
        }
        return false;
    }

    public void ObstacleHit(float drain)
    {
        if (invisActive == false)
        {
            BatteryDrain(drain);
            StartCoroutine(InvisTime(invisTime));
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
            meshRenderer.material.color = hitColor;
            yield return new WaitForSeconds(blinkOne);
            meshRenderer.material.color = defaultColor;
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
            gameTimer.EndGame();
        }
        TempUI.Instance.UpdateBatteryBar(currentBattery);
    }

}
