using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private BatteryController batteryController;
    [SerializeField] private int drainValue;
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        batteryController = FindAnyObjectByType<BatteryController>();
    }

    private void OnCollisionEnter(Collision collision)
    { 
        Debug.Log("Damage taken " + collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            batteryController.BatteryDrain(drainValue);
            boxCollider.enabled = false;
        }        
    }
}
