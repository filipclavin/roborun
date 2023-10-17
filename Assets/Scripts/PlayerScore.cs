
using System;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int CurrentScore { get; private set; }
    
    public int scoreValue;


    [Header("TempUI")]
    [SerializeField] private TMP_Text scoreText;


    private void Update()
    {
        scoreValue = CurrentScore;
        if (scoreText != null) // Remove when we implement new UI
        {
            scoreText.text = "Score " + CurrentScore;
        }
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
    }
}
