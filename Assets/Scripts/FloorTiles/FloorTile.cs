﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloorTile : MonoBehaviour {

	public float lifetime = 4;

	[Header("Neighbours")]
	public FloorTile upTile;
	public FloorTile downTile;
	public FloorTile rightTile;
	public FloorTile leftTile;

	[HideInInspector]
	public bool isSpecial;
	[HideInInspector]
	public bool isCheckpoint;
	[HideInInspector]
	public FloorTileManager manager;
	Vector3 oriPos;
	bool onceGenerated;
	bool startedLife;
	bool isGone;

	protected virtual void Update () {
		if (GameController.Instance != null) {
			if (!startedLife && GameController.Instance.isPlaying) {
				startedLife = true;
			}
		}

		if (lifetime <= 0) {
			OnDestroyed ();
		}

		if (startedLife) {
			lifetime -= Time.deltaTime;
		}
	}

	public bool IsTileValidAt (TileDirection direction) {
		switch (direction) {
			default:
				return false;
			case TileDirection.Middle:
				return true;
			case TileDirection.Up:
				return upTile != null;
			case TileDirection.Down:
				return downTile != null;
			case TileDirection.Right:
				return rightTile != null;
			case TileDirection.Left:
				return leftTile != null;
		}
	}
	
	public FloorTile GetTileAt (TileDirection direction) {
		switch (direction) {
			default:
			case TileDirection.Middle:
				return this;
			case TileDirection.Up:
				return upTile;
			case TileDirection.Down:
				return downTile;
			case TileDirection.Right:
				return rightTile;
			case TileDirection.Left:
				return leftTile;
		}
	}

	public Vector3 GetTilePosition () {
		return oriPos;
	}
	
	public virtual void Setup () {
		oriPos = transform.position;
	}

	public virtual void OnLandingBy (Transform player) {
		if (isCheckpoint && !onceGenerated) {
			manager.GenerateNextPart ();
			onceGenerated = true;
		}
	}

	public virtual void OnDestroyed () {
		// if hold in list, remove it
		TileFall ();
		Destroy (gameObject, 2);
	}

	public void TileFall () {
		transform.DOMove (new Vector3 (0, -3, 0), 2).SetRelative ().SetDelay (0.5f);
		transform.DOShakePosition (1, 0.2f).SetRelative ().SetDelay (0.8f);
		isGone = true;
	}

	public bool IsGone() {
		return isGone;
	}
}

public enum TileDirection {
	Middle,
	Up,
	Down,
	Right,
	Left
}
