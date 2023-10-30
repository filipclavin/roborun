
using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private float multipliertimer;
    private float multiplierDuration;
    private readonly float baseMultipler = 1;
    private float multiplier;

    public int CurrentScore { get; private set; }
    
    public int scoreValue;

    private void Start()
    {
        multiplier = baseMultipler;
    }

    private void Update()
    {
        scoreValue = CurrentScore;
        if (multiplier != baseMultipler)
        {
            multipliertimer += Time.deltaTime;
            if (multipliertimer >= multiplierDuration)
            {
                multiplier = 1;
                UIManager.Instance.DisableScoreMulti();
            }
        }
    }

    public void AddScore(int score)
    {
        CurrentScore += (int) (score * multiplier);
        UIManager.Instance.UpdateScore(CurrentScore);
    }

    public void PowerUpMultiplier(float duration, float pickUpMultiplier)
    {
        if (multiplier == baseMultipler)
        {
            multiplier *= pickUpMultiplier;
        }
        multiplierDuration = duration;
        multipliertimer = 0;
        UIManager.Instance.ActivateScoreMulti(multiplier);
        
    }
}
