using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Settings")] public float speed = 10f;
    [Space]
    [Header("Speed Boost")]
    public float gameTimer = 0;
    public List<float> speedBoostTimes = new List<float>();
    public float speedBoost = 2.5f;
    public float timeScale;
    public float maxTimeScale = 5f;

    void Start()
    {
    }

    // Update is called once per frames
    void Update()
    {
        timeScale = Time.timeScale;
    }

    private void FixedUpdate()
    {
        gameTimer += 1 * Time.deltaTime / Time.timeScale;
        SpeedBoostTimer();
        MoveForward();
    }

    // private void SpeedBoostTimer()
    // {
    //     if (gameTimer % speedBoostTimes.ElementAt(0) < Time.deltaTime && timeScale < 2f)
    //     {
    //         Time.timeScale += speedBoost;
    //     }
    //
    //     if (gameTimer % speedBoostTimes.ElementAt(1) < Time.deltaTime && timeScale is < 3f and >= 2f)
    //     {
    //         Time.timeScale += speedBoost;
    //     }
    //     
    // }
    
    // private void SpeedBoostTimer()
    // {
    //     if (gameTimer % speedBoostTimes.ElementAt(0) < Time.deltaTime && speed < 20)
    //     {
    //         speed += speedBoost;
    //     }
    //
    //     if (gameTimer % speedBoostTimes.ElementAt(1) < Time.deltaTime && speed is < 30 and >= 20)
    //     {
    //         speed += speedBoost;
    //     }
    //     if (gameTimer % speedBoostTimes.ElementAt(2) < Time.deltaTime && speed is < 40 and >= 30)
    //     {
    //         speed += speedBoost;
    //     }
    //     
    // }
    private void MoveForward()
    {
        transform.Translate(Vector3.back * (speed * Time.deltaTime));
    }

    private void SpeedBoostTimer()
    {
        if(timeScale >= maxTimeScale) return;
        Time.timeScale = Mathf.Log(speedBoost * gameTimer + .1f) +1;
    }

}