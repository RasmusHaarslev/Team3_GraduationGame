using UnityEngine;
using System.Collections;

public class LoseScreenPanel : MonoBehaviour {

    public GameObject Panel1;
    public GameObject Panel2;
    public GameObject Text1;
    public GameObject Text2;

    void OnEnable()
    {
        EventManager.Instance.StartListening<GameLost>(ShowCanvas);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<GameLost>(ShowCanvas);
    }

    void OnApplicationQuit()
    {
        this.enabled = false;
    }

    void ShowCanvas (GameLost e) {
        Panel1.SetActive(true);
        Panel2.SetActive(true);

        if (e.LeaderDeath)
        {
            Text1.SetActive(true);
        }
        else
        {
            Text2.SetActive(true);
        }
    }

    public void Restart()
    {
        GameController.Instance.LoseGame();
    }
    
}
