using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	public FloorTile currentTile;

	TouchGesture touch;

	void Start () {

		//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		touch = new TouchGesture ();

		StartCoroutine (touch.CheckHorizontalSwipes (
			onLeftSwipe: () => { MoveLeft (); },
			onRightSwipe: () => { MoveRight (); },
			onDownSwipe: () => { MoveDown (); },
			onUpSwipe: () => { MoveUp (); }
			));

		//End of mobile platform dependendent compilation section started above with #if
#endif

	}

	void Update () {

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

		int horizontal = Input.GetButtonDown ("Horizontal") ? (int)(Input.GetAxisRaw ("Horizontal")) : 0;	//Used to store the horizontal move direction.
		int vertical = Input.GetButtonDown ("Vertical") ? (int)(Input.GetAxisRaw ("Vertical")) : 0;        //Used to store the vertical move direction.

		//Check if moving horizontally, if so set vertical to zero.
		if (horizontal != 0) {
			vertical = 0;
		}

		//Check if we have a non-zero value for horizontal or vertical
		if (horizontal != 0 || vertical != 0) {
			if (horizontal > 0) {
				MoveRight ();
			} else if (horizontal < 0) {
				MoveLeft ();
			} else if (vertical > 0) {
				MoveUp ();
			} else if (vertical < 0) {
				MoveDown ();
			}
		}

#endif

	}

	void MoveLeft () {
		if (currentTile == null) {
			Debug.Log ("No information on current tile the character is standing on!");
			return;
		}

		if (currentTile.IsLeftValid()) {
			FloorTile targetTile = currentTile.GetLeftTile ();
			ShiftTo (targetTile);
		}
	}

	void MoveRight () {
		if (currentTile == null) {
			Debug.Log ("No information on current tile the character is standing on!");
			return;
		}

		if (currentTile.IsRightValid ()) {
			FloorTile targetTile = currentTile.GetRightTile ();
			ShiftTo (targetTile);
		}
	}

	void MoveDown () {
		if (currentTile == null) {
			Debug.Log ("No information on current tile the character is standing on!");
			return;
		}

		if (currentTile.IsDownValid ()) {
			FloorTile targetTile = currentTile.GetDownTile ();
			ShiftTo (targetTile);
		}
	}

	void MoveUp () {
		if (currentTile == null) {
			Debug.Log ("No information on current tile the character is standing on!");
			return;
		}

		if (currentTile.IsUpValid ()) {
			FloorTile targetTile = currentTile.GetUpTile ();
			ShiftTo (targetTile);
		}
	}

	void ShiftTo (FloorTile tile) {
		// TODO animation
		transform.position = tile.GetTilePosition ();
		currentTile = tile;
	}
}
