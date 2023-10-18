using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private BatteryController batteryController;
    [SerializeField] private int drainValue;

    private void Start()
    {
        batteryController = FindAnyObjectByType<BatteryController>();
    }

    private void OnCollisionEnter(Collision collision)
    { 
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Damage taken " + hit);
        if (hit.gameObject.CompareTag("Player"))
        {
            batteryController.BatteryDrain(drainValue);
            gameObject.SetActive(false);
        }        
        
    }
}
