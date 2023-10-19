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
            TempUI.Instance.UpdateTimer(gameLength);

			if (gameLength <= 0)
            {
                EndGame(true);
            }
        }
    }

    public void EndGame(bool won)
    {
        goingOn = false;
        TempUI.Instance.UpdateHighScore(player.scoreValue);
        Time.timeScale = 0;
        if (won)
        {
            TempUI.Instance.Victory(player.scoreValue);
        }
        else
        {
            TempUI.Instance.GameOver();
        }
    }
}
