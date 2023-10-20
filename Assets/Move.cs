using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 10f;
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
