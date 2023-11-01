using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private float multipliertimer;
    private float multiplierDuration;
    private readonly float baseMultipler = 1;
    private float multiplier;
    private int tincanCount;
    private int pantBurkCount;

    public int CurrentScore { get; private set; }

    public int scoreValue;

    private void Start()
    {
        multiplier = baseMultipler;
    }

    private void Update()
    {
        scoreValue = CurrentScore;
        if (multiplier != baseMultipler)
        {
            multipliertimer += Time.deltaTime;
            if (multipliertimer >= multiplierDuration)
            {
                multiplier = 1;
                UIManager.Instance.UpdateScore(CurrentScore, multiplier);
            }
        }
    }

    public void AddScore(int score)
    {
        CurrentScore += (int)(score * multiplier);
        UIManager.Instance.UpdateScore(CurrentScore, multiplier);
    }

    public void AddPickup(string name)
    {
        switch (name)
        {
            case string s when s.StartsWith("Tincan"):
                tincanCount++;
                UIManager.Instance.UpdatePickup("Tincan", tincanCount.ToString());
                break;
            case string s when s.StartsWith("PantBurk"):
                pantBurkCount++;
                UIManager.Instance.UpdatePickup("PantBurk", pantBurkCount.ToString());
                break;
            default:
                Debug.LogError("Unknown pickup name: " + name);
                break;
        }
    }

    public void PowerUpMultiplier(float duration, float pickUpMultiplier)
    {
        if (multiplier == baseMultipler)
        {
            multiplier *= pickUpMultiplier;
        }
        multiplierDuration = duration;
        multipliertimer = 0;
        UIManager.Instance.UpdateScore(CurrentScore, multiplier);
    }
}
