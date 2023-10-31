using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    private PlayerVisuals playerVisuals;
    private GameTimer gameTimer;

    private bool updatingVisualBattery = false;

    private float invisTimer = 0;
    private float invisDuration;

    private float godTimer = 0;
    private float godDuration;
    private readonly float damageInvis = 1.5f;
    
    private float maxBattery;

    [SerializeField] private Material hitMaterial;
    public float currentBattery = 100;
    public bool invisActive = false;
    public bool isGod;

    /*
        If we ever bring back batterycharge
        [Header("Charge values")]
        [SerializeField] private float batteryCharge = 0.50f;
    */

    private void Start()
    {
        playerVisuals = GetComponent<PlayerVisuals>();
        gameTimer = FindAnyObjectByType<GameTimer>();
        maxBattery = currentBattery;
        UIManager.Instance.StartUI(currentBattery, maxBattery);
        UIManager.Instance.UpdateBatteryBar(currentBattery);
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

        if (!isGod) return;
        godTimer += Time.fixedDeltaTime;
        if (!(godTimer >= godDuration)) return;
        PlayerFXManager.Instance.StopGodSparkles();
        PlayerFXManager.Instance.DustEffect();
        godTimer = 0;
        isGod = false;
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
        if (invisActive == false && isGod == false)
        {
            BatteryDrain(drain);
            gameTimer.ApplySpeedPenalty();
            SetInvis(damageInvis);
        }
    }

    private IEnumerator InvisTime(float seconds)
    {
        float blinkingTime = 0f;
        float blinkOne = 0.2f;
        float blinkTwo = 0.1f;
        while (blinkingTime <= invisDuration)
        {
            playerVisuals.ChangeColors(hitMaterial, blinkOne);
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
        PlayerStateManager.Instance.animator.SetTrigger("Collision");
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
            StartCoroutine(InvisTime(duration));
        }
        invisDuration = duration;
    }

    public void SetGod(float duration, Material godMaterial)
    {
        if (isGod)
        {
            godTimer = 0;
        }
        else
        {
            PlayerStateManager.Instance.animator.SetTrigger("ChargeToGod");
            PlayerFXManager.Instance.StopDustEffect();
            PlayerFXManager.Instance.PlayGodSparkles();
            isGod = true;
            invisActive = false;
        }
        godDuration = duration;
        playerVisuals.ChangeColors(godMaterial, duration);
    }
}