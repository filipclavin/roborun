using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private float _spawnDistance;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _minSpawnInterval;
    [SerializeField] private float _maxSpawnInterval;
    [SerializeField] private float _laneWidth;

    private List<GameObject> _spawnedObjects = new List<GameObject>();
    private float _timer = 0;
    private float _nextSpawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        _nextSpawnInterval = Random.Range(_minSpawnInterval, _maxSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _nextSpawnInterval)
        {
            _timer = 0;
            DestroyAndSpawn();
            _nextSpawnInterval = Random.Range(_minSpawnInterval, _maxSpawnInterval);
        }
    }

    void DestroyAndSpawn()
    {
        List<GameObject> objectsBehindPlayer = _spawnedObjects.FindAll(
            obj => Vector3.Dot(_playerTransform.forward, obj.transform.position - _playerTransform.position) < 0
        );

        if (objectsBehindPlayer.Count > 0)
        {
            _spawnedObjects.RemoveRange(0, objectsBehindPlayer.Count);
            objectsBehindPlayer.ForEach(obj => Destroy(obj));
        }

        // If random number is less than 1/3, spawn on left lane
        // If random number is less than 2/3, spawn on middle lane
        // Otherwise, spawn on right lane
        Vector3 spawnPosition = new(
            Random.Range(0f, 1f) < 1 / 3f ? -_laneWidth :
                Random.Range(0f, 1f) < 2 / 3f ? 0 :
                    _laneWidth,
            0,
            _playerTransform.position.z + _spawnDistance
        );

        _spawnedObjects.Add(
            Instantiate(
                _prefabToSpawn,
                spawnPosition,
                Quaternion.identity
            )
        );
    }
}
