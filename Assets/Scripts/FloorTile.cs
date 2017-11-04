using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

	public FloorTile upTile;
	public FloorTile downTile;
	public FloorTile rightTile;
	public FloorTile leftTile;

	void Start () {
		
	}
	
	void Update () {
		
	}

	public bool IsUpValid () {
		return upTile != null;
	}

	public bool IsDownValid () {
		return downTile != null;
	}

	public bool IsRightValid () {
		return rightTile != null;
	}

	public bool IsLeftValid () {
		return leftTile != null;
	}
	
	public FloorTile GetUpTile () {
		return upTile;
	}

	public FloorTile GetDownTile () {
		return downTile;
	}

	public FloorTile GetRightTile () {
		return rightTile;
	}

	public FloorTile GetLeftTile () {
		return leftTile;
	}

	public Vector3 GetTilePosition () {
		return transform.position;
	}
}
