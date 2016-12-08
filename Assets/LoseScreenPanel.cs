using UnityEngine;
using System.Collections;

public class LoseScreenPanel : MonoBehaviour {

    public GameObject Panel1;
    public GameObject Panel2;

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
    }

    public void Restart()
    {
        GameController.Instance.LoseGame();
    }
    
}
