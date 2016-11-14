using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    private int AlliesAlive;
    private bool PlayerAlive;

    private int EnemiesAlive = 0;

    private List<EquippableItem> gainedWeapons = new List<EquippableItem>();

    void Start () {
        PlayerAlive = true;
        AlliesAlive = 3;

    }

    void OnEnable()
    {
        //Enemy spawn for counting
        EventManager.Instance.StartListening<EnemySpawned>(EnemySpawn);

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

    private void EnemySpawn(EnemySpawned e)
    {
        EnemiesAlive++;
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
    }

    void CheckConditions()
    {
        if (EnemiesAlive <= 0) //Shouldn't ever go below 0, but still
        {
            //Extra condition for the choice encounters

            WinLevel();
        }
    }

    void LoseLevel()
    {
        EventManager.Instance.TriggerEvent(new LevelWon());
    }

    void WinLevel()
    {
        EventManager.Instance.TriggerEvent(new LevelLost());
    }

}
