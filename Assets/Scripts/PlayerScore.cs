
using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int CurrentScore { get; private set; }
    
    public int scoreValue;

    private void Update()
    {
        scoreValue = CurrentScore;
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
        Debug.Log(CurrentScore);
    }
}
