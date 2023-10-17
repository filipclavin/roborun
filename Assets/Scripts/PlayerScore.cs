using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private int currentScore;


    public void AddScore(int score)
    {
        currentScore += score;
    }
}
