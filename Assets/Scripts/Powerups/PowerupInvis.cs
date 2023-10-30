using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupInvis : Powerup
{
    public Material material;

    protected override IEnumerator PowerUpActive()
    {
        batteryController.SetGod(duration, material);
        yield return null;
    }
}
