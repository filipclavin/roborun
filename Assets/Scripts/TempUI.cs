using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempUI : MonoBehaviour
{
    private readonly string leadingScoreKey = "Highscore";
    private TMP_Text batteryText;
    [SerializeField] private Slider batteryBar;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text tempHighScore;

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

    public void UpdateHighScore(int currentScore)
    {
        if (currentScore > PlayerPrefs.GetInt(leadingScoreKey))
        {
            PlayerPrefs.SetInt(leadingScoreKey, currentScore);
        }

        if (tempHighScore != null)
        {
            tempHighScore.text = "Highscore: " + PlayerPrefs.GetInt(leadingScoreKey);
        }
    }
}
