using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupInvis : Powerup
{
    private Color invisColor = Color.yellow;

    protected override IEnumerator PowerUpActive()
    {
        BatteryController batteryController = FindAnyObjectByType<BatteryController>();
        batteryController.ChangeColors(invisColor, duration);
        batteryController.SetInvis(duration);
        yield return null;
    }
}
