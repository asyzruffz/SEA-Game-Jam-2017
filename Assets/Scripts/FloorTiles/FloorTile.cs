using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

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
	bool onceGenerated;

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
		return transform.position;
	}
	
	public virtual void Setup () {

	}

	public virtual void OnLandingBy (Transform player) {
		if (isCheckpoint && !onceGenerated) {
			manager.GenerateNextPart ();
			onceGenerated = true;
		}
	}

	public virtual void OnDestroyed () {
		// if hold in list, remove it
		Destroy (gameObject);
	}
}

public enum TileDirection {
	Middle,
	Up,
	Down,
	Right,
	Left
}
