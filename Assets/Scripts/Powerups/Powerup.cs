using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    protected BatteryController batteryController;
    public float duration;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            batteryController = FindAnyObjectByType<BatteryController>();
            PowerUpActive();
        }
    }

    protected abstract void PowerUpActive();
}
