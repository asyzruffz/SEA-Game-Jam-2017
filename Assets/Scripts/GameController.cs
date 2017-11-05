using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

	protected override void SingletonAwake () {
		//DontDestroyOnLoad (gameObject);
	}

	public bool isPlaying;
	public bool isGameOver;

	[Header("Reference")]
	public CharacterMovement player;
	public FloorTileManager floor;

	void Start () {
		
	}
	
	void Update () {
		
	}

	public void StartGame () {
		isPlaying = true;
		floor.startTile.OnDestroyed ();
	}
	
}
