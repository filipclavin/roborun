using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMultiplier : Powerup
{
    [SerializeField] private int multiplier;

    protected override void PowerUpActive()
    {
        PlayerScore playerScore = FindAnyObjectByType<PlayerScore>();
        playerScore.PowerUpMultiplier(duration, multiplier);
    }
}