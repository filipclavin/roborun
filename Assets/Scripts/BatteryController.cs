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

    [Header("TempUI")]
    [SerializeField] private Slider batteryBar;
    private TMP_Text batteryText;

    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
        if (batteryBar != null)
        {
            batteryText = batteryBar.gameObject.GetComponentInChildren<TMP_Text>();
            maxBattery = currentBattery;
            batteryBar.maxValue = 0;
            batteryBar.maxValue = maxBattery;
            UpdateBatteryBar();
        }
        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
    }

    private void FixedUpdate()
    {
        if (gameTimer.goingOn)
        {
            ChargeBattery(batteryCharge);
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
        else
        {
            UpdateBatteryBar();
        }
        return false;
    }

    public void ObstacleHit(float drain)
    {
        if (invisActive == false)
        {
            BatteryDrain(drain);
            StartCoroutine(ChangeColorOnHit(invisTime));
            invisActive = true;
        }
    }

    private IEnumerator ChangeColorOnHit(float seconds)
    {
        float blinkingTime = 0f;
        while (blinkingTime < seconds)
        {
            meshRenderer.material.color = hitColor;
            yield return new WaitForSeconds(0.3f);
            meshRenderer.material.color = defaultColor;
            yield return new WaitForSeconds(0.2f);
            blinkingTime += 0.2f;
        }
        invisActive = false;
    }

    private void BatteryDrain(float drain)
    {
        currentBattery -= drain;
        if (currentBattery <= 0)
        {
            gameTimer.EndGame();
        }
        else
        {
            UpdateBatteryBar();
        }
    }

    private void UpdateBatteryBar()
    {
        if (batteryBar != null)
        {
            batteryBar.value = currentBattery;
            batteryText.text = Mathf.RoundToInt(currentBattery) + "/" + maxBattery;
        }
    }
}
