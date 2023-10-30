using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    private PlayerVisuals playerVisuals;
    private GameTimer gameTimer;

    private List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
    private List<Color> defaultColor = new List<Color>();

    public bool invisActive = false;
    private bool updatingVisualBattery = false;

    private float invisTimer = 0;
    private float invisDuration;
    private readonly float damageInvis = 1.5f;

    private float maxBattery;
    public float currentBattery = 100;

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
    }

    private void Update()
    {
        Powerup();
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
            playerVisuals.ChangeColors(Color.red , blinkOne);
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

    public void Powerup()
    {
        if (isGod == true)
        {
            StartCoroutine(GodTimer());
        }
    }

    private IEnumerator GodTimer()
    {
        yield return new WaitForSeconds(5);
        isGod = false;
    }
}
