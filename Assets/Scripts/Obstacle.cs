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
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Damage taken " + collision);
            batteryController.BatteryDrain(drainValue);
            boxCollider.enabled = false;
        }        
    }
}
