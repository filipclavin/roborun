using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public void LoadScene(string sceneName)
	{
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
