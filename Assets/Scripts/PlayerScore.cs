
using System;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int CurrentScore { get; private set; }
    
    public int scoreValue;


    [Header("TempUI")]
    [SerializeField] private TMP_Text batteryText;


    private void Update()
    {
        scoreValue = CurrentScore;
        if (batteryText != null) // Remove when we implement new UI
        {
            batteryText.text = "Score " + CurrentScore;
        }
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
    }
}
