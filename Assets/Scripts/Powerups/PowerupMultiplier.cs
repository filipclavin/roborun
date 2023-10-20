using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMultiplier : Powerup
{
    [SerializeField] private int multiplier;

    protected override IEnumerator PowerUpActive()
    {
        PlayerScore playerScore = FindAnyObjectByType<PlayerScore>();
        if (playerScore.multipler == multiplier)
        {
            StopCoroutine(PowerUpActive());
        }
        playerScore.multipler *= multiplier;
        yield return new WaitForSeconds(duration);
        playerScore.multipler /= multiplier;
    }
}