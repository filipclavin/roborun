using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _roadTransform;
    [SerializeField] private float _laneWidth;
    [SerializeField] private GameData _gameData;

    private List<Spawnable> _allSpawnables = new();
    private List<GameObject> _spawnedObjects = new();
    private float _randomTimer = 0f;
    private float _nextRandomSpawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        _nextRandomSpawnInterval = Random.Range(_gameData._minSpawnInterval, _gameData._maxSpawnInterval);
        _allSpawnables.AddRange(_gameData.randomizedSpawnables);
        _allSpawnables.AddRange(_gameData.fixedSpawnables);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameData.randomizedSpawnables.Length > 0)
        {
            Debug.Log("Random");
            _randomTimer += Time.deltaTime;

            if (_randomTimer >= _nextRandomSpawnInterval)
            {
                _randomTimer = 0f;
                SpawnRandom();
                DestroyPassed();
                _nextRandomSpawnInterval = Random.Range(_gameData._minSpawnInterval, _gameData._maxSpawnInterval);
            }
        }

        if (_gameData.fixedSpawnables.Length > 0)
        {
            Debug.Log("Fixed");
            TrySpawnFixed();
            DestroyPassed();
        }

    }

    void SpawnRandom()
    {
        Spawnable[] randomizedSpawnables = _gameData.randomizedSpawnables;

        Spawnable randomSpawnable = randomizedSpawnables[Random.Range(0, randomizedSpawnables.Length)];

        if (randomSpawnable.allowedLanes == 0) return;

        Vector3 spawnPosition = GenerateSpawnPosition(randomSpawnable);

        Addressables.InstantiateAsync(
            randomSpawnable.prefabAddressable,
            spawnPosition,
            Quaternion.identity,
            _roadTransform
        ).Completed += handle =>
        {
            handle.Result.AddComponent<SpawnableDataContainer>().spawnable = randomSpawnable;
            _spawnedObjects.Add(handle.Result);
        };
    }

    void TrySpawnFixed()
    {
        foreach (FixedSpawnable fixedSpawnable in _gameData.fixedSpawnables)
        {
            if (fixedSpawnable.allowedLanes == 0) return;

            fixedSpawnable.timer += Time.deltaTime;

            if (fixedSpawnable.timer >= fixedSpawnable.spawnInterval)
            {
                fixedSpawnable.timer = 0f;

                Vector3 spawnPosition = GenerateSpawnPosition(fixedSpawnable);

                Addressables.InstantiateAsync(
                    fixedSpawnable.prefabAddressable,
                    spawnPosition,
                    Quaternion.identity,
                    _roadTransform
                ).Completed += handle =>
                {
                    handle.Result.AddComponent<SpawnableDataContainer>().spawnable = fixedSpawnable;
                    _spawnedObjects.Add(handle.Result);
                };
            }
        }
    }

    Vector3 GenerateSpawnPosition(Spawnable spawnable)
    {
        float[] xPositions = spawnable.allowedLanes switch
        {
            Lanes.Left => new float[] { -_laneWidth },
            Lanes.Middle => new float[] { 0 },
            Lanes.Right => new float[] { _laneWidth },
            Lanes.Left | Lanes.Middle => new float[] { -_laneWidth, 0 },
            Lanes.Left | Lanes.Right => new float[] { -_laneWidth, _laneWidth },
            Lanes.Middle | Lanes.Right => new float[] { 0, _laneWidth },
            _ => new float[] { -_laneWidth, 0, _laneWidth }
        };

        return new Vector3(
            xPositions[Random.Range(0, xPositions.Length)],
            spawnable.spawnHeights[Random.Range(0, spawnable.spawnHeights.Length)],
            _playerTransform.position.z + spawnable._spawnDistance
        );
    }

    void DestroyPassed()
    {
        List<GameObject> _objectsBehindPlayer = _spawnedObjects.FindAll(
            item => Vector3.Dot(_playerTransform.forward, item.transform.position - _playerTransform.position) < -item.GetComponent<SpawnableDataContainer>().spawnable._despawnDistance
        );

        foreach (GameObject item in _objectsBehindPlayer)
        {
            _spawnedObjects.Remove(item);
            Destroy(item);
        }
    }
}
