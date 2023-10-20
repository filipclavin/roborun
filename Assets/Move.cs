using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 10f;
    [Header("Recourses")]
    [SerializeField] private GameObject _gameManager;
    private GameTimer _timer;
    
    void Start()
    {
        _timer = _gameManager.GetComponent<GameTimer>();
    }

    // Update is called once per framess
    void Update()
    {
        gameObject.transform.position += Vector3.back * (speed * Time.deltaTime) / _timer.gameLength;
    }
}
