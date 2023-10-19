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
        boxCollider = transform.parent.GetComponent<BoxCollider>();
        batteryController = FindAnyObjectByType<BatteryController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            batteryController.ObstacleHit(drainValue);
            boxCollider.enabled = false;
        }         
    }
}
