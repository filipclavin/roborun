using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupInvis : Powerup
{
    public Material material;

    protected override void PowerUpActive()
    {
        batteryController.SetGod(duration, material);
    }
}
