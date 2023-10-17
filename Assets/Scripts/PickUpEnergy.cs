using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEnergy : MonoBehaviour
{
	[SerializeField] private int scoreValue;
	private void OnTriggerEnter(Collider other)
	{
		var playerScore = other.GetComponent<PlayerScore>();
		if (playerScore)
		{
			GetComponent<MeshRenderer>().enabled = false;
			playerScore.AddScore(scoreValue);
			Debug.Log("Score");
			
		}
	}

	
}
