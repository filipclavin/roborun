using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
	private PlayerScore player;
    [SerializeField] private float gameLength = 100f;

	[Header("High score")]
    private readonly string leadingScoreKey = "Highscore";
    [SerializeField] private TMP_Text tempHighScore;

	public bool goingOn = true;

    private void Start()
    {
		TemporaryUI();
        player = FindAnyObjectByType<PlayerScore>();
    }

    private void Update()
	{
		if (goingOn)
		{
			gameLength -= Time.deltaTime;

			if (gameLength <= 0)
			{
				EndGame();
			}
		}
	}

	public void EndGame()
	{
		if (player.CurrentScore > PlayerPrefs.GetInt(leadingScoreKey))
		{
            PlayerPrefs.SetInt("Highscore", player.scoreValue);
			TemporaryUI();

        }
		goingOn = false;
		Debug.Log("You got " + player.scoreValue + " and it is a new highscore!");
	}

	private void TemporaryUI()
	{
        if (tempHighScore != null)
        {
            tempHighScore.text = "Highscore: " + PlayerPrefs.GetInt(leadingScoreKey);
        }
	}

	private void DeleteScore()
	{
		PlayerPrefs.DeleteKey("Highscore");
	}
}
