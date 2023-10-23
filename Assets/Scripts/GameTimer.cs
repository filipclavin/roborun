using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private PlayerScore player;

    public float gameLength = 100f;
    public float gameTimer = 0f;
    public bool goingOn = false;

    [SerializeField] private GameData _gameData;
    [SerializeField] private float _timeScaleMultiplier = 1f;

    private void Start()
    {
        UIManager.Instance.UpdateHighScore(0);
        player = FindAnyObjectByType<PlayerScore>();
    }

    private void Update()
    {
        if (goingOn)
        {
            gameTimer += Time.deltaTime;
            UIManager.Instance.UpdateTimer(gameLength - gameTimer);

            _gameData.scaledDeltaTime = Time.deltaTime * Mathf.Max(Mathf.Log(_timeScaleMultiplier * gameTimer + 1f), 1);

			if (gameTimer >= gameLength)
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
