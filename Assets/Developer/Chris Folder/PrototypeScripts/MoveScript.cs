using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class MoveScript : MonoBehaviour
{

	NavMeshAgent agent;
	public bool movement = true;
	public bool attacking = false;
	Character character;
	float distanceToTarget;
	public float attackSpeed;
	public float counter = 0f;
	bool attack = false;
	bool isDead = false;
	bool isFleeing = false;
	List<GameObject> hunters = new List<GameObject>();
	bool hasShot = false;
	public float fleeSpeed = 4f;

	// Use this for initialization
	void Start()
	{

	}

	void OnEnable()
	{
		EventManager.Instance.StartListening<FleeStateEvent>(Flee);
		EventManager.Instance.StartListening<StopFleeEvent>(StopFlee);
		agent = GetComponent<NavMeshAgent>();
		character = GetComponent<Character>();
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<FleeStateEvent>(Flee);
		EventManager.Instance.StopListening<StopFleeEvent>(StopFlee);
	}

	private void StopFlee(StopFleeEvent e)
	{
		isFleeing = false;
		agent.speed = 2.75f;
		agent.Stop();
	}

	private void Flee(FleeStateEvent e)
	{
		isFleeing = true;
		agent.speed = fleeSpeed;
	}


	void OnApplicationQuit()
	{
		this.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (hunters.Count == 0)
		{
			//hunters.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
		}
		int counter = 0;
		foreach (var hunter in hunters)
		{
			if (hunter.GetComponent<Character>().isInCombat)
			{
				character.isInCombat = true;
				counter++;
			}
		}
		if (counter == 0)
			character.isInCombat = false;
		if (!isFleeing)
		{
			attackSpeed = character.damageSpeed;
			if (character.currentHealth <= 0 && isDead == false)
			{
				EventManager.Instance.TriggerEvent(new PlayerDeathEvent());
				isDead = true;
			}
			if (movement)
			{
				if (!character.isDead)
				{
					if (Input.GetKeyDown(KeyCode.Mouse0))
					{
						Manager_Audio.PlaySound(Manager_Audio.walkTapUISound, this.gameObject);
					}
					if (Input.GetKey(KeyCode.Mouse0))
					{
						agent.Resume();
						MoveToClickPosition();
					}
				}
				if (attacking)
				{
					if (character.target != null)
					{
						if (character.target != null && !character.target.GetComponent<Character>().isDead)
						{
							Attacking();
						}
						else
						{
							agent.Resume();
							attacking = false;
						}
					}
				}
			}
		}
		else
		{
			character.isFleeing = true;
			agent.SetDestination(GameObject.FindGameObjectWithTag("FleePoint").transform.position);
		}
	}

	public void MoveToClickPosition()
	{
		agent.updateRotation = true;
		agent.Resume();
		character.animator.SetBool("isAware", false);

		bool enemyHit = false;
		GameObject firstEnemyHit = null;
		Vector3 firstGroundHitPoint = new Vector3();
		Transform firstGroundHitTransform = null;

		RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 1000);
		Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction.normalized * 500f);
		if (hits.Length > 0)
		{
			foreach (var hit in hits)
			{
				if (hit.transform.gameObject.tag == "Unfriendly")
				{
					enemyHit = true;
					firstEnemyHit = hit.transform.gameObject;
					break;
				}
				else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Environment"))
				{
					if (Vector3.Distance(firstGroundHitPoint, Camera.main.transform.position) > Vector3.Distance(hit.point, Camera.main.transform.position))
					{
						firstGroundHitPoint = hit.point;
						firstGroundHitTransform = hit.transform;
					}
				}
				else if (hit.transform.gameObject.tag == "Item")
				{
					EventManager.Instance.TriggerEvent(new ItemClicked(hits[0].transform.GetComponent<ClickableItem>()));
				}
			}

			if (hits[0].transform.gameObject.tag == "Unfriendly" || enemyHit)
			{
				character.target = firstEnemyHit;
				attacking = true;
				agent.stoppingDistance = character.range;
				agent.SetDestination(firstEnemyHit.transform.position);

				Character currentTarget = firstEnemyHit.GetComponentInParent<Character>();
				if (currentTarget == null)
				{
					currentTarget = firstEnemyHit.GetComponent<Character>();
				}

				if (currentTarget != null)
				{
					if (!currentTarget.isDead)
					{
						EventManager.Instance.TriggerEvent(new EnemyClicked(currentTarget.gameObject));
						EventManager.Instance.TriggerEvent(new EnemyAttackedByLeaderEvent(currentTarget.gameObject.transform.parent.parent.gameObject));
					}
				}
			}
			else if (hits[0].transform.gameObject.tag == "Player")
			{
				attacking = false;
				agent.stoppingDistance = 1.2f;
			}
			else if (hits[0].transform.gameObject.tag == "Item")
			{
				EventManager.Instance.TriggerEvent(new ItemClicked(hits[0].transform.GetComponent<ClickableItem>()));
			}
			else
			{
				EventManager.Instance.TriggerEvent(new PositionClicked(firstGroundHitPoint, firstGroundHitTransform));
				agent.stoppingDistance = 1.2f;
				agent.SetDestination(new Vector3(firstGroundHitPoint.x, firstGroundHitPoint.y, firstGroundHitPoint.z));
				attacking = false;
			}
		}
	}
	private void Attacking()
	{
		character.isInCombat = true;
		agent.SetDestination(character.target.transform.position);
		distanceToTarget = Vector3.Distance(transform.position, character.target.transform.position);
		if (distanceToTarget < agent.stoppingDistance)
		{
			agent.Stop();
			character.RotateTowards(character.target.transform);
			if (character.isInCombat)
			{
				character.animator.SetBool("isAware", true);
			}
			else
			{
				character.animator.SetBool("isAware", false);
			}

			if (counter <= 0)
			{
				hasShot = false;
				character.animator.SetBool("isAware", true);
				character.animator.SetTrigger("Attack");
				counter = attackSpeed;
			}
			else
			{
				if (attackSpeed - 0.6f >= counter && !hasShot)
				{
					if (character.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
					{
						character.DealDamage();
						hasShot = true;
					}
				}
				counter -= Time.deltaTime;
			}
		}

	}
}

