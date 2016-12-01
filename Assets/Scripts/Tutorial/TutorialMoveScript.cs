using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TutorialMoveScript : MonoBehaviour
{

	NavMeshAgent agent;
	public bool movement = true;
	public bool attacking = false;
	TutorialPlayerCharacter character;
	float distanceToTarget;
	float attackSpeed;
	float counter = 0;
	bool attack = false;
	bool isDead = false;
	bool isFleeing = false;
	public List<GameObject> hunters = new List<GameObject>();

	// Use this for initialization
	void Start()
	{
		character = GetComponent<TutorialPlayerCharacter>();
		agent = GetComponent<NavMeshAgent>();
	}

	void OnEnable()
	{
		EventManager.Instance.StartListening<FleeStateEvent>(Flee);
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<FleeStateEvent>(Flee);
	}

	private void Flee(FleeStateEvent e)
	{
		isFleeing = true;
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
			hunters.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
		}
		int counter = 0;
		foreach (var hunter in hunters)
		{
			if (hunter.GetComponent<TutorialHunterCharacter>().isInCombat)
			{
				character.isInCombat = true;
				counter++;
			}
		}
		if (counter == 0 && character != null)
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
				if (Input.GetKeyDown(KeyCode.Mouse0))
					Manager_Audio.PlaySound(Manager_Audio.walkTapUISound, this.gameObject);
				if (Input.GetKey(KeyCode.Mouse0))
				{
					agent.Resume();
					character.isInCombat = false;
					//attacking = false;
					MoveToClickPosition();
				}
				if (attacking)
				{
					if (character.target != null)
					{
						if (character.target.GetComponent<TutorialCharacter>() != null)
						{
							if (!character.target.GetComponent<TutorialCharacter>().isDead)
							{
								Attacking();
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
			}
		}
		else
		{
			agent.SetDestination(GameObject.FindGameObjectWithTag("FleePoint").transform.position);
			StartCoroutine(LoseScene());
		}
	}

	IEnumerator LoseScene()
	{
		yield return new WaitForSeconds(5);

		SceneManager.LoadScene("LevelFleeCutscene");
		yield return null;
	}

	public void MoveToClickPosition()
	{
		agent.Resume();
		character.animator.SetBool("isAware", false);
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
		{
			if (hit.transform.gameObject.tag == "Unfriendly")
			{
				character.target = hit.transform.gameObject;
				attacking = true;
				agent.stoppingDistance = character.range;
				agent.SetDestination(hit.transform.position);
				if (hit.transform.gameObject.GetComponent<TutorialCharacter>() != null)
				{
					if (!character.isInCombat && !hit.transform.gameObject.GetComponent<TutorialCharacter>().isDead)
					{
						EventManager.Instance.TriggerEvent(new EnemyClicked(hit.transform.gameObject));
						EventManager.Instance.TriggerEvent(new EnemyAttackedByLeaderEvent(hit.transform.gameObject));
					}
				}
			}
			else if (hit.transform.gameObject.tag == "Player")
			{
				attacking = false;
				agent.stoppingDistance = 1.2f;
			}
			else if (hit.transform.gameObject.tag == "Item")
			{
				EventManager.Instance.TriggerEvent(new ItemClicked(hit.transform.GetComponent<ClickableItem>()));
			}
			else
			{
				EventManager.Instance.TriggerEvent(new PositionClicked(hit.point, hit.transform));
				agent.stoppingDistance = 1.2f;
				agent.SetDestination(new Vector3(hit.point.x, hit.point.y, hit.point.z));
				attacking = false;
			}
		}
	}
	private void Attacking()
	{
		character.isInCombat = true;
		agent.SetDestination(character.target.transform.position);
		distanceToTarget = agent.remainingDistance;
		if (distanceToTarget < agent.stoppingDistance)
		{
			agent.Stop();
			character.RotateTowards(character.target.transform);
			if (character.isInCombat)
			{
				if (!character.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
				{
					character.animator.SetBool("isAware", true);
				}
				else
				{
					character.animator.SetBool("isAware", false);
				}
			}
			else
			{
				character.animator.SetBool("isAware", false);
			}
			if (counter <= 0)
			{
				character.DealDamage();
				character.animator.SetTrigger("Attack");
				counter = attackSpeed;
			}
			else
			{
				counter -= Time.deltaTime;
			}
		}
	}


}

