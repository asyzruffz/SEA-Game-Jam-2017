using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjenAnimatorScript : MonoBehaviour 
{
	public CharacterMovement characterScript;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DoJump()
	{
		characterScript.JumpEvent();
	}
}
