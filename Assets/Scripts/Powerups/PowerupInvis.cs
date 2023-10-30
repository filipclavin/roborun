using UnityEngine;

public class PowerupInvis : Powerup
{
    public Material material;

    protected override void PowerUpActive()
    {
        batteryController.SetGod(duration, material);
    }
}
