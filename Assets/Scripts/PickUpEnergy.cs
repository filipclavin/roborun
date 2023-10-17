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

    private void Start()
    {
		playerScore = FindAnyObjectByType<PlayerScore>();
		meshRenderer = GetComponent<MeshRenderer>();
        batteryController = GetComponent<BatteryController>();
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
            meshRenderer.enabled = false;
			playerScore.AddScore(scoreValue);
			batteryController.ChargeBattery(scoreValue);
		}
	}
}
