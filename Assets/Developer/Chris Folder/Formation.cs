using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Formation : MonoBehaviour
{

	public Dictionary<GameObject, Vector3> formationPositions = new Dictionary<GameObject, Vector3>();

	public List<Transform> frontPositions;
	public List<Transform> rearPositions;

	public List<GameObject> followers;

	// Use this for initialization
	void Start()
	{

	}

	void OnEnable()
	{
		

	}

	// Update is called once per frame
	void Update()
	{
		if (followers.Count == 0)
		{
			followers.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
			formationPositions.Add(followers[0], frontPositions[0].position);
			formationPositions.Add(followers[1], frontPositions[1].position);
			formationPositions.Add(followers[2], frontPositions[2].position);
		} else
		{
			formationPositions[followers[0]] = frontPositions[0].position;
			formationPositions[followers[1]] = frontPositions[1].position;
			formationPositions[followers[2]] = frontPositions[2].position;
		}
		
	}
}
