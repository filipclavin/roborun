using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private readonly string leadingScoreKey = "Highscore";
    private string sceneName;
    private TMP_Text batteryText;
    private GameTimer gameTimer;
    private bool playedAnimation = false;
    private bool pauseMenuActive = false;
    private DontDestroy dontDestroy;

    [SerializeField] private CinemachineVirtualCamera gameplayCamera;
    [SerializeField] public PlayableDirector gameDirector;
	[SerializeField] private InputManager input;
	[SerializeField] private Slider batteryBar;

    [Header("Buttons")]
    private Button startGameButton;
    private Button exitGameButton;
    [SerializeField] private GameObject startGame;
	[SerializeField] private GameObject exitGame;


	[Header("Texts")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
	[SerializeField] private TMP_Text timerText;
	[SerializeField] private TMP_Text victoryText;

	[Header("Panels")]
	[SerializeField] private GameObject gameOverPanel;
	[SerializeField] private GameObject victoryPanel;
	[SerializeField] private GameObject pausePanel;

	private static UIManager instance;
    public static UIManager Instance { get { return instance; } set { instance = value; } }
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
    
	private void Start()
	{
        dontDestroy = DontDestroy.Instance;
        playedAnimation = dontDestroy.skipMainMenu;
		gameTimer = FindAnyObjectByType<GameTimer>();
        startGameButton = startGame.GetComponent<Button>();
        exitGameButton = exitGame.GetComponent<Button>();
		OpenMenu();
	}

	private void Update()
	{
        if (input.controller.Movement.Pause.triggered && !gameOverPanel.activeSelf && !victoryPanel.activeSelf)
        {
            Debug.Log("Pause");
            Pause();
        }
	}

    private void SkipMainMenu()
    {
        startGame.SetActive(false);
        exitGame.SetActive(false);

        scoreText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        victoryText.gameObject.SetActive(true);
        batteryBar.gameObject.SetActive(true);

        gameplayCamera.gameObject.SetActive(true);
        input.enabled = true;
        gameTimer.StartGame();
    }

    private void DeleteScore() // If we wanna reset score
    {
        PlayerPrefs.DeleteKey(leadingScoreKey);
    }

    private IEnumerator IntroAnimation()
    {
        gameDirector.Play();
        yield return new WaitForSeconds((float) gameDirector.playableAsset.duration);
        dontDestroy.skipMainMenu = true;
        input.enabled = true;
    }

    public void OpenMenu()
    {
        startGame.SetActive(true);
		exitGame.SetActive(true);
        input.enabled = false;
        if (playedAnimation)
        {
            SkipMainMenu();
        }
    }

	public void LoadGame()
	{
		gameTimer.StartGame();
		startGame.SetActive(false);
		exitGame.SetActive(false);
        StartCoroutine(IntroAnimation()); 
    }

	public void ExitGame()
	{
        #if UNITY_EDITOR
		    UnityEditor.EditorApplication.isPlaying = false;
        #endif
		    Application.Quit();
	}

	public void StartUI(float currentBattery, float maxBattery)
    {
        if (batteryBar != null)
        {
            batteryText = batteryBar.gameObject.GetComponentInChildren<TMP_Text>();
            maxBattery = currentBattery;
            batteryBar.maxValue = 0;
            batteryBar.maxValue = maxBattery;
            UpdateScore(0);
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
            scoreText.text = "Score: " + scoreValue;
        }
    }

    public void UpdateTimer(float timerLeft)
    {
        if (timerText != null)
        {
            timerText.text = "Time until escape: " + Mathf.RoundToInt(timerLeft) + "s";
        }
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
        pauseMenuActive = !pauseMenuActive;
        if (pauseMenuActive)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Continue();
        }

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
