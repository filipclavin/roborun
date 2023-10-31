using System;
using System.Collections;
using UnityEngine;

public class GodModeSpeed : MonoBehaviour
{
    private Move move;
    private BatteryController _batteryController;
    private float minTimeScale = 0.5f;
    private float maxTimeScale = 1f;
    private float currentTimeScale;
    private GameTimer gameTimer;
    
    
    [Obsolete("Obsolete")]
    private void Start()
    {
        move = GetComponent<Move>();
        _batteryController = FindObjectOfType<BatteryController>();
        gameTimer = FindObjectOfType<GameTimer>();
    }
    
   private void Update()
    {
        move.speed = _batteryController.isGod ? 10f : 5f;
        
        GodModeEffect();
    }

    private void GodModeEffect()
    {
        if (_batteryController.isGod)
            StartCoroutine(GodSlowMo());

    }
    
    private void AdjustTimeScale()
    {
        float progress = gameTimer.gameTimer / gameTimer.gameLength;
        currentTimeScale = Mathf.Lerp(minTimeScale, maxTimeScale, progress);
    }
    
    private static IEnumerator GodSlowMo()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 1;
    }
}
