using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int drainValue;
    [SerializeField] private GameData gameData;
    private GameTimer gameTimer;

    private BatteryController batteryController;
    private BoxCollider boxCollider;
    Movement movement;
    

    private void Start()
    {
        movement = FindAnyObjectByType<Movement>();
        boxCollider = transform.parent.GetComponent<BoxCollider>();
        batteryController = FindAnyObjectByType<BatteryController>();
        gameTimer = FindAnyObjectByType<GameTimer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("Crash");
            movement.effects.ElementAt(2).Play();
            gameTimer.ApplySpeedPenalty();
            batteryController.ObstacleHit(drainValue);
            boxCollider.enabled = false;
        }         
    }
}
