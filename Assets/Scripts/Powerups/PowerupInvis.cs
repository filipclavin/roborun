using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupInvis : Powerup
{
    private Color invisColor = Color.yellow;

    protected override IEnumerator PowerUpActive()
    {
        BatteryController batteryController = FindAnyObjectByType<BatteryController>();
        batteryController.GetComponent<PlayerVisuals>().ChangeColors(invisColor, duration);
        batteryController.SetInvis(duration);
        batteryController.isGod = true;
        yield return null;
    }
}
