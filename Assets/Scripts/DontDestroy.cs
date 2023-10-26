using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private readonly string leadingScoreKey = "Highscore";
    public bool skipMainMenu;

    private static DontDestroy instance;
    public static DontDestroy Instance { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        PlayerPrefs.DeleteKey(leadingScoreKey);
    }
}
