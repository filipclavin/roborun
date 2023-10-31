using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private Slider timerBar;
    [SerializeField] private Image robotPortrait;

    [Header("Face Animation")]
    public Animator faceAnimator;
    [Space]

    [Header("Buttons")]
    [SerializeField] private Canvas startMenu;

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
        // startGame.SetActive(false);
        // exitGame.SetActive(false);

        /*
        scoreText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        victoryText.gameObject.SetActive(true);
        batteryBar.gameObject.SetActive(true);
        robotPortrait.gameObject.SetActive(true);
        */

        gameplayCamera.gameObject.SetActive(true);
        input.enabled = true;
        gameTimer.StartGame();
    }

    private IEnumerator IntroAnimation()
    {
        gameDirector.Play();
        yield return new WaitForSeconds((float) gameDirector.playableAsset.duration);
        //dontDestroy.skipMainMenu = true;
        input.enabled = true;
    }

    public void OpenMenu()
    {
        startMenu.gameObject.SetActive(true);
        input.enabled = false;
        if (playedAnimation)
        {
            SkipMainMenu();
        }
    }

	public void LoadGame()
	{
		gameTimer.StartGame();
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
            UpdateScore(0, 1);
        }

        if (timerBar != null)
        {
            gameTimer = FindAnyObjectByType<GameTimer>();
            timerBar.minValue = 0;
            timerBar.maxValue = gameTimer.gameLength;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "Recordwatt: " + PlayerPrefs.GetInt(leadingScoreKey);
        }
    }

    public void UpdateBatteryBar(float currentBattery)
    {
        if (batteryBar != null)
        {
            batteryBar.value = currentBattery;
            batteryText.text = Mathf.RoundToInt(currentBattery) + "%";
        }
    }

    public void UpdateScore(int scoreValue, float multiplier)
    {
        if (scoreText != null)
        {
            if (multiplier != 1)
            {
                scoreText.text = "Watthours saved: " + multiplier + "X " + scoreValue;
            }
            else
            {
                scoreText.text = "Watthours saved: " + scoreValue;
            }
        }
    }

    public void UpdateTimer(float timerLeft)
    {
        if (timerText != null)
        {
            timerText.text = "Time remaining: " + Mathf.RoundToInt(timerLeft) + "s";
        }

        if (timerBar != null)
        {
            timerBar.value = timerBar.maxValue - timerLeft;
        }
	}

    public void UpdateHighScore(int currentScore)
    {
        if (currentScore > PlayerPrefs.GetInt(leadingScoreKey))
        {
            PlayerPrefs.SetInt(leadingScoreKey, currentScore);
            if (highScoreText != null)
            {
                highScoreText.text = "Recordwatt: " + PlayerPrefs.GetInt(leadingScoreKey);
            }
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
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }

	}

    public void Victory(int score)
    {
		victoryPanel.SetActive(true);
		Time.timeScale = 0f;
        victoryText.text = "You saved " + score + " watthours during your game";
	}

    public void ReloadScene()
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
	}
}
