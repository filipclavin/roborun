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
public struct SpawnableAddressable
{
    public AssetReferenceT<GameObject> prefabAddressable;
    public float[] spawnHeights;
    public Lanes allowedLanes;
}

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public SpawnableAddressable[] spawnables;

}
