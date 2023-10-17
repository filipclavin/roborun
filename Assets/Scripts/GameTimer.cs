using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float gameLength = 100f;

	private void Update()
	{
		gameLength -= Time.deltaTime;

		if (gameLength <= 0)
		{
			// player with most score win
		}
	}
}
