using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempUI : MonoBehaviour
{
    private readonly string leadingScoreKey = "Highscore";
    private string sceneName;
    private TMP_Text batteryText;

	[SerializeField] private InputManager input;
	[SerializeField] private Slider batteryBar;
	[Header("Texts")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
	[SerializeField] private TMP_Text timerText;
	[SerializeField] private TMP_Text victoryText;

	[Header("Panels")]
	[SerializeField] private GameObject gameOverPanel;
	[SerializeField] private GameObject victoryPanel;
	[SerializeField] private GameObject pausePanel;

	private static TempUI instance;
    public static TempUI Instance { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        sceneName = SceneManager.GetActiveScene().name;
    }

	private void Update()
	{
        if (input.controller.Movement.Pause.triggered)
        {
            Pause();
        }
	}

	private void DeleteScore() // If we wanna reset score
    {
        PlayerPrefs.DeleteKey(leadingScoreKey);
    }

    public void StartUI(float currentBattery, float maxBattery)
    {
        if (batteryBar != null)
        {
            batteryText = batteryBar.gameObject.GetComponentInChildren<TMP_Text>();
            maxBattery = currentBattery;
            batteryBar.maxValue = 0;
            batteryBar.maxValue = maxBattery;
        }
    }

    public void UpdateBatteryBar(float currentBattery)
    {
        if (batteryBar != null)
        {
            batteryBar.value = currentBattery;
            batteryText.text = "Battery: " + Mathf.RoundToInt(currentBattery) + "/" + batteryBar.maxValue;
        }
    }

    public void UpdateScore(int scoreValue)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score " + scoreValue;
        }
    }

    public void UpdateTimer(float timerLeft)
    {
        timerText.text = "Time until escape: " + Mathf.RoundToInt(timerLeft) + "s";
	}

    public void UpdateHighScore(int currentScore)
    {
        if (currentScore > PlayerPrefs.GetInt(leadingScoreKey))
        {
            PlayerPrefs.SetInt(leadingScoreKey, currentScore);
        }

        if (highScoreText != null)
        {
            highScoreText.text = "Highscore: " + PlayerPrefs.GetInt(leadingScoreKey);
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Pause()
    {
		pausePanel.SetActive(true);
		Time.timeScale = 0f;
	}

    public void Victory(int score)
    {
		victoryPanel.SetActive(true);
		Time.timeScale = 0f;
        victoryText.text = "You got " + score + " points";
	}

    public void Continue()
    {
		gameOverPanel.SetActive(false);
		pausePanel.SetActive(false);
		victoryPanel.SetActive(false);
        Time.timeScale = 1f;
	}
    public void ReloadScene()
    {
        SceneManager.LoadScene(sceneName);
		Time.timeScale = 1f;
	}
}
