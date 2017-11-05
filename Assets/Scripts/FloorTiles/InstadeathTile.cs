using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InstadeathTile : FloorTile {

	public Image tiedIcon;
	
	bool isFallen;
	
	public override void OnLandingBy (Transform player) {
		// Call player die
		FallDownWith (player);
	}

	public override void OnDestroyed () {
		// Destroy animation

		base.OnDestroyed ();
	}

	void FallDownWith (Transform player) {
		player.GetComponent<CharacterMovement> ().Fall ();
		if (!isFallen) {
			transform.DOMove (new Vector3 (0, -3, 0), 2).SetRelative ().SetDelay (0.5f);
			transform.DOShakePosition (1, 0.2f).SetRelative ().SetDelay (0.8f);
			isFallen = true;
		}
	}
}
