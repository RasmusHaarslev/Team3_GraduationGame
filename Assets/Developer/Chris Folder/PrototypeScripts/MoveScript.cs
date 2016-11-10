using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour
{

	NavMeshAgent agent;
	public bool movement = true;

	// Use this for initialization
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	void OnEnable()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (movement)
		{
			if (Input.GetKey(KeyCode.Mouse0))
			{
				MoveToClickPosition();
			}
		}
		

	}


	public void MoveToClickPosition()
	{

		RaycastHit hit;

		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
		{
			agent.destination = hit.point;
		}
	}

}
