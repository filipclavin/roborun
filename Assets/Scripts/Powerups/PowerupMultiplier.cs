
using UnityEngine;

public class PowerupMultiplier : Powerup
{
    [SerializeField] private int multiplier;

    protected override void PowerUpActive()
    {
        PlayerScore playerScore = FindAnyObjectByType<PlayerScore>();
        playerScore.PowerUpMultiplier(duration, multiplier);
        gameObject.SetActive(false);
        PlayerFXManager.Instance.Play2X();
        AudioManager.Instance.Play("2x");
    }
}