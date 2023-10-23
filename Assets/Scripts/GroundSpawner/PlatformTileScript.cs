using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTileScript : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject[] obstacles = new GameObject[0]; //Objects that contains different obstacle types which will be randomly activated


    public void ActivateRandomObstacle()
    {
        if (obstacles.Length == 0) return;
        DeactivateAllObstacles();
        System.Random random = new System.Random();
        int randomNumber = random.Next(0, obstacles.Length);
        obstacles[randomNumber].SetActive(true);
    }

    public void DeactivateAllObstacles()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(false);
        }
    }
}
