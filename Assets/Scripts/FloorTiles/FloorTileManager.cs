using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileManager : MonoBehaviour {

	public FloorTile startTile;
	public float baseDecay = 5;
	public float rowDecay = 2;

	[Header("Generator Settings")]
	public bool autoGenerate;
	public float generateDelay = 1;
	public bool initFirstTile;
	public float offsetDistance = 1;
	public int corridorLength = 10;

	[Header ("Spawnables")]
	public GameObject prefabNormalTile;
	public List<GameObject> prefabSpecialTiles = new List<GameObject> ();
	public List<TilesPattern> hallPatterns = new List<TilesPattern> ();

	FloorTile lastTileGenerated = null;
	FloorTile lastMiddleGenerated = null;
	FloorTile lastLeftGenerated = null;
	FloorTile lastRightGenerated = null;
	float timer = 0;
	bool toggleFloor;

	void Start () {
		if (startTile != null) {
			lastTileGenerated = startTile;
		} else if (initFirstTile) {
			GenerateTileWithOffset (null, Vector3.zero);
		}

		GenerateNextPart ();
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
				GenerateHallForward (lastMiddleGenerated == null ? lastTileGenerated : lastMiddleGenerated);
			} else if (Input.GetKeyDown (KeyCode.B)) {
				FloorTile exit = GetEmptyHallEndTile ();
				if (exit != null) {
					GenerateCorridorForward (exit);
				} else {
					Debug.Log ("No empty tiles to exit hall into corridor!");
				}
			}
		}
	}

	public void GenerateNextPart () {
		if (toggleFloor) {
			GenerateCorridorForward (lastTileGenerated);
		} else {
			int occurances = Random.Range (1, 3);
			for (int i = 0; i < occurances; i++) {
				GenerateHallForward (lastMiddleGenerated == null ? lastTileGenerated : lastMiddleGenerated, i == (occurances - 1));
				baseDecay += (3 * rowDecay);
			}
			baseDecay -= (occurances * 3 * rowDecay);
		}

		toggleFloor = !toggleFloor;
	}

	void GenerateHallForward (FloorTile refTile, bool hasExitTrigger = true) {
		FloorTile anchor = refTile;

		TilesPattern randomPattern = hallPatterns[Random.Range(0, hallPatterns.Count)];

		for (int i = 0; i < randomPattern.pattern.Count; i++) {
			GenerateRelativeToTileAt (anchor, TileDirection.Up, randomPattern.pattern[i].col2);
			lastTileGenerated.lifetime = baseDecay + (i * rowDecay);
			lastTileGenerated.Setup ();
			anchor = lastTileGenerated;

			GenerateRelativeToTileAt (anchor, TileDirection.Left, randomPattern.pattern[i].col1);
			lastTileGenerated.lifetime = baseDecay + (i * rowDecay);
			if (lastLeftGenerated != null) {
				lastLeftGenerated.upTile = lastTileGenerated;
			}
			lastTileGenerated.downTile = lastLeftGenerated;
			lastTileGenerated.Setup ();
			lastLeftGenerated = lastTileGenerated;

			GenerateRelativeToTileAt (anchor, TileDirection.Right, randomPattern.pattern[i].col3);
			lastTileGenerated.lifetime = baseDecay + (i * rowDecay);
			if (lastRightGenerated != null) {
				lastRightGenerated.upTile = lastTileGenerated;
			}
			lastTileGenerated.downTile = lastRightGenerated;
			lastTileGenerated.Setup ();
			lastRightGenerated = lastTileGenerated;
		}

		lastMiddleGenerated = anchor;

		FloorTile exit = GetEmptyHallEndTile ();
		if (exit != null) {
			exit.isCheckpoint = hasExitTrigger;
			lastTileGenerated = exit;
		} else {
			Debug.Log ("No empty tiles to exit hall into corridor!");
		}
	}

	void GenerateCorridorForward (FloorTile refTile) {
		GenerateRelativeToTileAt (refTile, TileDirection.Up);
		lastTileGenerated.lifetime = baseDecay + rowDecay;
		int lastTurnCount = 1;

		TileDirection oldDir = TileDirection.Up;
		for (int i = 1; i < corridorLength - 1; i++) {
			TileDirection dir = oldDir;
			if (lastTurnCount >= 2) {
				if (Random.value < 0.5f) {
					dir = TileDirection.Up;
				} else {
					if (oldDir == TileDirection.Left || oldDir == TileDirection.Right) {
						dir = oldDir;
					} else {
						dir = (Random.value < 0.5f) ? TileDirection.Left : TileDirection.Right;
					}
				}
			}
			
			GenerateRelativeToTileAt (lastTileGenerated, dir);
			lastTileGenerated.lifetime = baseDecay + ((i + 1) * rowDecay);
			lastTurnCount = (dir == oldDir) ? lastTurnCount + 1 : 0;
			oldDir = dir;
		}

		GenerateRelativeToTileAt (lastTileGenerated, TileDirection.Up);
		lastTileGenerated.lifetime = baseDecay + (corridorLength * rowDecay);
		lastTileGenerated.isCheckpoint = true;
		lastMiddleGenerated = lastTileGenerated;
		lastLeftGenerated = lastRightGenerated = null;
	}

	FloorTile GetEmptyHallEndTile () {
		List<FloorTile> endHallTiles = new List<FloorTile> ();

		if (lastMiddleGenerated != null && !lastMiddleGenerated.isSpecial) {
			endHallTiles.Add (lastMiddleGenerated);
		}
		if (lastLeftGenerated != null && !lastLeftGenerated.isSpecial) {
			endHallTiles.Add (lastLeftGenerated);
		}
		if (lastRightGenerated != null && !lastRightGenerated.isSpecial) {
			endHallTiles.Add (lastRightGenerated);
		}

		if (endHallTiles.Count > 0) {
			return endHallTiles[Random.Range (0, endHallTiles.Count)];
		} else {
			return null;
		}
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

		GameObject newTile = Instantiate (isSpecialTile ? prefabSpecialTiles[Random.Range(0, prefabSpecialTiles.Count)] : prefabNormalTile, transform);
		newTile.transform.localPosition = refPos + offset;

		FloorTile tile = newTile.GetComponent<FloorTile> ();
		tile.manager = this;
		tile.isSpecial = isSpecialTile;
		tile.Setup ();
		lastTileGenerated = tile;
	}
}
