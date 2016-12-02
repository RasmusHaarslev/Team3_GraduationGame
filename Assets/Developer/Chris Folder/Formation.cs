using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Formation : MonoBehaviour
{ 
	public Dictionary<GameObject, Transform> formationPositions = new Dictionary<GameObject, Transform>();
	public List<Transform> frontPositions;
	public List<Transform> rearPositions;
	public List<GameObject> followers;
	public List<bool> rears;
	int numberOfFollowers = 0;


	void OnEnable()
	{
		EventManager.Instance.StartListening<ChangeFormationEvent>(ChangeFormation);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<ChangeFormationEvent>(ChangeFormation);
	}

	void OnApplicationQuit()
	{
		this.enabled = false;
	}

	private void ChangeFormation(ChangeFormationEvent e)
	{
		var index = followers.IndexOf(e.hunter);
		if (rears[index] == true)
		{
			Move(index, rearPositions[index]);
			rears[index] = false;
		}
		else
		{
			Move(index, frontPositions[index]);
			rears[index] = true;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (followers.Count == 0)
		{
			followers.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
			numberOfFollowers = followers.Count;
			for (int i = 0; i < numberOfFollowers; i++)
			{
				formationPositions.Add(followers[i], frontPositions[i]);
			}
			rears.Add(false);
			rears.Add(false);
			rears.Add(false);
		}
		else
		{
			for(int i = 0; i < numberOfFollowers; i++)
			{
				if(rears[i] == false)
				{
					Move(i, frontPositions[i]);
				}
				else
				{
					Move(i, rearPositions[i]);
				}
			}
		}
	}

	public void Move(int followerIndex, Transform position)
	{
		formationPositions[followers[followerIndex]] = position;
	}
}
