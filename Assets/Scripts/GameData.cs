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
public class GameObjectAssetReference : AssetReferenceT<GameObject>
{
    public GameObjectAssetReference(string guid) : base(guid) { }
}

[Serializable]
public class Spawnable
{
    public GameObjectAssetReference prefabAddressable;
    public float[] spawnHeights = new float[0];
    public Lanes allowedLanes = Lanes.Left | Lanes.Middle | Lanes.Right;
    public float _spawnDistance = 50;
    public float _despawnDistance = 20;

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
    public Spawnable[] randomizedSpawnables = new Spawnable[0];
    public float _minSpawnInterval;
    public float _maxSpawnInterval;

    [Header("Fixed")]
    public FixedSpawnable[] fixedSpawnables = new FixedSpawnable[0];

    [NonSerialized] public float scaledDeltaTime;
}
