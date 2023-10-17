using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEnergy : MonoBehaviour
{
	[SerializeField] private int scoreValue;

	private void OnCollisionEnter(Collision collision)
	{
		PlayerScore playerScore = collision.gameObject.GetComponent<PlayerScore>();
		if (playerScore != null)
		{
			playerScore.AddScore(scoreValue);
		}
	}
}
