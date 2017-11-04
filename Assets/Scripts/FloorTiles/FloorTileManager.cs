using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileManager : MonoBehaviour {

	public FloorTile startTile;

	[Header("Generator Settings")]
	public bool autoGenerate;
	public float generateDelay = 1;
	public bool initFirstTile;
	public float offsetDistance = 1;
	public GameObject prefabNormalTile;
	public GameObject prefabSpecialTile;
	public List<TilesPattern> patterns = new List<TilesPattern> ();

	FloorTile lastTileGenerated = null;
	FloorTile lastLeftGenerated = null;
	FloorTile lastRightGenerated = null;
	float timer = 0;

	void Start () {
		if (startTile != null) {
			lastTileGenerated = startTile;
		} else if (initFirstTile) {
			GenerateTileWithOffset (null, Vector3.zero);
		}
	}
	
	void Update () {
		if (autoGenerate) {
			if (timer >= generateDelay) {
				GenerateHallForward (lastTileGenerated);
				timer -= generateDelay;
			}

			timer += Time.deltaTime;
		} else {
			if (Input.GetKeyDown (KeyCode.U)) {
				GenerateRelativeToTileAt (lastTileGenerated, TileDirection.Up);
			} else if (Input.GetKeyDown (KeyCode.J)) {
				GenerateRelativeToTileAt (lastTileGenerated, TileDirection.Down);
			} else if (Input.GetKeyDown (KeyCode.H)) {
				GenerateRelativeToTileAt (lastTileGenerated, TileDirection.Left);
			} else if (Input.GetKeyDown (KeyCode.K)) {
				GenerateRelativeToTileAt (lastTileGenerated, TileDirection.Right);
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				GenerateHallForward (lastTileGenerated);
			}
		}
	}

	void GenerateHallForward (FloorTile refTile) {
		FloorTile anchor = refTile;

		TilesPattern randomPattern = patterns[Random.Range(0, patterns.Count)];

		for (int i = 0; i < randomPattern.pattern.Count; i++) {
			GenerateRelativeToTileAt (anchor, TileDirection.Up, randomPattern.pattern[i].col2);
			anchor = lastTileGenerated;

			GenerateRelativeToTileAt (anchor, TileDirection.Left, randomPattern.pattern[i].col1);
			if (lastLeftGenerated != null) {
				lastLeftGenerated.upTile = lastTileGenerated;
			}
			lastTileGenerated.downTile = lastLeftGenerated;
			lastLeftGenerated = lastTileGenerated;

			GenerateRelativeToTileAt (anchor, TileDirection.Right, randomPattern.pattern[i].col3);
			if (lastRightGenerated != null) {
				lastRightGenerated.upTile = lastTileGenerated;
			}
			lastTileGenerated.downTile = lastRightGenerated;
			lastRightGenerated = lastTileGenerated;
		}
		
		lastTileGenerated = anchor;
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
