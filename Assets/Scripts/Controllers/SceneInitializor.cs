using UnityEngine;
using System.Collections;

public class SceneInitializor : MonoBehaviour {

    [Header("UI prefabs")]
    public GameObject Camera;

    [Header("GameController")]
    public GameObject GameController;

    [Header("UI prefabs")]
    public GameObject[] UIPrefabs;

    void Awake()
    {
#if UNITY_EDITOR
        if (GameObject.FindObjectsOfType<GameController>().Length == 0)
        {
            Instantiate(GameController);
        }
#endif
    }

    // Use this for initialization
    void Start() {
        foreach (GameObject ui in UIPrefabs) { 
            GameObject _ui = (GameObject)Instantiate(ui);
        }
    }
}
