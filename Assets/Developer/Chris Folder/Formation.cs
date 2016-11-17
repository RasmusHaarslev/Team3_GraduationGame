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

	private void ChangeFormation(ChangeFormationEvent e)
	{
		var index = followers.IndexOf(e.hunter);
		if (e.rear == true)
		{
			Move(index, rearPositions[index]);
			rears[index] = true;
		}
		else
		{
			Move(index, frontPositions[index]);
			rears[index] = false;
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
					Move(i, frontPositions[frontPositions.IndexOf(formationPositions[followers[0]])]);
				else
					Move(i, rearPositions[rearPositions.IndexOf(formationPositions[followers[0]])]);
			}
		}
	}

	public void Move(int followerIndex, Transform position)
	{
		formationPositions[followers[followerIndex]] = position;
	}
}
