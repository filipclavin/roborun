using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float _speedScalar = 10f;
    [SerializeField] private float _maxVectorMagnitude = .5f;
    [SerializeField] private GameObject _gameManager;
    private GameTimer _timer;
    
    void Start()
    {
        _timer = _gameManager.GetComponent<GameTimer>();
    }

    // Update is called once per framess
    void Update()
    {
        Vector3 moveVector = Vector3.back * (_speedScalar * Time.deltaTime) / _timer.gameLength;
        Debug.Log(moveVector.magnitude);

        if (moveVector.magnitude > _maxVectorMagnitude)
        {
            moveVector = moveVector.normalized * _maxVectorMagnitude;
        }
        gameObject.transform.position += moveVector;
    }
}
