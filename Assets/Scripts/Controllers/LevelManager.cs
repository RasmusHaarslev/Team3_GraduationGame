using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	private DataService dataService = new DataService(StringResources.databaseName);
	private int AlliesAlive;
	private bool PlayerAlive;

	[SerializeField]
	private int EnemiesAlive = 0;
	private int ItemsLeft = 0;
	public bool inCombat = false;

	public string NextLevel = "";
	public bool IsTutorial = false;

	private List<EquippableItem> gainedWeapons = new List<EquippableItem>();
	public List<GameObject> huntersAndPlayer = new List<GameObject>();
	LevelGenerator levelGenerator;

	void Start()
	{
		PlayerAlive = true;
		AlliesAlive = 3;
		levelGenerator = GetComponent<LevelGenerator>();
		if (levelGenerator.isTutorial)
		{
			EnemiesAlive = GameObject.FindGameObjectsWithTag("Unfriendly").Length / 2;
		}
	}

	void Update()
	{ 
		if (huntersAndPlayer.Count == 0)
		{
			huntersAndPlayer.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
			huntersAndPlayer.Add(GameObject.FindGameObjectWithTag("Player"));
		}
		UpdateState();
	}

	private void SetGameState(bool inCombat)
	{
		if (this.inCombat == false && inCombat)
		{
			Manager_Audio.ChangeState(Manager_Audio.playStateGroupContainer, Manager_Audio.fightSnapshot);
		}
		else if (this.inCombat == true && !inCombat && !huntersAndPlayer[huntersAndPlayer.Count - 1].GetComponent<Character>().isDead)
		{
			Manager_Audio.ChangeState(Manager_Audio.playStateGroupContainer, Manager_Audio.exploreSnapshot);
		}

		this.inCombat = inCombat;
	}

	void UpdateState()
	{
		foreach (var friendly in huntersAndPlayer)
		{
			if (friendly.GetComponent<Character>().isInCombat)
			{
				SetGameState(true);
				return;
			}
		}
		SetGameState(false);
	}

	#region Listeners
	void OnEnable()
	{
		Manager_Audio.ChangeState(Manager_Audio.playStateGroupContainer, Manager_Audio.exploreSnapshot);

		//Enemy spawn for counting
		EventManager.Instance.StartListening<EnemySpawned>(EnemySpawn);
		EventManager.Instance.StartListening<ItemSpawned>(ItemSpawn);

		// -Collecting-
		//Loot received
		EventManager.Instance.StartListening<EnemyDeathEvent>(LootReceived);

		// -Win-
		//Enemy dies for progress
		EventManager.Instance.StartListening<EnemyDeathEvent>(EnemyDeath);

		// -Lose-
		//Player dies

		EventManager.Instance.StartListening<PlayerDeathEvent>(PlayerDeath);
		//Follower dies
		EventManager.Instance.StartListening<AllyDeathEvent>(AllyDeath);
	}


	void OnDisable()
	{
		GameController.Instance.numberOfActiveUIs = 0;
		//Enemy spawn for counting
		EventManager.Instance.StopListening<EnemySpawned>(EnemySpawn);
		EventManager.Instance.StopListening<ItemSpawned>(ItemSpawn);

		// -Collecting-
		//Loot received
		EventManager.Instance.StopListening<EnemyDeathEvent>(LootReceived);

		// -Win-
		//Enemy dies for progress
		EventManager.Instance.StopListening<EnemyDeathEvent>(EnemyDeath);

		// -Lose-
		//Player dies
		EventManager.Instance.StopListening<PlayerDeathEvent>(PlayerDeath);
		//Follower dies
		EventManager.Instance.StopListening<AllyDeathEvent>(AllyDeath);
	}

	void OnApplicationQuit()
	{
		this.enabled = false;
	}
	#endregion

	#region functions
	private void EnemySpawn(EnemySpawned e)
	{
		EnemiesAlive++;
	}

	private void ItemSpawn(ItemSpawned e)
	{
		ItemsLeft++;
	}

	private void AllyDeath(AllyDeathEvent e)
	{

		//removing hunter from database
		dataService.RemoveCharacter(e.deadAlly.characterBaseValues);

		AlliesAlive--;

		CheckConditions();
	}

	private void EnemyDeath(EnemyDeathEvent e)
	{
		EnemiesAlive--;
		CheckConditions();
	}

	void PlayerDeath(PlayerDeathEvent e)
	{
		if (!IsTutorial)
		{
			LoseGame("CampManagement");
		}
		else
		{
			LoseGame(SceneManager.GetActiveScene().name);
		}
	}

	void LootReceived(EnemyDeathEvent e)
	{

	}

	void CollectLoot()
	{
		gainedWeapons.Add(new EquippableItem());

		CheckConditions();
	}

	#endregion

	void CheckConditions()
	{
		if (EnemiesAlive <= 0) //Shouldn't ever go below 0, but still
		{
			WinLevel();
		}
		else if (AlliesAlive <= 0 && GameController.Instance._VILLAGERS <= 0)
		{
			if (!IsTutorial)
			{
				LoseGame("CampManagement");
			}
			else
			{
				LoseGame(SceneManager.GetActiveScene().name);
			}
		}
		else if (AlliesAlive <= 0)
		{
			LoseLevel();
		}
	}

	public void LoseGame(string scene = "CampManagement")
	{
		Manager_Audio.ChangeState(Manager_Audio.playStateGroupContainer, Manager_Audio.loseState);
		Camera.main.GetComponent<CameraDeathEffect>().TriggerDeath();
		StartCoroutine(LoseGameCoroutine(scene));
	}

	IEnumerator LoseGameCoroutine(string scene = "CampManagement")
	{
		while (Time.timeScale > 0.2f)
		{
			Time.timeScale -= 0.1f;

			yield return new WaitForSeconds(0.25f);
		}

		if (!IsTutorial) { 
			GameController.Instance.LoseGame();
            yield break;
        }

        GameController.Instance.LoadScene(scene);

		Time.timeScale = 1f;

		yield return new WaitForSeconds(0f);
	}

	public void LoseLevel()
	{
		Manager_Audio.ChangeState(Manager_Audio.playStateGroupContainer, Manager_Audio.loseState);
		EventManager.Instance.TriggerEvent(new LevelLost());
		PlayerPrefs.SetInt("LevelResult", 0);

		GameController.Instance.LoadScene("LevelFleeCutscene");
	}

	public void WinLevel()
	{
		Manager_Audio.ChangeState(Manager_Audio.playStateGroupContainer, Manager_Audio.winState);
		StartCoroutine(WinGameCoroutine());
	}

	IEnumerator WinGameCoroutine(string scene = "CampManagement")
	{
		while (Time.timeScale > 0.2f)
		{
			Time.timeScale -= 0.1f;

			yield return new WaitForSeconds(0.25f);
		}

		yield return new WaitForSeconds(0.5f);

		EventManager.Instance.TriggerEvent(new LevelWon());

		Time.timeScale = 1f;

        if (IsTutorial)
		{
			EventManager.Instance.TriggerEvent(new TutorialDone());
			yield break;
		}

		if (GameController.Instance.numberOfActiveUIs == 0)
		{
			EventManager.Instance.TriggerEvent(new UIPanelActiveEvent(false));
		}
		GameController.Instance.numberOfActiveUIs++;
        EventManager.Instance.TriggerEvent(
            new ChangeResources(
                food: PlayerPrefs.GetInt("FoodAmount"),
                scraps: PlayerPrefs.GetInt("ScrapAmount"),
                premium: PlayerPrefs.GetInt(StringResources.PremiumDropAmountPrefsName)
            )
        );

        PlayerPrefs.SetInt("LevelResult", 1);
		GameObject.FindGameObjectWithTag("Player").GetComponent<MoveScript>().enabled = false;
		//generate and display the new items
		GenerateNewItems();
		Time.timeScale = 1f;
		yield return new WaitForSeconds(0f);
	}

	//called on the canvas button of the new generated items
	public void levelWonEnding()
	{
		GameController.Instance.LoadScene("LevelWinCutscene");
	}


	void replaceCharactersWeapons()
	{
		WeaponGenerator weaponGenerator = new WeaponGenerator();
		DataService dataService = new DataService(StringResources.databaseName);
		GameObject playerFellowship = GameObject.Find("PlayerFellowship");
		int level = PlayerPrefs.GetInt(StringResources.hardnessLevel, 4);

		foreach (Character character in playerFellowship.transform.GetComponentsInChildren<Character>())
		{
			EquippableitemValues oldEquippableitemValues = character.GetComponentInChildren<EquippableItem>().itemValues;
			EquippableitemValues newItemValues = weaponGenerator.GenerateEquippableItem(
				character.GetComponentInChildren<EquippableItem>().itemValues.Type, level);

			GameObject newItem =
				dataService.GenerateNewEquippableItemFromValues(newItemValues);
			dataService.equipItemsToCharacter(new List<GameObject>() { newItem }, character);
		}
	}

	void GenerateNewItems()
	{
		int difficultyLevel = GetComponent<LevelGenerator>().difficultyLevel;
		WeaponGenerator weaponsGenerator = GetComponent<WeaponGenerator>();

		//fill a list with the new items values
		EquippableitemValues[] newItemsValues = weaponsGenerator.GetNewItemsValues(difficultyLevel);

		//Generate new weapons values

		//save them into database inventory
		DataService dataService = new DataService(StringResources.databaseName);
		dataService.InsertItemsValuesInInventory(newItemsValues);
		LevelCanvasManager canvasManager = GetComponentInChildren<LevelCanvasManager>();
		canvasManager.DisplayEndLootItems(newItemsValues);
	}
}
