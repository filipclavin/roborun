using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "AllAddressables", menuName = "ScriptableObjects/AllAdressables", order = 1)]
public class AllAdressables : ScriptableObject
{
    public AssetReference obstaclePrefab;
    public AssetReference collectablePrefab;

}
