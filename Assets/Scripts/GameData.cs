using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


[Serializable]
public struct SpawnableAddressable
{
    public AssetReferenceT<GameObject> prefabAddressable;
    public float[] spawnHeights;
}

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public SpawnableAddressable[] spawnables;

}
