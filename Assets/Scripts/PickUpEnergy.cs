using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEnergy : MonoBehaviour
{
	[SerializeField] private int scoreValue;

	// private void OnCollisionEnter(Collision collision)
	// {
	// 	if (collision.gameObject.TryGetComponent<PlayerScore>(out var playerScore))
	// 	{
	// 		Destroy(this.gameObject);
	// 		playerScore.AddScore(scoreValue);
	// 		
	// 		Debug.Log("Score");
	// 	}
	// }

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.TryGetComponent<PlayerScore>(out var playerScore))
		{
			playerScore.AddScore(scoreValue);
			
			Debug.Log("Score");
		}
	}
}
