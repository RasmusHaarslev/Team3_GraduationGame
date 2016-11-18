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
	bool isDead = false;

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
		if (character.currentHealth <= 0 && isDead == false)
		{
			EventManager.Instance.TriggerEvent(new PlayerDeathEvent());
			isDead = true;
		}
		if (movement)
		{
			if (Input.GetKey(KeyCode.Mouse0))
			{
				agent.Resume();
				character.isInCombat = false;
				attacking = false;
				MoveToClickPosition();
			}
			if (attacking)
			{
				if (character.target != null && !character.target.GetComponent<Character>().isDead)
				{
					character.isInCombat = true;
					agent.SetDestination(character.target.transform.position);
					distanceToTarget = agent.remainingDistance;
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
					character.isInCombat = false;
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
				if (character.target)
					character.target.GetComponent<MaterialSwitcher>().SwitchMaterial();

				character.target = hit.transform.gameObject;
				attacking = true;
				agent.stoppingDistance = character.range;
				agent.SetDestination(hit.transform.position);
				if (!character.isInCombat)
				{
					EventManager.Instance.TriggerEvent(new EnemyAttackedByLeaderEvent(hit.transform.gameObject));
				}
				
				hit.transform.gameObject.GetComponent<MaterialSwitcher>().SwitchMaterial();
			}
			else if (hit.transform.gameObject.tag == "Player")
			{
				attacking = false;
				agent.stoppingDistance = 1.2f;
			}
			else
			{
				EventManager.Instance.TriggerEvent(new PositionClicked(hit.point));
				agent.stoppingDistance = 1.2f;
				agent.SetDestination(new Vector3(hit.point.x, hit.point.y, hit.point.z));
				attacking = false;
			}
		}
	}
}
