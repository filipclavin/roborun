using System.Collections.Generic;
using UnityEngine;

public class Spawnable : SpawnableAddressable
{
    public GameObject prefab;

    public Spawnable()
    {
    }
}

public class FixedSpawnable : FixedSpawnableAddressable
{
    public GameObject prefab;
    public float timer = 0f;

    public FixedSpawnable()
    {
    }
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _roadTransform;
    [SerializeField] private float _spawnDistance;
    [SerializeField] private float _despawnDistance;
    [SerializeField] private float _laneWidth;
    [SerializeField] private GameData _gameData;

    private List<Spawnable> _randomizedSpawnables = new();
    private List<FixedSpawnable> _fixedSpawnables = new();
    private List<GameObject> _spawnedObjects = new();
    private float _randomTimer = 0f;
    private float _nextRandomSpawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        LoadAdressables();
        _nextRandomSpawnInterval = Random.Range(_gameData._minSpawnInterval, _gameData._maxSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (_randomizedSpawnables.Count > 0)
        {
            _randomTimer += Time.deltaTime;

            if (_randomTimer >= _nextRandomSpawnInterval)
            {
                _randomTimer = 0f;
                SpawnRandom();
                DestroyPassed();
                _nextRandomSpawnInterval = Random.Range(_gameData._minSpawnInterval, _gameData._maxSpawnInterval);
            }
        }

        if (_fixedSpawnables.Count > 0)
        {
            TrySpawnFixed();
            DestroyPassed();
        }

    }

    void SpawnRandom()
    {

        Spawnable randomSpawnable = _randomizedSpawnables[Random.Range(0, _randomizedSpawnables.Count)];

        if (randomSpawnable.allowedLanes == 0) return;

        Vector3 spawnPosition = GenerateSpawnPosition(randomSpawnable);

        _spawnedObjects.Add(
            Instantiate(
                randomSpawnable.prefab,
                spawnPosition,
                Quaternion.identity,
                _roadTransform
            )
        );
    }

    void TrySpawnFixed()
    {
        foreach (FixedSpawnable fixedSpawnable in _fixedSpawnables)
        {
            if (fixedSpawnable.allowedLanes == 0) return;

            fixedSpawnable.timer += Time.deltaTime;

            if (fixedSpawnable.timer >= fixedSpawnable.spawnInterval * 1000)
            {
                fixedSpawnable.timer = 0f;

                Vector3 spawnPosition = GenerateSpawnPosition(fixedSpawnable);

                _spawnedObjects.Add(
                    Instantiate(
                        fixedSpawnable.prefab,
                        spawnPosition,
                        Quaternion.identity,
                        _roadTransform
                    )
                );
            }
        }
    }

    Vector3 GenerateSpawnPosition(SpawnableAddressable spawnable)
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
            _playerTransform.position.z + _spawnDistance
        );
    }

    void DestroyPassed()
    {
        foreach (GameObject item in _spawnedObjects)
        {
            if (Vector3.Dot(_playerTransform.forward, item.transform.position - _playerTransform.position) < -_despawnDistance)
            {
                _spawnedObjects.Remove(item);
                Destroy(item);
            }
        }
    }

    void LoadAdressables()
    {

        foreach (SpawnableAddressable item in _gameData.randomizedSpawnables)
        {
            item.prefabAddressable.LoadAssetAsync<GameObject>().Completed += handle =>
            {
                _randomizedSpawnables.Add(new Spawnable()
                {
                    prefab = handle.Result,
                    spawnHeights = item.spawnHeights,
                    allowedLanes = item.allowedLanes
                });
            };
        }

        foreach (FixedSpawnableAddressable item in _gameData.randomizedSpawnables)
        {
            item.prefabAddressable.LoadAssetAsync<GameObject>().Completed += handle =>
            {
                _fixedSpawnables.Add(new FixedSpawnable()
                {
                    prefab = handle.Result,
                    spawnHeights = item.spawnHeights,
                    allowedLanes = item.allowedLanes,
                    spawnInterval = item.spawnInterval
                });
            };
        }
    }
}
