using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundGeneratorScript : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField]
    private float movingSpeed = 12;// tile Speed
    [SerializeField]
    private int tilesToPreSpawn = 15; //How many tiles should be pre-spawned
    [SerializeField]
    private int tilesWithoutObstacles = 3; //How many tiles thet should be empty on starting

    [Header("Tile Position")]

    [SerializeField]
    private Transform DespawnPoint;//point from where ground tiles will end
    [SerializeField]
    private Transform startPoint; //Point from where ground tiles will start

    [Header("Prefab")]
    [SerializeField]
    private PlatformTileScript tilePrefab;

    //Reacources
    private static GroundGeneratorScript instance;
    List<PlatformTileScript> TileList = new List<PlatformTileScript>();

    
    [HideInInspector]
    float score = 0;


    void Start()
    {
        instance = this;
        TilePreSpawning();
    }

    private void TilePreSpawning()
    {
        Vector3 spawnPosition = startPoint.position;
        int tilesWithNoObstaclesTmp = tilesWithoutObstacles;
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            spawnPosition -= tilePrefab.startPoint.localPosition;
            PlatformTileScript spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity) as PlatformTileScript;
            if (tilesWithNoObstaclesTmp > 0)
            {
                spawnedTile.DeactivateAllObstacles();
                tilesWithNoObstaclesTmp--;
            }
            else
            {
                spawnedTile.ActivateRandomObstacle();
            }

            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            TileList.Add(spawnedTile);
        }
    }

    void Update()
    {
        MoveTiles();
        DespawnTiles();
    }

    private void DespawnTiles()
    {
        if (TileList[0].endPoint.position.z < DespawnPoint.position.z)
        {
            //Move the tile to the front if it's behind the Camera
            PlatformTileScript tileTmp = TileList[0];
            TileList.RemoveAt(0);
            tileTmp.transform.position = TileList[TileList.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            tileTmp.ActivateRandomObstacle();
            TileList.Add(tileTmp);
        }
    }

    private void MoveTiles()
    {
        transform.Translate(-TileList[0].transform.forward * Time.deltaTime * (movingSpeed + (score / 500)), Space.World);
        score += Time.deltaTime * movingSpeed;
    }
}