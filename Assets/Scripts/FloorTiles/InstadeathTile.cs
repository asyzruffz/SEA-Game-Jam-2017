using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InstadeathTile : FloorTile {

	public Image tiedIcon;
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
		transform.DOMove (new Vector3 (0, -3, 0), 2).SetRelative ();
		player.DOMove (new Vector3 (0, -3, 0), 2).SetRelative ();
	}
}
