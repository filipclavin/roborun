using UnityEngine;

public class GameTimer : MonoBehaviour
{
	private PlayerScore player;
	private float leadingScore;
    [SerializeField] private float gameLength = 100f;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerScore>();
    }

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
