using System;
using System.Collections;
using UnityEngine;

//script made by Daniel Alvarado
public class GodModeSpeed : MonoBehaviour
{
    private Move _move;
    private BatteryController _batteryController;
    private readonly float _minTimeScale = 0.2f; 
    private readonly float _maxTimeScale = 1f;
    private float _currentTimeScale;
    private float _waitTime;
    private GameTimer _gameTimer; 
    
    [Obsolete("Obsolete")]
    private void Start()
    {
        _move = GetComponent<Move>();
        _batteryController = FindObjectOfType<BatteryController>();
        _gameTimer = FindObjectOfType<GameTimer>();
    }
    private void Update()
    { 
       
        _move.speed = _batteryController.isGod ? 10f : 5f;
        
        AdjustTimeBasedOnTimer();
        GodModeEffect();
        if(UIManager.Instance.isPaused)
            Time.timeScale = 0f;
        if(UIManager.Instance.isReloaded)
            Time.timeScale = 1f;
        
        Debug.Log("Current Time Scale: " + Time.timeScale);
        
    }

    private void GodModeEffect()
    {
        if (_batteryController.isGod)
        {
            StartCoroutine(GodSlowMo());
        }
    }
    
    private void AdjustTimeBasedOnTimer()
    {
        float progress = _gameTimer.gameTimer / _gameTimer.gameLength;
        _currentTimeScale = Mathf.Lerp(_minTimeScale, _maxTimeScale, progress);
        _waitTime = Mathf.Lerp(.5f, 2f, progress); 
    }
    
    

    private IEnumerator GodSlowMo()
    {
        float originalTimeScale = _currentTimeScale;
        Time.timeScale = originalTimeScale * 0.5f; 

        yield return new WaitForSecondsRealtime(_waitTime);
        if(UIManager.Instance.isPaused == false)
        {
            PlayerFXManager.Instance.PlayCameraEffect();
            Time.timeScale = 1;
            //Time.timeScale = UIManager.Instance.isPaused ? 0f : 1f; 
        }
    }
}