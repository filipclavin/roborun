
using System;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int CurrentScore { get; private set; }
    
    public int scoreValue;

    [NonSerialized] public int multipler = 1;


    private void Update()
    {
        scoreValue = CurrentScore;
    }

    public void AddScore(int score)
    {
        CurrentScore += score * multipler;
        TempUI.Instance.UpdateScore(CurrentScore);
    }
}
