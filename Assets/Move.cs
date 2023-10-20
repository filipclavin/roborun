using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 10f;
    [SerializeField] private float _maxSpeed = 20f;
    [SerializeField] private GameObject _gameManager;
    private GameTimer _timer;
    
    void Start()
    {
        _timer = _gameManager.GetComponent<GameTimer>();
    }

    // Update is called once per framess
    void Update()
    {
        Vector3 moveVector = Vector3.back * (_baseSpeed * Time.deltaTime) / _timer.gameLength;
        gameObject.transform.position += moveVector;
    }
}
