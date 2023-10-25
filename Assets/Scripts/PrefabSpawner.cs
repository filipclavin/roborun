using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

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

        while (SpawnedInsideOther(handle.Result))
        {
            spawnable.allowedLanes &= handle.Result.transform.position.x == -_laneWidth ? ~Lanes.Left :
                handle.Result.transform.position.x == 0f ? ~Lanes.Middle
                    : ~Lanes.Right;

            if (spawnable.allowedLanes == 0)
            {
                spawnable.spawnHeights = spawnable.spawnHeights.Where(h => h != handle.Result.transform.position.y).ToArray();

                if (spawnable.spawnHeights.Length == 0)
                {
                    Destroy(handle.Result);
                    return;
                }
            }

            Vector3 newPosition = GenerateSpawnPosition(spawnable);
            handle.Result.transform.position = newPosition;
        }

        handle.Result.AddComponent<SpawnableMonoBehaviour>().spawnable = spawnable;
        _spawnedObjects.Add(handle.Result);
    }

    private bool SpawnedInsideOther(GameObject spawnedObject)
    {
        List<Collider> cols = new();
        bool colFound = spawnedObject.TryGetComponent(out Collider col);
        if (colFound) cols.Add(col);
        cols.AddRange(spawnedObject.GetComponentsInChildren<Collider>());

        List<Collider> hitCols = new();

        cols.ForEach(c => c.enabled = false);
        foreach (Collider c in cols)
        {
            hitCols.AddRange(Physics.OverlapBox(
                c.bounds.center,
                c.bounds.extents,
                Quaternion.identity,
                LayerMask.NameToLayer("Ground"),
                QueryTriggerInteraction.Collide)
            );
        }
        cols.ForEach(c => c.enabled = true);

        return hitCols.Count > 0;
    }
}
