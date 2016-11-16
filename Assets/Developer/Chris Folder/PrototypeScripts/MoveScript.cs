using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour
{

	NavMeshAgent agent;
	public bool movement = true;
	public bool attacking = false;
	Character character;
	float distanceToTarget;
	float attackSpeed;
	float counter = 0;
	bool attack = false;


	// Use this for initialization
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		character = GetComponent<Character>();
		attackSpeed = character.damageSpeed;
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
				agent.Resume();
				attacking = false;
				MoveToClickPosition();
			}
			if (attacking)
			{
				if (character.target != null && !character.target.GetComponent<Character>().isDead)
				{
					agent.SetDestination(character.target.transform.position);
					distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(character.target.transform.position.x, 0, character.target.transform.position.z));
					if (distanceToTarget < agent.stoppingDistance)
					{
						character.RotateTowards(character.target.transform);
						if (counter <= 0)
						{
							character.DealDamage();
							counter = attackSpeed;
						}
						else
						{
							counter -= Time.deltaTime;
						}
					}
				}
				else
				{
					agent.Resume();
					attacking = false;
				}
			}

		}
	}


	public void MoveToClickPosition()
	{

		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
		{
			if (hit.transform.gameObject.tag == "Unfriendly")
			{
                if(character.target)
                    character.target.GetComponent<MaterialSwitcher>().SwitchMaterial();

                character.target = hit.transform.gameObject;
				attacking = true;
				agent.stoppingDistance = character.range;
				agent.SetDestination(hit.transform.position);

                hit.transform.gameObject.GetComponent<MaterialSwitcher>().SwitchMaterial();
            }
			else if (hit.transform.gameObject.tag == "Player")
			{
				attacking = false;
				agent.stoppingDistance = 0;
			}
			else
			{
                EventManager.Instance.TriggerEvent(new PositionClicked(hit.point));
                agent.stoppingDistance = 0;
				agent.destination = hit.point;
				attacking = false;
			}
		}
	}
}
