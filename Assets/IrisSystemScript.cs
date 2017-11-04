using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IrisSystemScript : MonoBehaviour 
{
	bool isIrisActive = false;
	public Image dangerPrefab;
	// Use this for initialization
	void Start () 
	{

	}

	//! Obstacle has ref of Image icon
	public void CheckDanger()
	{
		GameObject[] obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
		isIrisActive = true;
		for(int i = 0; i < obstacleList.Length; i ++)
		{
			InstadeathTile tempDeathTile = obstacleList[i].GetComponent<InstadeathTile>();
			BlockedTile tempBlockTile = obstacleList[i].GetComponent<BlockedTile>();
			bool isTied = false;
			if(tempDeathTile != null)
			{
				isTied = tempDeathTile.tiedIcon;
			}
			else if(tempBlockTile != null)
			{
				isTied = tempBlockTile.tiedIcon;
			}


			if(obstacleList[i].GetComponent<Renderer>().isVisible && isTied == false)
			{
				Vector3 spawnPos = Camera.main.WorldToScreenPoint(obstacleList[i].transform.position);
				Image tempImg = Instantiate(dangerPrefab, spawnPos, Quaternion.identity);
				tempImg.GetComponent<ObstacleFollowScript>().obstacle = obstacleList[i].gameObject.transform;

				if(tempDeathTile != null)
				{
					tempDeathTile.tiedIcon = tempImg;	
				}
				else if(tempBlockTile != null)
				{
					tempBlockTile.tiedIcon = tempImg;
				}

				tempImg.transform.parent = this.gameObject.transform;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			CheckDanger();
//			Camera.main.GetComponent<PostProcessingBehavior>();
		}

		if(!isIrisActive)
		{
			//! Check steps taken
		}
	}
}
