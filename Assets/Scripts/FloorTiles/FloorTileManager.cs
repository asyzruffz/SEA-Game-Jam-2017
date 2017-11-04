using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileManager : MonoBehaviour {

	public FloorTile startTile;

	[Header("Generator Settings")]
	public GameObject prefabNormalTile;
	public GameObject prefabSpecialTile;
	public bool generateOnStart;
	public float offsetDistance = 1;
	public List<TilesPattern> patterns = new List<TilesPattern> ();

	FloorTile lastTileGenerated;

	void Start () {
		if (startTile != null) {
			lastTileGenerated = startTile;
		} else if (generateOnStart) {
			GenerateTileWithOffset (null, Vector3.zero);
		}
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.U)) {
			GenerateRelativeToTileAt (lastTileGenerated, TileDirection.Up);
		} else if (Input.GetKeyDown (KeyCode.J)) {
			GenerateRelativeToTileAt (lastTileGenerated, TileDirection.Down);
		} else if (Input.GetKeyDown (KeyCode.H)) {
			GenerateRelativeToTileAt (lastTileGenerated, TileDirection.Left);
		} else if (Input.GetKeyDown (KeyCode.K)) {
			GenerateRelativeToTileAt (lastTileGenerated, TileDirection.Right);
		} else if (Input.GetKeyDown(KeyCode.Space)) {
			GenerateNineTileForward (lastTileGenerated);
		}
	}

	void GenerateNineTileForward (FloorTile refTile) {
		FloorTile middleAnchor = refTile;
		FloorTile leftAnchor = null;
		FloorTile rightAnchor = null;

		TilesPattern randomPattern = patterns[Random.Range(0, patterns.Count)];

		for (int i = 0; i < randomPattern.pattern.Count; i++) {
			GenerateRelativeToTileAt (middleAnchor, TileDirection.Up, randomPattern.pattern[i].col2);
			middleAnchor = lastTileGenerated;

			GenerateRelativeToTileAt (middleAnchor, TileDirection.Left, randomPattern.pattern[i].col1);
			if (leftAnchor != null) {
				leftAnchor.upTile = lastTileGenerated;
			}
			lastTileGenerated.downTile = leftAnchor;
			leftAnchor = lastTileGenerated;

			GenerateRelativeToTileAt (middleAnchor, TileDirection.Right, randomPattern.pattern[i].col3);
			if (rightAnchor != null) {
				rightAnchor.upTile = lastTileGenerated;
			}
			lastTileGenerated.downTile = rightAnchor;
			rightAnchor = lastTileGenerated;
		}

		lastTileGenerated = middleAnchor;
	}

	void GenerateRelativeToTileAt (FloorTile refTile, TileDirection direction, bool isSpecialTile = false) {
		if (refTile == null) {
			Debug.Log ("No information on reference tile to generate at direction!");
			return;
		}
		
		switch (direction) {
			case TileDirection.Up:
				GenerateTileWithOffset (refTile, new Vector3 (0, 0, offsetDistance), isSpecialTile);
				refTile.upTile = lastTileGenerated;
				lastTileGenerated.downTile = refTile;
				break;
			case TileDirection.Down:
				GenerateTileWithOffset (refTile, new Vector3 (0, 0, -offsetDistance), isSpecialTile);
				refTile.downTile = lastTileGenerated;
				lastTileGenerated.upTile = refTile;
				break;
			case TileDirection.Right:
				GenerateTileWithOffset (refTile, new Vector3 (offsetDistance, 0, 0), isSpecialTile);
				refTile.rightTile = lastTileGenerated;
				lastTileGenerated.leftTile = refTile;
				break;
			case TileDirection.Left:
				GenerateTileWithOffset (refTile, new Vector3 (-offsetDistance, 0, 0), isSpecialTile);
				refTile.leftTile = lastTileGenerated;
				lastTileGenerated.rightTile = refTile;
				break;
			case TileDirection.Middle:
				GenerateTileWithOffset (refTile, Vector3.zero, isSpecialTile);
				break;
			default:
				break;
		}
	}

	void GenerateTileWithOffset (FloorTile refTile, Vector3 offset, bool isSpecialTile = false) {
		Vector3 refPos = (refTile == null) ? Vector3.zero : refTile.GetTilePosition ();

		GameObject newTile = Instantiate (isSpecialTile ? prefabSpecialTile : prefabNormalTile, transform);
		newTile.transform.localPosition = refPos + offset;

		lastTileGenerated = newTile.GetComponent<FloorTile> ();
	}
}
