using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class TutorialHunterCharacter : MonoBehaviour
{
	//values gained from the database
	public CharacterValues characterBaseValues;

	//combat values
	public int health = 0;
	public int damage = 0;
	public int range = 0;
	public float damageSpeed = 0;
	public float currentHealth;

	NavMeshAgent agent;
	[HideInInspector]
	public Animator animator;
	public GameObject target;
	GameObject targetParent;
	GameObject parent;
	public List<GameObject> currentOpponents = new List<GameObject>();
	public bool isCurrentTargetAlive;
	public float rotationSpeed = 2;

	//Combat state values
	public bool isInCombat = false;
	public bool isDead = false;
	public bool isFleeing = false;
	bool deadEvent = false;
	[Range(0, 99)]
	public int randomTargetProbability = 25;
	float isFleeingValue;

	//model values
	//private Dictionary<string, Transform> slots;
	public Dictionary<EquippableitemValues.slot, Transform> equippableSpots;

	public Transform rightHandSlot;
	public Transform leftHandSlot;
	public Transform headSlot;
	public Transform torsoSlot;
	public Transform legsSlot;

	public EquippableitemValues.type equippedWeaponType;
	public CharacterValues.type characterType;

	public bool isMale = true;

	// Use this for initialization
	void Start()
	{
		currentHealth = health;
	}

	void OnApplicationQuit()
	{
		this.enabled = false;
	}

	void Update()
	{
		//isFleeingValue = isFleeing ? 1 : 0;
		//animator.SetFloat("isWounded", isFleeingValue);
		if (agent.velocity.normalized.magnitude < 0.2f)
		{
			animator.SetBool("isAware", isInCombat);
		}
		animator?.SetFloat("Speed", agent.velocity.normalized.magnitude, 0.15f, Time.deltaTime);
		if (currentHealth <= 0)
		{
			if (isDead == false)
			{
				GetComponent<Collider>().enabled = false;
				agent.enabled = false;
				// VESO REMOVE THIS:
				GetComponent<RagdollControl>().EnableRagDoll();
				animator.SetTrigger("Die");
				if (deadEvent == false)
				{
					if (isMale)
					{
						Manager_Audio.PlaySound(Manager_Audio.deathMale1, this.gameObject);
					}
					else
					{
						Manager_Audio.PlaySound(Manager_Audio.deathFemale1, this.gameObject);
					}
					deadEvent = true;
				}
			}
			isInCombat = false;
			isDead = true;
		}
		if (!isInCombat)
		{
			currentOpponents.Clear();
		}
	}

	void OnEnable()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();

		EventManager.Instance.StartListening<EnemySpottedEvent>(StartCombatState);
		EventManager.Instance.StartListening<TakeDamageEvent>(TakeDamage);
		EventManager.Instance.StartListening<EnemyDeathEvent>(EnemyDeath);
		EventManager.Instance.StartListening<CommandEvent>(CommandAnimator);

		damageSpeed = Mathf.Clamp(damageSpeed, 1.1f, damageSpeed);
		//DO NOT initialize here the equipped weapon type, because it is already done when a weapon is equipped !!//equippedWeaponType = GetComponentInChildren<EquippableItem>().itemValues.Type;
	}

	private void CommandAnimator(CommandEvent e)
	{
		animator.SetTrigger("IssueCommand");
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<EnemySpottedEvent>(StartCombatState);
		EventManager.Instance.StopListening<TakeDamageEvent>(TakeDamage);
		EventManager.Instance.StopListening<EnemyDeathEvent>(EnemyDeath);
		EventManager.Instance.StopListening<CommandEvent>(CommandAnimator);
	}

	// Finds the appropriate target based on traits
	public void TargetOpponent()
	{
		if (currentOpponents.Count == 0)
		{
			FindCurrentOpponents();
		}
		target = FindNearestEnemy();
	}

	private void FindCurrentOpponents()
	{
		foreach (Transform child in targetParent.transform)
		{
			foreach (Transform child2 in child)
			{
				if (child2.gameObject.tag == "Unfriendly")
				{
					currentOpponents.Add(child2.gameObject);
				}
			}
		}
	}

	public GameObject FindNearestEnemy()
	{
		GameObject finalTarget;
		finalTarget = null;
		float min = float.MaxValue;

		foreach (var possibleTarget in currentOpponents)
		{
			float distances;
			distances = Vector3.Distance(transform.position, possibleTarget.transform.position);
			if (distances < min)
			{
				min = distances;
				finalTarget = possibleTarget;
			}
		}
		return finalTarget;
	}

	public GameObject FindRandomEnemy()
	{
		GameObject finalTarget;
		finalTarget = currentOpponents[UnityEngine.Random.Range(0, currentOpponents.Count)];
		return finalTarget;
	}

	private void StartCombatState(EnemySpottedEvent e)
	{
		if (!isInCombat)
		{
			targetParent = e.parent;
			TargetOpponent();
			isInCombat = true;
		}
	}

	public void DealDamage()
	{
		EventManager.Instance.TriggerEvent(new TakeDamageEvent(damage, target));
		switch (equippedWeaponType)
		{
			case EquippableitemValues.type.polearm:
				Manager_Audio.PlaySound(Manager_Audio.attackSpear, this.gameObject);
				break;
			case EquippableitemValues.type.rifle:
				Manager_Audio.PlaySound(Manager_Audio.attackRiffle, this.gameObject);
				break;
			case EquippableitemValues.type.shield:
				Manager_Audio.PlaySound(Manager_Audio.attackShield, this.gameObject);
				break;
		}

		if (isMale)
		{
			Manager_Audio.PlaySound(Manager_Audio.attackMale1, this.gameObject);
		}
		else
		{
			Manager_Audio.PlaySound(Manager_Audio.attackFemale1, this.gameObject);
		}

		if (target != null)
		{
			if (target.GetComponent<HunterStateMachine>() != null)
			{
				if (target.GetComponent<HunterStateMachine>().combatCommandState == HunterStateMachine.CombatCommandState.Defense)
				{
					target.GetComponent<Character>().target = gameObject;
					target.GetComponent<HunterStateMachine>().attacked = true;
				}
			}
		}
	}

	private void TakeDamage(TakeDamageEvent e)
	{
		if (e.target == gameObject)
		{
			Manager_Audio.PlaySound(Manager_Audio.genericHit, this.gameObject);

			if (equippedWeaponType == EquippableitemValues.type.shield)
			{
				Manager_Audio.PlaySound(Manager_Audio.shieldHit, this.gameObject);
			}

			currentHealth -= e.damage;
		}
	}

	public void RotateTowards(Transform target)
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
		transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
	}

	private void EnemyDeath(EnemyDeathEvent e)
	{
		currentOpponents.Remove(e.enemy);
	}
}

