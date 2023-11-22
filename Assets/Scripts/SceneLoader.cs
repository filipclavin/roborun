using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
	public void LoadScene(string sceneName)
	{
		if(sceneName == "MainMenu")
		{
			UIManager.Instance.QuitToMenu();
			DontDestroy.Instance.skipMainMenu = false;
		}
		
		SceneManager.LoadScene(sceneName);
	}

	public void ExitGame()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
			Application.Quit();
	}
}
