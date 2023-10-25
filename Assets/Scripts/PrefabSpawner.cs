using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PrefabSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _laneWidth;
    [SerializeField] private GameData _gameData;
    [Header("Refrences")]
    [SerializeField] private Transform _playerTransform;

    private List<Spawnable> _allSpawnables = new();
    private List<GameObject> _spawnedObjects = new();
    private float _randomTimer = 0f;
    private float _nextRandomSpawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        _nextRandomSpawnInterval = UnityEngine.Random.Range(_gameData._minSpawnInterval, _gameData._maxSpawnInterval);
        _allSpawnables.AddRange(_gameData.randomizedSpawnables);
        _allSpawnables.AddRange(_gameData.fixedSpawnables);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameData.randomizedSpawnables.Length > 0)
        {
            _randomTimer += _gameData.scaledDeltaTime;

            if (_randomTimer >= _nextRandomSpawnInterval)
            {
                _randomTimer = 0f;
                SpawnRandom();
                DestroyPassed();
                _nextRandomSpawnInterval = UnityEngine.Random.Range(_gameData._minSpawnInterval, _gameData._maxSpawnInterval);
            }
        }

        if (_gameData.fixedSpawnables.Length > 0)
        {
            TrySpawnFixed();
            DestroyPassed();
        }

    }

    void SpawnRandom()
    {
        Spawnable[] randomizedSpawnables = _gameData.randomizedSpawnables;

        Spawnable randomSpawnable = randomizedSpawnables[UnityEngine.Random.Range(0, randomizedSpawnables.Length)];

        if (randomSpawnable.allowedLanes == 0) return;

        Vector3 spawnPosition = GenerateSpawnPosition(randomSpawnable);

        Transform roadTile = GameObject.FindGameObjectsWithTag(_gameData.roadTileTag)
            .Select(obj => obj.transform)
            .OrderBy(tra => (spawnPosition - tra.position).magnitude)
            .First();

        Addressables.InstantiateAsync(
            randomSpawnable.prefabAddressable,
            spawnPosition,
            Quaternion.identity,
            roadTile
        ).Completed += handle => HandleSpawned(handle, randomSpawnable);
    }

    void TrySpawnFixed()
    {
        foreach (FixedSpawnable fixedSpawnable in _gameData.fixedSpawnables)
        {
            if (fixedSpawnable.allowedLanes == 0) return;

            fixedSpawnable.timer += _gameData.scaledDeltaTime;

            if (fixedSpawnable.timer >= fixedSpawnable.spawnInterval)
            {
                fixedSpawnable.timer = 0f;

                Vector3 spawnPosition = GenerateSpawnPosition(fixedSpawnable);

                Transform roadTile = GameObject.FindGameObjectsWithTag(_gameData.roadTileTag)
                    .Select(obj => obj.transform)
                    .OrderBy(tra => (spawnPosition - tra.position).magnitude)
                    .First();

                Addressables.InstantiateAsync(
                    fixedSpawnable.prefabAddressable,
                    spawnPosition,
                    Quaternion.identity,
                    roadTile
                ).Completed += handle => HandleSpawned(handle, fixedSpawnable);
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
            xPositions[UnityEngine.Random.Range(0, xPositions.Length)],
            spawnable.spawnHeights[UnityEngine.Random.Range(0, spawnable.spawnHeights.Length)],
            _playerTransform.position.z + spawnable._spawnDistance
        ); 
    }

    void DestroyPassed()
    {
        List<GameObject> _objectsBehindPlayer = _spawnedObjects.FindAll(
            item => Vector3.Dot(_playerTransform.forward, item.transform.position - _playerTransform.position) < -item.GetComponent<SpawnableMonoBehaviour>().spawnable._despawnDistance
        );

        foreach (GameObject item in _objectsBehindPlayer)
        {
            _spawnedObjects.Remove(item);
            Destroy(item);
        }
    }

    private void HandleSpawned(AsyncOperationHandle<GameObject> handle, Spawnable spawnable)
    {
        handle.Result.AddComponent<SpawnableMonoBehaviour>().spawnable = spawnable;

        List<Collider> cols = new();
        bool colFound = handle.Result.TryGetComponent(out Collider col);
        if (colFound) cols.Add(col);
        cols.AddRange(handle.Result.GetComponentsInChildren<Collider>());

        cols.ForEach(col => col.enabled = false);
        Collider[] hitCols = Physics.OverlapSphere(cols[0].bounds.center,
                                                    cols[0].bounds.extents.magnitude,
                                                    LayerMask.NameToLayer("Ground"),
                                                    QueryTriggerInteraction.Collide);
        cols.ForEach(col => col.enabled = true);

        if (hitCols.Length > 0)
        {
            // figure out what lanes the hitCols span and move new object to a different lane
            // or just above the hitCols if all allowed lanes are blocked
            //Debug.Log($"{handle.Result.name} spawned inside something else at " + handle.Result.transform.position);
            float originalY = handle.Result.transform.position.y;

            foreach (Collider hitCol in hitCols)
            {
                //Debug.Log("Hit thing: " + hitCol.name);

                if (hitCol.bounds.extents.x * 2 <= _laneWidth)
                {
                    if (hitCol.bounds.center.x < 0)
                    {
                        spawnable.allowedLanes &= ~Lanes.Left;
                    }
                    else if (hitCol.bounds.center.x > 0)
                    {
                        spawnable.allowedLanes &= ~Lanes.Right;
                    }
                    else
                    {
                        spawnable.allowedLanes &= ~Lanes.Middle;
                    }
                }
                else if (hitCol.bounds.extents.x * 2 <= 2 * _laneWidth)
                {
                    if (hitCol.bounds.center.x < 0)
                    {
                        spawnable.allowedLanes &= ~(Lanes.Left | Lanes.Middle);
                    }
                    else if (hitCol.bounds.center.x > 0)
                    {
                        spawnable.allowedLanes &= ~(Lanes.Right | Lanes.Middle);
                    }
                }
                else
                {
                    spawnable.allowedLanes = 0;

                    if (originalY + 2 * hitCol.bounds.extents.y > handle.Result.transform.position.y)
                    {
                        handle.Result.transform.position = new Vector3(
                            handle.Result.transform.position.x,
                            originalY + 2 * hitCol.bounds.extents.y,
                            handle.Result.transform.position.z
                        );
                    }
                }
            }

            if (spawnable.allowedLanes != 0)
            {
                handle.Result.transform.position = GenerateSpawnPosition(spawnable);
            }

            Debug.Log("Moving to " + handle.Result.transform.position);
        }

        _spawnedObjects.Add(handle.Result);
    }


}
