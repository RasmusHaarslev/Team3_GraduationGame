using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BridgeWalkable : MonoBehaviour
{

	GameObject player;

	// Use this for initialization
	void Start()
	{


	}

	// Update is called once per frame
	void Update()
	{

		if (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}

		if ((transform.position.y - player.transform.position.y) > 2)
		{
			if (gameObject.layer != 2)
			{
				gameObject.layer = 2;

				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					gameObject.transform.GetChild(i).gameObject.layer = 2;
				}

			}


		}
		else
		{
			if (gameObject.layer != 13)
			{
				gameObject.layer = 13;
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					gameObject.transform.GetChild(i).gameObject.layer = 13;
				}
			}
		}

	}
}
