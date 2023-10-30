using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupInvis : Powerup
{
    [SerializeField] private Color godColor;

    protected override IEnumerator PowerUpActive()
    {
        batteryController.GetComponent<PlayerVisuals>().ChangeColors(godColor, duration);
        batteryController.isGod = true;
        yield return null;
    }
}
