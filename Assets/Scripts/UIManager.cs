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
    [SerializeField] private BatteryController battery;

    [SerializeField] private CinemachineVirtualCamera gameplayCamera;
    [SerializeField] public PlayableDirector startDirector;
    [SerializeField] public PlayableDirector pauseDirector;
    [SerializeField] public PlayableDirector continueDirector;
    [SerializeField] private InputManager input;
    [SerializeField] private Slider batteryBar;
    [SerializeField] private Image forestImage;
    [SerializeField] private Image robotPortrait;

    [Header("Face Animation")]
    public Animator faceAnimator;
    [Space]

    [Header("Buttons")]
    [SerializeField] private Canvas startMenu;
    [SerializeField] private Canvas UIGame;

    [Header("Texts")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text batteryCountText;
    [SerializeField] private TMP_Text tincanCountText;
    [SerializeField] private TMP_Text pantBurkCountText;

    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
	[SerializeField] private TMP_Text batteryCountVictory;
	[SerializeField] private TMP_Text tincanCountVictory;
	[SerializeField] private TMP_Text pantBurkCountVictory;
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

    [HideInInspector] public bool isPaused = false;  

    private void Update()
    {
        if (input.controller.Movement.Pause.triggered && !gameOverPanel.activeSelf && !victoryPanel.activeSelf)
        {
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
        startDirector.Play();
        yield return new WaitForSeconds((float)startDirector.playableAsset.duration);
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
        Time.timeScale = 1f;
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

        if (forestImage != null)
        {
            gameTimer = FindAnyObjectByType<GameTimer>();
            forestImage.fillAmount = 0;
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

    public void UpdatePickup(string type, string count)
    {
        switch (type)
        {
            case "Tincan":
                tincanCountText.text = count;
                break;
            case "PantBurk":
                pantBurkCountText.text = count;
                break;
            case "Battery":
                batteryCountText.text = count;
                break;
        }
    }

    public void UpdateTimer(float timerLeft)
    {
        if (forestImage != null)
        {
            forestImage.fillAmount = gameTimer.gameTimer / gameTimer.gameLength;
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
        UIGame.gameObject.SetActive(false);
        //pauseDirector.Play();
        AudioManager.Instance.StopPizzaTheme();
        AudioManager.Instance.sounds[0].source.pitch = 1;
    }

    public void Pause()
    {
        AudioManager.Instance.sounds[0].source.volume = 0.25f;
        isPaused = !isPaused;  
    
        pauseMenuActive = !pauseMenuActive;
        if (pauseMenuActive)
        {
            UIGame.gameObject.SetActive(false);
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else 
        {
            AudioManager.Instance.sounds[0].source.volume = 0.5f;
            UIGame.gameObject.SetActive(true);
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }


    public void Victory(int score, int tincan, int pantburk, int battery)
    {
        AudioManager.Instance.StopPizzaTheme();
        AudioManager.Instance.sounds[0].source.pitch = 1;
        //pauseDirector.Play();
        UIGame.gameObject.SetActive(false);
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
        victoryText.text = "You saved " + score + " watthours during your game";
        tincanCountVictory.text = tincan.ToString();
		pantBurkCountVictory.text = pantburk.ToString();
		batteryCountVictory.text = battery.ToString();

	}
    public bool isReloaded = false;
    public void ReloadScene()
    {
        AudioManager.Instance.StopPizzaTheme();
        AudioManager.Instance.sounds[0].source.pitch = 1;
        isReloaded = true;
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
}
