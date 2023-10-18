using System.Collections.Generic;
using UnityEngine;

public struct Spawnable
{
    public GameObject prefab;
    public float[] spawnHeights;
    public Lanes allowedLanes;

    public Spawnable(GameObject prefab, float[] spawnHeights, Lanes allowedLanes)
    {
        this.prefab = prefab;
        this.spawnHeights = spawnHeights;
        this.allowedLanes = allowedLanes;
    }
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _spawnDistance;
    [SerializeField] private float _minSpawnInterval;
    [SerializeField] private float _maxSpawnInterval;
    [SerializeField] private float[] _spawnHeights;
    [SerializeField] private float _laneWidth;
    [SerializeField] private GameData _gameData;

    private List<Spawnable> _spawnables = new();
    private List<GameObject> _spawnedObjects = new();
    private float _timer = 0;
    private float _nextSpawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        LoadAdressables();
        _nextSpawnInterval = Random.Range(_minSpawnInterval, _maxSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnables.Count == 0) return;

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

        Spawnable randomSpawnable = _spawnables[Random.Range(0, _spawnables.Count)];

        if (randomSpawnable.allowedLanes == 0) return;

        float[] xPositions = randomSpawnable.allowedLanes switch
        {
            Lanes.Left => new float[] { -_laneWidth },
            Lanes.Middle => new float[] { 0 },
            Lanes.Right => new float[] { _laneWidth },
            Lanes.Left | Lanes.Middle => new float[] { -_laneWidth, 0 },
            Lanes.Left | Lanes.Right => new float[] { -_laneWidth, _laneWidth },
            Lanes.Middle | Lanes.Right => new float[] { 0, _laneWidth },
            _ => new float[] { -_laneWidth, 0, _laneWidth }
        };

        Vector3 spawnPosition = new(
            xPositions[Random.Range(0, xPositions.Length)],
            randomSpawnable.spawnHeights[Random.Range(0, randomSpawnable.spawnHeights.Length)],
            _playerTransform.position.z + _spawnDistance
        );

        _spawnedObjects.Add(
            Instantiate(
                randomSpawnable.prefab,
                spawnPosition,
                Quaternion.identity
            )
        );
    }

    void LoadAdressables()
    {

        foreach (SpawnableAddressable item in _gameData.spawnables)
        {
            Debug.Log(item.allowedLanes == (Lanes.Middle | Lanes.Right));

            item.prefabAddressable.LoadAssetAsync<GameObject>().Completed += handle =>
            {
                _spawnables.Add(new Spawnable()
                {
                    prefab = handle.Result,
                    spawnHeights = item.spawnHeights,
                    allowedLanes = item.allowedLanes
                });
            };
        }

    }
}
