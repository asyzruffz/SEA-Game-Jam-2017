using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		SoundManager.instance.PlaySFX ("SFX Floors Disappear");
		player.GetComponent<CharacterMovement> ().Fall ();
		if (!isFallen) {
			TileFall ();
			isFallen = true;
		}
	}
}
