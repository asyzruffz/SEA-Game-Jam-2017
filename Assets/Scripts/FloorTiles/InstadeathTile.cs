using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InstadeathTile : FloorTile {

	bool isFallen;

	void Start () {
		DOTween.Init (false, true, LogBehaviour.ErrorsOnly);
	}

	public override void OnLandingBy (Transform player) {
		// Call player die
		FallDownWith (player);
	}

	public override void OnDestroyed () {
		// Destroy animation

		base.OnDestroyed ();
	}

	void FallDownWith (Transform player) {
		player.DOMove (new Vector3 (0, -3, 0), 2).SetRelative ();
		if (!isFallen) {
			transform.DOMove (new Vector3 (0, -3, 0), 2).SetRelative ();
			isFallen = true;
		}
	}
}
