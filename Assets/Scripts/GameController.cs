using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController> {

	protected override void SingletonAwake () {
		//DontDestroyOnLoad (gameObject);
	}

	public bool isPlaying;
	public bool isGameOver;
	public int score;

	[Header("Reference")]
	public CharacterMovement player;
	public FloorTileManager floor;

	float hiddenScore;

	void Start () {
		SoundManager.instance.PlayBGM ("BGM Gameplay");
	}
	
	void Update () {
		if (player != null) {
			hiddenScore = Mathf.Max (hiddenScore, player.transform.position.z);
		}
		score = (int)(hiddenScore * 10);
	}

	public void StartGame () {
		isPlaying = true;
		isGameOver = false;
		score = 0;
		hiddenScore = 0;
		floor.startTile.OnDestroyed ();
	}
	
	public void GameIsOver () {
		isPlaying = false;
		SceneManager.LoadScene ("MainMenu");
	}
}
