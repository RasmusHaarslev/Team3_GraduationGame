using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Character : MonoBehaviour
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

		if (target != null)
		{
			if (target.GetComponent<Character>() != null)
			{
				if (target.GetComponent<Character>().isFleeing)
				{
					currentOpponents.Clear();
					target = null;
					isInCombat = false;
				}
			} else if (target.GetComponent<TutorialCharacter>() != null)
			{
				if (target.GetComponent<TutorialCharacter>().isFleeing)
				{
					isInCombat = false;
					currentOpponents.Clear();
					target = null;
				}
			}
		}

		if (currentHealth <= 0)
		{
			if (isDead == false && characterBaseValues.Type == CharacterValues.type.Hunter)
			{
				GetComponent<Collider>().enabled = false;
				agent.enabled = false;
				// VESO REMOVE THIS:
				GetComponent<RagdollControl>().EnableRagDoll();
				animator.SetTrigger("Die");
				if (deadEvent == false)
				{
					EventManager.Instance.TriggerEvent(new AllyDeathEvent(this));
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
			else if (isDead == false && (characterBaseValues.Type == CharacterValues.type.Wolf || characterBaseValues.Type == CharacterValues.type.Tribesman))
			{
				if (!isFleeing)
				{
					EventManager.Instance.TriggerEvent(new EnemyDeathEvent(gameObject));
				}
				GetComponent<Collider>().enabled = false;
				agent.enabled = false;
				animator.SetTrigger("Die");
				GetComponentInChildren<AggroRange>().gameObject.SetActive(false);
				// VESO REMOVE THIS:
				GetComponent<RagdollControl>().EnableRagDoll();

				if (isMale)
				{
					Manager_Audio.PlaySound(Manager_Audio.evilDeathMale1, this.gameObject);
				}
				else
				{
					Manager_Audio.PlaySound(Manager_Audio.evilDeathFemale1, this.gameObject);
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

		equippableSpots = new Dictionary<EquippableitemValues.slot, Transform>(){ //TODO: chage gameObject of this list
		{EquippableitemValues.slot.head, headSlot },
		{EquippableitemValues.slot.torso, torsoSlot },
		{EquippableitemValues.slot.leftHand, leftHandSlot },
		{EquippableitemValues.slot.rightHand, rightHandSlot},
		{EquippableitemValues.slot.legs, legsSlot }
	};
		damageSpeed = Mathf.Clamp(damageSpeed, 1.1f, damageSpeed);
		//DO NOT initialize here the equipped weapon type, because it is already done when a weapon is equipped !!//equippedWeaponType = GetComponentInChildren<EquippableItem>().itemValues.Type;
	}

	private void CommandAnimator(CommandEvent e)
	{
		Debug.Log("received ");
		animator.SetTrigger("IssueCommand");
	}

	void OnDisable()
	{
		EventManager.Instance.StopListening<EnemySpottedEvent>(StartCombatState);
		EventManager.Instance.StopListening<TakeDamageEvent>(TakeDamage);
		EventManager.Instance.StopListening<EnemyDeathEvent>(EnemyDeath);
		EventManager.Instance.StopListening<CommandEvent>(CommandAnimator);
	}

	/// <summary>
	/// Set the character values passed in the parameter
	/// </summary>
	/// <param name="initValues"></param>
	public void init(CharacterValues initValues)
	{
		characterBaseValues = initValues;
		//setting the first summary values for the player. Those will be then increased by weapon stats when one is quipped.
		health = initValues.health;
		range = initValues.range;
		damage = initValues.damage;
		damageSpeed = initValues.damageSpeed;
		currentHealth = health;
		if (characterBaseValues.Type == CharacterValues.type.Hunter || characterBaseValues.Type == CharacterValues.type.Player || characterBaseValues.Type == CharacterValues.type.Tribesman)
		{
			animator = GetComponent<Animator>();
		}
		isMale = initValues.isMale;
	}

	/// <summary>
	/// Changes the stats and spawn the item on the right character slot
	/// </summary>
	/// <param name="item"></param>
	void EquipItems(IEnumerable<GameObject> equips)
	{
		EquippableitemValues currentEquipValues;

		foreach (GameObject equip in equips)
		{
			currentEquipValues = equip.GetComponent<EquippableItem>().itemValues;
			if (currentEquipValues != null)
			{
				//checking if another item is equipped in the item slot
				if (equippableSpots[currentEquipValues.Slot].GetComponentInChildren<EquippableItem>() != null)
				{
					//if thats the case, remove the values and remove the old object
					DetachItem(currentEquipValues.Slot);
				}
				//parent and position the item on the appropriate slot
				equip.transform.parent = equippableSpots[currentEquipValues.Slot]; equip.transform.localPosition = Vector3.zero;
				//add the new item values
				health += currentEquipValues.health;

				damage += currentEquipValues.damage;
				damageSpeed = currentEquipValues.damageSpeed;
				range = currentEquipValues.range;
			}
			else
			{
				print("Trying to equip " + equip.name + " that is not an equippable item!");
			}
		}
	}

	void DetachItem(EquippableitemValues.slot slotToDetatch)
	{
		//remove item values from total on the player
		EquippableitemValues itemValuesToDetatch = equippableSpots[slotToDetatch].GetComponentInChildren<EquippableitemValues>();
		//detatch and remove the item from the game
		//TODO complete here and find a way to communicate with the database
	}

	// Finds the appropriate target based on traits
	public void TargetOpponent()
	{
		if (currentOpponents.Count == 0)
		{
			FindCurrentOpponents();
		}

		if (characterBaseValues.Type == CharacterValues.type.Wolf || characterBaseValues.Type == CharacterValues.type.Tribesman)
		{
			foreach (GameObject opp in currentOpponents)
			{
				var hunter = opp.GetComponent<HunterStateMachine>();
				if (UnityEngine.Random.Range(0, 100) < randomTargetProbability)
				{
					target = FindRandomEnemy();
				}
				else
				{
					target = FindNearestEnemy();
				}
			}
		}
		else
		{
			target = FindNearestEnemy();
		}
	}

	private void FindCurrentOpponents()
	{
		if (gameObject.tag == "Unfriendly")
		{
			if (characterBaseValues.Type == CharacterValues.type.Wolf || characterBaseValues.Type == CharacterValues.type.Tribesman)
			{
				List<GameObject> friendlies = new List<GameObject>();
				friendlies.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
				friendlies.Add(GameObject.FindGameObjectWithTag("Player"));

				foreach (GameObject child in friendlies)
				{
					currentOpponents.Add(child);
				}

			}
		}
		else
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
			if (characterBaseValues.Type == CharacterValues.type.Hunter || ((characterBaseValues.Type == CharacterValues.type.Wolf || characterBaseValues.Type == CharacterValues.type.Tribesman) && e.parent == gameObject.transform.parent.parent.gameObject))
			{
				targetParent = e.parent;
				TargetOpponent();
				isInCombat = true;
			}
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
			if (characterBaseValues.Type == CharacterValues.type.Hunter || characterBaseValues.Type == CharacterValues.type.Player)
			{
				Manager_Audio.PlaySound(Manager_Audio.attackMale1, this.gameObject);
			}
			else
			{
				Manager_Audio.PlaySound(Manager_Audio.evilAttackMale1, this.gameObject);
			}
		}
		else
		{
			if (characterBaseValues.Type == CharacterValues.type.Hunter || characterBaseValues.Type == CharacterValues.type.Player)
			{
				Manager_Audio.PlaySound(Manager_Audio.attackFemale1, this.gameObject);
			}
			else
			{
				Manager_Audio.PlaySound(Manager_Audio.evilAttackFemale1, this.gameObject);
			}
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
		if (e.enemy == target && (characterBaseValues.Type == CharacterValues.type.Player))
		{
			currentOpponents.Remove(target);
			target = null;
		}
		if (characterBaseValues.Type == CharacterValues.type.Hunter)
		{
			currentOpponents.Remove(e.enemy);
		}
	}
}

