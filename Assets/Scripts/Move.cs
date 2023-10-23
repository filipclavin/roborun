using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Move : MonoBehaviour
{
    private GameTimer gameTimer;
    public float speed = 10f;

    [SerializeField] private GameData gameData;

    void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
    }

    private void FixedUpdate()
    {
        if (gameTimer.goingOn)
        {
            MoveForward();
        }
    }
    
    private void MoveForward()
    {
        transform.Translate(Vector3.back * (speed * gameData.scaledDeltaTime));
    }

}