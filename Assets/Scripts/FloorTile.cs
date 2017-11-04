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
}
