using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEnergy : MonoBehaviour
{
	private Movement movement;
	private PlayerScore playerScore;
	private MeshRenderer meshRenderer;
	private BatteryController batteryController;
	
	[SerializeField] private bool needFullEnergy = true;
	[SerializeField] private int scoreValue;
    [SerializeField] private int batteryValue;
    
    private void Start()
    {
	    movement = FindAnyObjectByType<Movement>();
		playerScore = FindAnyObjectByType<PlayerScore>();
		meshRenderer = GetComponent<MeshRenderer>();
        batteryController = playerScore.GetComponent<BatteryController>();
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{ 
			movement.lightning.Play();
			if (batteryController.ChargeBattery(batteryValue) == true || needFullEnergy == false)
			{
				playerScore.AddScore(scoreValue);
			}
			gameObject.SetActive(false);
		}
	}
}
