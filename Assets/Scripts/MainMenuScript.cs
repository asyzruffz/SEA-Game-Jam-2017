using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour 
{
	public GameObject creditsPanel;
	// Use this for initialization
	void Start () 
	{
		SoundManager.instance.PlayBGM("BGM Title");	
		creditsPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void StartBtn()
	{
		SoundManager.instance.PlaySFX("SFX UI Click");
		SceneManager.LoadSceneAsync(1);
	}

	public void CreditsBtn()
	{
		SoundManager.instance.PlaySFX("SFX UI Click");
		creditsPanel.SetActive(true);
	}

	public void CloseCredits()
	{
		SoundManager.instance.PlaySFX("SFX UI Click");
		creditsPanel.SetActive(false);
	}
}
