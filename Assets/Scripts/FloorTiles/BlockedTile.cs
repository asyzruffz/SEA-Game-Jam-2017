using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlockedTile : FloorTile 
{
	public Image tiedIcon;
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

	public override void OnLandingBy (Transform player) {
		if (!BounceToNeighbour (player)) {
			// Die if no neighbour available
		}
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

		player.DOShakePosition (0.5f, 0.2f);
		FloorTile randomNeigh = availableNeighbours[Random.Range (0, availableNeighbours.Count)];
		if (randomNeigh != null) {
			player.GetComponent<CharacterMovement> ().ShiftTo (randomNeigh);
		}

		return randomNeigh != null;
	}
}
