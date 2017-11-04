using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstadeathTile : FloorTile {
	
	public override void OnLandingAt () {
		// Call die
	}

	public override void OnDestroyed () {
		// Destroy animation

		base.OnDestroyed ();
	}
}
