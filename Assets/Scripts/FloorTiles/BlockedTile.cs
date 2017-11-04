using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedTile : FloorTile {

	public override void Setup () {
		// Make the tile inaccessible from neighbour
		if (upTile != null) {
			upTile.downTile = null;
		}
		if (downTile != null) {
			downTile.upTile = null;
		}
		if (rightTile != null) {
			rightTile.leftTile = null;
		}
		if (leftTile != null) {
			leftTile.rightTile = null;
		}
	}

	public override void OnLandingAt () {
		if (!BounceToNeighbour ()) {
			// Die if no neighbour available
		}
	}

	bool BounceToNeighbour () {
		List<FloorTile> availableNeighbours = new List<FloorTile> ();

		if (upTile != null && !upTile.isSpecial) {
			availableNeighbours.Add (upTile);
		}
		if (downTile != null && !downTile.isSpecial) {
			availableNeighbours.Add (downTile);
		}
		if (rightTile != null && !rightTile.isSpecial) {
			availableNeighbours.Add (rightTile);
		}
		if (leftTile != null && !leftTile.isSpecial) {
			availableNeighbours.Add (leftTile);
		}

		FloorTile randomNeigh = availableNeighbours[Random.Range (0, availableNeighbours.Count)];

		// ShiftTo neighbours

		return randomNeigh != null;
	}
}
