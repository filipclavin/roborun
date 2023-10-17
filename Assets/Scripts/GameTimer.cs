using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float gameLength = 100f;
	[SerializeField] private PlayerScore player;
	private float leadingScore;

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
		if (player.CurrentScore < leadingScore)
		{
			// Change leading score
		}
		EndGame();
	}

	private void EndGame()
	{

		Debug.Log("You got " + player.scoreValue + " and it is a new highscore!");
	}
}
