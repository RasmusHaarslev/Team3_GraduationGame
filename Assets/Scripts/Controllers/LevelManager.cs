using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private int AlliesAlive;
    private bool PlayerAlive;

    private int EnemiesAlive = 0;
    private int ItemsLeft = 0;

    private List<EquippableItem> gainedWeapons = new List<EquippableItem>();

    void Start () {
        PlayerAlive = true;
        AlliesAlive = 3;

    }

    #region Listeners
    void OnEnable()
    {
        //Enemy spawn for counting
        EventManager.Instance.StartListening<EnemySpawned>(EnemySpawn);
        EventManager.Instance.StartListening<EnemySpawned>(ItemSpawn);

        // -Collecting-
        //Loot received
        EventManager.Instance.StartListening<EnemyDeathEvent>(LootReceived);

        // -Win-
        //Enemy dies for progress
        EventManager.Instance.StartListening<EnemyDeathEvent>(EnemyDeath);

        // -Lose-
        //Player dies
        EventManager.Instance.StartListening<EnemyDeathEvent>(PlayerDeath);
        //Follower dies
        EventManager.Instance.StartListening<AllyDeathEvent>(AllyDeath);
    }

    void OnDisable()
    {
        //Enemy spawn for counting
        EventManager.Instance.StopListening<EnemySpawned>(EnemySpawn);
        EventManager.Instance.StopListening<EnemySpawned>(ItemSpawn);

        // -Collecting-
        //Loot received
        EventManager.Instance.StartListening<EnemyDeathEvent>(LootReceived);

        // -Win-
        //Enemy dies for progress
        EventManager.Instance.StopListening<EnemyDeathEvent>(EnemyDeath);

        // -Lose-
        //Player dies
        EventManager.Instance.StopListening<EnemyDeathEvent>(PlayerDeath);
        //Follower dies
        EventManager.Instance.StopListening<AllyDeathEvent>(AllyDeath);
    }
    #endregion

    #region functions
    private void EnemySpawn(EnemySpawned e)
    {
        EnemiesAlive++;
    }

    private void ItemSpawn(EnemySpawned e)
    {
        ItemsLeft++;
    }

    private void AllyDeath(AllyDeathEvent e)
    {
        AlliesAlive--;

        CheckConditions();
    }

    private void EnemyDeath(EnemyDeathEvent e)
    {
        EnemiesAlive--;

        CheckConditions();
    }

    void PlayerDeath(EnemyDeathEvent e)
    {
        LoseLevel();
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
            if (ItemsLeft <= 0) //Extra condition for the choice encounters
            {
                WinLevel();
            }
        }
    }

    public void LoseLevel()
    {
        EventManager.Instance.TriggerEvent(new LevelLost());
        PlayerPrefs.SetInt("LevelResult", 0);
        SceneManager.LoadScene("CampManagement");
    }

    public void WinLevel()
    {
        EventManager.Instance.TriggerEvent(new LevelWon());
        PlayerPrefs.SetInt("LevelResult", 1);
        SceneManager.LoadScene("CampManagement");
    }

}
