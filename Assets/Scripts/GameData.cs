using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Flags]
public enum Lanes
{
    Left = 1 << 0,
    Middle = 1 << 1,
    Right = 1 << 2
}

[Serializable]
public class SpawnableAddressable
{
    public AssetReferenceT<GameObject> prefabAddressable;
    public float[] spawnHeights;
    public Lanes allowedLanes;
}

[Serializable]
public class FixedSpawnableAddressable : SpawnableAddressable
{
    public float spawnInterval;
}

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Random")]
    public SpawnableAddressable[] randomizedSpawnables;
    public float _minSpawnInterval;
    public float _maxSpawnInterval;

    [Header("Fixed")]
    public FixedSpawnableAddressable[] fixedSpawnables;
}
