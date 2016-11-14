using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour {

    private int AlliesAlive;
    private bool PlayerAlive;

    private int EnemiesAlive = 0;

    void Start () {
        PlayerAlive = true;
        AlliesAlive = 3;

    }

    void OnEnable()
    {
        //Enemy spawn for counting
        EventManager.Instance.StartListening<EnemyDeathEvent>(EnemySpawn);

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
        EventManager.Instance.StopListening<EnemyDeathEvent>(EnemySpawn);

        // -Win-
        //Enemy dies for progress
        EventManager.Instance.StopListening<EnemyDeathEvent>(EnemyDeath);

        // -Lose-
        //Player dies
        EventManager.Instance.StopListening<EnemyDeathEvent>(PlayerDeath);
        //Follower dies
        EventManager.Instance.StopListening<AllyDeathEvent>(AllyDeath);
    }

    private void EnemySpawn(EnemyDeathEvent e)
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
        EventManager.Instance.TriggerEvent()
    }

    void WinLevel()
    {

    }

}
