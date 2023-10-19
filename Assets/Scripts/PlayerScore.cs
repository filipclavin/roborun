
using System;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int CurrentScore { get; private set; }
    
    public int scoreValue;


    private void Update()
    {
        scoreValue = CurrentScore;
        TempUI.Instance.UpdateScore(scoreValue);
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
    }
}
