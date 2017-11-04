using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFollowScript : MonoBehaviour 
{
	public Transform obstacle;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(obstacle != null)
		{
			transform.position = Camera.main.WorldToScreenPoint(obstacle.position);
		}
	}
}
