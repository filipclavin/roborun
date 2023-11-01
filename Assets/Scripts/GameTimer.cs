using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private PlayerScore player;

    public float gameLength = 100f;
    public float gameTimer = 0f;
    public bool goingOn = false;

    [SerializeField] private GameData _gameData;
    [SerializeField] private AnimationCurve _timeScaleCurve;
    [SerializeField] private float _timeScaleMultiplier = 1f;
    [SerializeField] private float _speedPenaltyMultiplier = 1f;
    [SerializeField] private float _speedRecoveryRate = 1f;


    private float _speedPenalty = 1;
    
    private void Start()
    {
        UIManager.Instance.UpdateHighScore(0);
        //player = FindAnyObjectByType<PlayerScore>();
    }
    private void Update()
    {
        if (goingOn)
        {
            gameTimer += Time.deltaTime;
            UIManager.Instance.UpdateTimer(gameLength - gameTimer);

            _gameData.scaledDeltaTime = Time.deltaTime * _speedPenalty * _timeScaleCurve.Evaluate(gameTimer / gameLength) * _timeScaleMultiplier;

            if (gameTimer >= gameLength)
            {
                EndGame(true);
            }

            if (_speedPenalty < 1)
            {
                RecoverSpeed();
            }
        }
    }

    public void StartGame()
    {
        gameTimer = 0f;
        goingOn = true;
    }

    public void EndGame(bool won)
    {
        goingOn = false;
        player.GetComponent<BatteryController>().isGod = false;
        UIManager.Instance.UpdateHighScore(player.scoreValue);
        Time.timeScale = 0;
        if (won)
		{
			UIManager.Instance.Victory(player.scoreValue, player.GetPickup("Tincan"), player.GetPickup("PantBurk"), player.GetPickup("Battery"));
        }
        else
        {
            UIManager.Instance.GameOver();
        }
    }

    public void ApplySpeedPenalty()
    {
        _speedPenalty *= _speedPenaltyMultiplier;
    }

    public void RecoverSpeed()
    {
        _speedPenalty += _speedRecoveryRate * Time.deltaTime;

        if (_speedPenalty > 1)
        {
            _speedPenalty = 1;
        }
    }
}
