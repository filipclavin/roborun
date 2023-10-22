using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Move : MonoBehaviour
{
    private GameTimer gameTimer;
    [Header("Settings")] public float speed = 10f;
    [Space]
    [Header("Speed Boost")]
    public float gameTime = 0;
    public List<float> speedBoostTimes = new List<float>();
    public float speedBoost = 2.5f;
    public float timeScale;
    public float maxTimeScale = 5f;
    

    void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
    }

    // Update is called once per frames
    void Update()
    {
        timeScale = Time.timeScale;
    }

    private void FixedUpdate()
    {
        if (gameTimer.goingOn)
        {
            gameTime += 1 * Time.deltaTime / Time.timeScale;
            SpeedBoostTimer();
            MoveForward();
        }
    }
    
    private void MoveForward()
    {
        if (gameTimer.goingOn)
        {
            transform.Translate(Vector3.back * (speed * Time.deltaTime));
        }
    }

    private void SpeedBoostTimer()
    {
        if(timeScale >= maxTimeScale) return;
        Time.timeScale = Mathf.Max(Mathf.Log(speedBoost * gameTime + .1f) + 1, 1);
    }

   

}