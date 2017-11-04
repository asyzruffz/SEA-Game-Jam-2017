using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	public FloorTile currentTile;

	TouchGesture touch;

	void Start () {

		//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		RunMobileInput ();
		//End of mobile platform dependendent compilation section started above with #if
#endif

	}

	void Update () {

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		RunDefaultInput ();
#endif

	}

	void RunMobileInput () {
		touch = new TouchGesture ();

		StartCoroutine (touch.CheckHorizontalSwipes (
			onLeftSwipe: () => { MoveTowards (TileDirection.Left); },
			onRightSwipe: () => { MoveTowards (TileDirection.Right); },
			onDownSwipe: () => { MoveTowards (TileDirection.Down); },
			onUpSwipe: () => { MoveTowards (TileDirection.Up); }
			));
	}

	void RunDefaultInput () {
		int horizontal = Input.GetButtonDown ("Horizontal") ? (int)(Input.GetAxisRaw ("Horizontal")) : 0;   //Used to store the horizontal move direction.
		int vertical = Input.GetButtonDown ("Vertical") ? (int)(Input.GetAxisRaw ("Vertical")) : 0;        //Used to store the vertical move direction.

		//Check if moving horizontally, if so set vertical to zero.
		if (horizontal != 0) {
			vertical = 0;
		}

		//Check if we have a non-zero value for horizontal or vertical
		if (horizontal != 0 || vertical != 0) {
			if (horizontal > 0) {
				MoveTowards (TileDirection.Right);
			} else if (horizontal < 0) {
				MoveTowards (TileDirection.Left);
			} else if (vertical > 0) {
				MoveTowards (TileDirection.Up);
			} else if (vertical < 0) {
				MoveTowards (TileDirection.Down);
			}
		}
	}

	void MoveTowards (TileDirection direction) {
		if (currentTile == null) {
			Debug.Log ("No information on current tile the character is standing on!");
			return;
		}

		if (currentTile.IsTileValidAt (direction)) {
			FloorTile targetTile = currentTile.GetTileAt (direction);
			ShiftTo (targetTile);
		}
	}
	
	public void ShiftTo (FloorTile tile) {
		// TODO animation
		transform.position = tile.GetTilePosition ();
		tile.OnLandingBy (transform);
		currentTile = tile;
	}
}
