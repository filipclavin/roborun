using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private PlayerScore player;

    public float gameLength = 100f;
    public bool goingOn = false;

    private void Start()
    {
        UIManager.Instance.UpdateHighScore(0);
        player = FindAnyObjectByType<PlayerScore>();
    }

    private void Update()
    {
        if (goingOn)
        {
            gameLength -= Time.deltaTime / Time.timeScale;
            UIManager.Instance.UpdateTimer(gameLength);

			if (gameLength <= 0)
            {
                EndGame(true);
            }
        }
    }

    public void StartGame()
    {
        goingOn = true;
    }

    public void EndGame(bool won)
    {
        goingOn = false;
        UIManager.Instance.UpdateHighScore(player.scoreValue);
        Time.timeScale = 0;
        if (won)
        {
            UIManager.Instance.Victory(player.scoreValue);
        }
        else
        {
            UIManager.Instance.GameOver();
        }
    }
}
