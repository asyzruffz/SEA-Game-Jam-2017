using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMovement : MonoBehaviour {

	public float rotateSpeed = 1.0f;
	public float defaultSpeed = 0.5f;
	public Vector3 Target;
	public FloorTile currentTile;

	Quaternion targetDir = Quaternion.Euler(Vector3.zero);
	private float _distanceToTarget;
	bool canRot = false;
	bool canMove = false;
	bool isDead = false;

	TouchGesture touch;
	Animator playerAnim;
	FloorTile prevTile;
	float yOffset;
	//Vector3 prevPos;

	void Start () {
		
		playerAnim = GetComponentInChildren<Animator>();
		yOffset = transform.position.y;
		DOTween.Init (false, true, LogBehaviour.ErrorsOnly);

		//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		RunMobileInput ();
		//End of mobile platform dependendent compilation section started above with #if
#endif

	}

	void Update () {


		if(canMove)
		{
			// Calculate our distance from target
			Vector3 deltaPosition = Target - transform.position;
			_distanceToTarget = deltaPosition.magnitude;

			// Set our position
			transform.position += deltaPosition * defaultSpeed * Time.deltaTime;
			if(_distanceToTarget <= 0.1f)
			{
				transform.position = Target;
				canMove = false;
			}
		}
		else
		{
			#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
			RunDefaultInput ();
			#endif
		}

		if(canRot)
		{
			transform.rotation = Quaternion.Euler(new Vector3(0.0f, transform.rotation.y + (rotateSpeed * Time.deltaTime), 0.0f));
//			Debug.Log(transform.rotation);
			if(transform.rotation == targetDir)
			{
				canRot = false;
			}
		}

		if (CheckLife() && !isDead) {
			isDead = true;
			Fall ();
		}
	}

	void RunMobileInput () {
		if(canMove) {
			return;
		}

		touch = new TouchGesture ();

		StartCoroutine (touch.CheckHorizontalSwipes (
			onLeftSwipe: () => { MoveTowards (TileDirection.Left); },
			onRightSwipe: () => { MoveTowards (TileDirection.Right); },
			onDownSwipe: () => { MoveTowards (TileDirection.Down); },
			onUpSwipe: () => { MoveTowards (TileDirection.Up); }
			));
	}

	void RunDefaultInput () {
		if(canMove) {
			return;
		}

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
			Debug.Log ("No information on current tile that the character is standing on!");
			return;
		}

		if (currentTile.IsTileValidAt (direction)) {
			FloorTile targetTile = currentTile.GetTileAt (direction);
			ShiftTo (targetTile);

			if(direction == TileDirection.Up)
			{
				targetDir = Quaternion.Euler(Vector3.zero);

				if (GameController.Instance != null) {
					if (!GameController.Instance.isPlaying) {
						GameController.Instance.StartGame ();
					}
				}
			}
			else if(direction == TileDirection.Down)
			{
				targetDir = Quaternion.Euler(Vector3.up * 180.0f);
			}
			else if(direction == TileDirection.Right)
			{
				targetDir = Quaternion.Euler(Vector3.up * 90.0f);
			}
			else if(direction == TileDirection.Left)
			{
				targetDir = Quaternion.Euler(Vector3.down * 90.0f);
			}

			transform.rotation = targetDir;
			transform.LookAt(Target);
			playerAnim.SetTrigger("isJumpAnim");
		}
	}
	
	public void ShiftTo (FloorTile tile) {
		//prevPos = transform.position;
		prevTile = currentTile;
		Target = new Vector3(tile.GetTilePosition ().x, transform.position.y, tile.GetTilePosition ().z);
		currentTile = tile;
		SoundManager.instance.PlaySFX ("SFX Player Jump");
		tile.OnLandingBy (transform);
	}

	public void ReverseShift () {
		if (prevTile != null) {
			Target = new Vector3 (prevTile.GetTilePosition ().x, yOffset, prevTile.GetTilePosition ().z);
			currentTile = prevTile;
		}
	}

	public void JumpEvent()
	{
		canMove = true;
	}

	public void SetMovementEnabled (bool enabled) {
		canMove = enabled;
	}

	bool CheckLife () {
		if (GameController.Instance) {
			if (GameController.Instance.isPlaying) {
				if (currentTile == null || currentTile.IsGone ()) {
					return true;
				}
			}
		}

		return false;
	}

	public void Die () {
		SetMovementEnabled (false);
		currentTile = null;

		if (GameController.Instance) {
			GameController.Instance.isGameOver = true;
			GameController.Instance.isPlaying = false;
		}

		GetComponent<InstantSpawner> ().Spawn ();
		Destroy (gameObject);
	}
	
	public void Fall () {
		SetMovementEnabled (false);
		transform.DOMove (new Vector3 (0, -3, 0), 2).SetRelative ().SetDelay (0.5f);
		SoundManager.instance.PlaySFX ("SFX Player Dies");
		Debug.Log ("SFX Player Dies");
		Invoke ("Die", 1.5f);
	}
}
