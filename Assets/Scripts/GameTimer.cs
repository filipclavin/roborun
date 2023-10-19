using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
	private PlayerScore player;
    [SerializeField] private float gameLength = 100f;


	public bool goingOn = true;

    private void Start()
    {
		TempUI.Instance.UpdateHighScore(0);
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
		goingOn = false;
		TempUI.Instance.UpdateHighScore(player.scoreValue);
		Debug.Log("You got " + player.scoreValue + " and it is a new highscore!");
		TempUI.Instance.GameOver();
	}
}
