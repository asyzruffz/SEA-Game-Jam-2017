using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGesture {
	
	private GestureSettings settings;
	private float swipeStartTime;
	private bool couldBeSwipe;
	private Vector2 startPos;
	private int stationaryForFrames;
	private TouchPhase lastPhase;

	public TouchGesture (GestureSettings gestureSettings = null) {
		if (gestureSettings != null) {
			settings = gestureSettings;
		} else {
			settings = new GestureSettings ();
		}
	}

	//Coroutine, which gets Started in "Start()" and runs over the whole game to check for swipes
	public IEnumerator CheckHorizontalSwipes (Action onLeftSwipe, Action onRightSwipe, Action onDownSwipe, Action onUpSwipe) {
		while (true) { //Loop. Otherwise we wouldnt check continuously ;-)
			foreach (Touch touch in Input.touches) { //For every touch in the Input.touches - array...
				switch (touch.phase) {
					case TouchPhase.Began: //The finger first touched the screen --> It could be(come) a swipe
						couldBeSwipe = true;
						startPos = touch.position;  //Position where the touch started
						swipeStartTime = Time.time; //The time it started
						stationaryForFrames = 0;
						break;
					case TouchPhase.Stationary: //Is the touch stationary? --> No swipe then!
						if (isContinouslyStationary (frames: 6))
							couldBeSwipe = false;
						break;
					case TouchPhase.Ended:
						if (isASwipe (touch)) {
							couldBeSwipe = false; //<-- Otherwise this part would be called over and over again.

							//Set touchEnd to equal the position of this touch
							Vector2 touchEnd = touch.position;

							//Calculate the difference between the beginning and end of the touch on the x axis.
							float x = touchEnd.x - startPos.x;

							//Calculate the difference between the beginning and end of the touch on the y axis.
							float y = touchEnd.y - startPos.y;

							//Check if the difference along the x axis is greater than the difference along the y axis.
							if (Mathf.Abs (x) > Mathf.Abs (y)) {
								if (x > 0) {
									onRightSwipe (); //Right-swipe
								} else {
									onLeftSwipe (); //Left-swipe
								}
							} else {
								if (y > 0) {
									onUpSwipe (); //Up-swipe
								} else {
									onDownSwipe (); //Down-swipe
								}
							}
						}
						break;
				}
				lastPhase = touch.phase;
			}
			yield return null;
		}
	}

	private bool isContinouslyStationary (int frames) {
		if (lastPhase == TouchPhase.Stationary)
			stationaryForFrames++;
		else // reset back to 1
			stationaryForFrames = 1;
		return stationaryForFrames > frames;
	}

	private bool isASwipe (Touch touch) {
		float swipeTime = Time.time - swipeStartTime; //Time the touch stayed at the screen till now.
		float swipeDist = Mathf.Abs (touch.position.x - startPos.x); //Swipe distance
		return couldBeSwipe && swipeTime < settings.maxSwipeTime && swipeDist > settings.minSwipeDist;
	}
}

[System.Serializable]
public class GestureSettings {
	public float minSwipeDist = 100;
	public float maxSwipeTime = 10;
}
