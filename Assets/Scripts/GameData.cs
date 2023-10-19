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
public class Spawnable
{
    public AssetReferenceT<GameObject> prefabAddressable;
    public float[] spawnHeights;
    public Lanes allowedLanes;

    [NonSerialized] public float timer;
}

[Serializable]
public class FixedSpawnable : Spawnable
{
    public float spawnInterval;
}

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Random")]
    public Spawnable[] randomizedSpawnables;
    public float _minSpawnInterval;
    public float _maxSpawnInterval;

    [Header("Fixed")]
    public FixedSpawnable[] fixedSpawnables;
}
