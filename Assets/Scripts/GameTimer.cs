using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float gameLength = 100f;
	[SerializeField] private PlayerScore playerOne;
	[SerializeField] private PlayerScore playerTwo;

	private void Update()
	{
		gameLength -= Time.deltaTime;

		if (gameLength <= 0)
		{
			CheckingScore();
		}
	}

	private void CheckingScore()
	{ 
		if (playerOne.CurrentScore != playerTwo.CurrentScore)
		{
			EndGame();
		}
		else
		{
			// Draw, Overtime????
		}
	}

	private void EndGame()
	{
		PlayerScore winningPlayer = playerOne.CurrentScore > playerTwo.CurrentScore ? playerOne : playerTwo;

		Debug.Log(winningPlayer.gameObject.name + "is the winner of this session");
	}
}
