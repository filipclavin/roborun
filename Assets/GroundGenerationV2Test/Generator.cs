using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generator : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject prefab;

    [Header("Settings")]
    [SerializeField] private float tileMoveSpeed = 5.0f;
    [SerializeField] private int startingTiles = 10;

    private List<GameObject> tileList = new();
    private Vector3 spawnPosition;

    void Start()
    {
        spawnPosition = Vector3.zero;
        PreSpawnTiles();
    }

    void Update()
    {
        if (spawnPosition.z < transform.position.z)
        {
            SpawnTile();
        }

        MoveTiles();
    }

    void PreSpawnTiles()
    {
        for (int i = 0; i < startingTiles; i++)
        {
            SpawnTile();
        }
    }

    void SpawnTile()
    {
        Vector3 tilePosition = spawnPosition;

        GameObject newTile = Instantiate(prefab, tilePosition, Quaternion.identity);
        tileList.Add(newTile);

        float tileLength = newTile.GetComponent<Renderer>().bounds.size.z;
        spawnPosition.z += tileLength;
    }

    void MoveTiles()
    {
        foreach (GameObject tile in tileList)
        {
            tile.transform.Translate(Vector3.back * tileMoveSpeed * Time.deltaTime);

            if (tile.transform.position.z + tile.GetComponent<Renderer>().bounds.size.z / 2 < transform.position.z)
            {
                MoveTileToFront(tile);
            }
        }
    }

    void MoveTileToFront(GameObject tile)
    {
        float tileLength = tile.GetComponent<Renderer>().bounds.size.z;
        Vector3 newPosition = new Vector3(0, 0, tileList[tileList.Count - 1].transform.position.z + tileLength);
        tile.transform.position = newPosition;
    }
}