using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    #region Setup Instance
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameController");
                go.AddComponent<GameController>();
            }
            return _instance;
        }
    }
    #endregion

    void Start()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int scene)
    {
        if (SceneTransistion.instance != null)
        {
            SceneTransistion.instance.LoadScene(scene);
        }
        else
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
