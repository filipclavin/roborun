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