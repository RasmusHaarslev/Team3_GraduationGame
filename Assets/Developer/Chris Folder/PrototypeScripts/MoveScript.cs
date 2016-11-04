using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	NavMeshAgent agent;
	public int health = 1000000;

	// Use this for initialization
	void Start () {
		agent = GetComponent < NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Mouse0))
			{
			MoveToClickPosition ();
		}
		if (health <= 0)
		{
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "EnemyWeapon")
		{
			health--;
		}
	}

	public void MoveToClickPosition() {
		
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
				agent.destination = hit.point;
			}
		}

}
