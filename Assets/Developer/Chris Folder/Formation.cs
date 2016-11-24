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
			formationPositions.Add(followers[0], frontPositions[0]);
			formationPositions.Add(followers[1], frontPositions[1]);
			formationPositions.Add(followers[2], frontPositions[2]);
			rears.Add(false);
			rears.Add(false);
			rears.Add(false);
		}
		else
		{
			for(int i = 0; i < 3; i++)
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
