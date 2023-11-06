using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int drainValue;
    [SerializeField] private GameData gameData;

    private BatteryController batteryController;
    private BoxCollider boxCollider;
    

    private void Start()
    {
        boxCollider = transform.parent.GetComponent<BoxCollider>();
        batteryController = FindAnyObjectByType<BatteryController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        boxCollider.enabled = false;
        if (batteryController.invisActive || batteryController.isGod) return;
        batteryController.ObstacleHit(drainValue);
    }
}
