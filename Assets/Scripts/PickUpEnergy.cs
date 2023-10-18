using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEnergy : MonoBehaviour
{
	private PlayerScore playerScore;
	private MeshRenderer meshRenderer;
	private BatteryController batteryController;

	[SerializeField] private int scoreValue;
    [SerializeField] private int batteryValue;

    private void Start()
    {
		playerScore = FindAnyObjectByType<PlayerScore>();
		meshRenderer = GetComponent<MeshRenderer>();
        batteryController = playerScore.GetComponent<BatteryController>();
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			gameObject.SetActive(false);
			if (batteryController.ChargeBattery(batteryValue) == true)
			{
				playerScore.AddScore(scoreValue);
			}
			
		}
	}
}
