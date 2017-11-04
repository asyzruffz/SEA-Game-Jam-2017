using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour 
{
	// public int sceneIndex;

	void Start () 
	{
		
	}

	public void PlayButton(int _index)
	{
		SceneManager.LoadSceneAsync(_index);
	}

	public void ExitButton()
	{
		Application.Quit();
		Debug.Log("Exit Application");
	}
}
