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
    [SerializeField] private GameTimer _gameTimer;

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
        if (_gameData.randomizedSpawnables.Length > 0 && Time.timeScale != 0f && _gameTimer.goingOn)
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

        if (_gameData.fixedSpawnables.Length > 0 && Time.timeScale != 0f && _gameTimer.goingOn)
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
            (Lanes)(-7) => new float[] { -_laneWidth },
            Lanes.Middle => new float[] { 0 },
            (Lanes)(-6) => new float[] { 0 },
            Lanes.Right => new float[] { _laneWidth },
            (Lanes)(-4) => new float[] { _laneWidth },
            (Lanes.Left | Lanes.Middle) => new float[] { -_laneWidth, 0 },
            (Lanes)(-5) => new float[] { -_laneWidth, 0 },
            (Lanes.Left | Lanes.Right) => new float[] { -_laneWidth, _laneWidth },
            (Lanes)(-3) => new float[] { -_laneWidth, _laneWidth },
            (Lanes.Middle | Lanes.Right) => new float[] { 0, _laneWidth },
            (Lanes)(-2) => new float[] { 0, _laneWidth },
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
        _oldPos = Vector3.zero;
        _oldColCenters.Clear();

        Spawnable tempSpawnable = new Spawnable();
        tempSpawnable.allowedLanes = spawnable.allowedLanes;
        tempSpawnable.spawnHeights = spawnable.spawnHeights;
        tempSpawnable._spawnDistance = spawnable._spawnDistance;

        while (SpawnedInsideOther(handle.Result))
        {
            float roundedX = (float)Math.Round(handle.Result.transform.position.x, 1);
            tempSpawnable.allowedLanes &= roundedX == -_laneWidth ? ~Lanes.Left :
                roundedX == 0f ? ~Lanes.Middle
                    : ~Lanes.Right;

            if (tempSpawnable.allowedLanes == 0 || tempSpawnable.allowedLanes == (Lanes)(-8))
            {
                float roundedY = (float)Math.Round(handle.Result.transform.position.y, 1);
                tempSpawnable.spawnHeights = tempSpawnable.spawnHeights.Where(h => h != handle.Result.transform.position.y).ToArray();

                if (tempSpawnable.spawnHeights.Length == 0)
                {
                    Destroy(handle.Result);
                    return;
                }
            }

            Vector3 newPosition = GenerateSpawnPosition(tempSpawnable);
            handle.Result.transform.position = newPosition;
        }

        handle.Result.AddComponent<SpawnableMonoBehaviour>().spawnable = spawnable;
        _spawnedObjects.Add(handle.Result);
    }

    private Vector3 _oldPos = Vector3.zero;
    private List<Vector3> _oldColCenters = new();
    private bool SpawnedInsideOther(GameObject spawnedObject)
    {
        List<Collider> cols = new();
        cols.AddRange(spawnedObject.GetComponentsInChildren<Collider>());

        List<Vector3> colCenters = new();
        List<Vector3> extents = new();

        List<Collider> hitCols = new();

        for (int i = 0; i < cols.Count; i++)
        {
            Collider c = cols[i];

            if (_oldColCenters.Count > 0)
            {
                System.Diagnostics.Debugger.Break();
            }

            if (
                    _oldColCenters.Count > 0 &&
                    _oldColCenters[i].x == c.bounds.center.x &&
                    _oldColCenters[i].y == c.bounds.center.y
                )
            {
                Vector3 diff = spawnedObject.transform.position - _oldPos;

                colCenters.Add(c.bounds.center + diff);
            }
            else
            {
                colCenters.Add(c.bounds.center);
            }

            extents.Add(c.bounds.extents);
            c.enabled = false;
        }

        for (int i = 0; i < cols.Count; i++)
        {
            List<Collider> hits = Physics.OverlapBox(
                colCenters[i],
                extents[i],
                spawnedObject.transform.rotation,
                LayerMask.GetMask("Default", "OutlineObjects"),
                QueryTriggerInteraction.Collide
            ).ToList();

            hitCols.AddRange(hits);
        }

        _oldPos = spawnedObject.transform.position;
        _oldColCenters.Clear();
        _oldColCenters.AddRange(colCenters);
        cols.ForEach(c => c.enabled = true);

        return hitCols.Count > 0;
    }
}
