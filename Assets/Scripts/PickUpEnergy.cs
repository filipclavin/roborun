using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEnergy : MonoBehaviour
{
	[SerializeField] private int scoreValue;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent<PlayerScore>(out var playerScore))
		{
			playerScore.AddScore(scoreValue);
		}
	}
}
