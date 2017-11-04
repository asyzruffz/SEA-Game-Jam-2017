using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockedTile : FloorTile {

	public override void Setup () {
		base.Setup ();

		// Make the tile inaccessible from neighbour
		/*if (upTile != null) {
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
		}*/
	}

	public override void OnLandingBy (Transform player) {
		if (!BounceToNeighbour (player)) {
			// Die if no neighbour available
		}
	}

	void BouneBack (Transform player) {
		player.DOShakePosition (0.5f, 0.2f, 5);
		//player.GetComponent<CharacterMovement> ().ShiftTo (randomNeigh);
	}

	bool BounceToNeighbour (Transform player) {
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
		if (randomNeigh != null) {
			player.GetComponent<CharacterMovement> ().ShiftTo (randomNeigh);
		}
		player.DOShakePosition (0.5f, 0.2f, 5);

		return randomNeigh != null;
	}
}
