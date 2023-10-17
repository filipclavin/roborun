using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int CurrentScore { get; private set; }

    public void AddScore(int score)
    {
        CurrentScore += score;
    }
}
