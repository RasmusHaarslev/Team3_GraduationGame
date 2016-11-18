using UnityEngine;
using System.Collections;

public class SceneInitializor : MonoBehaviour {

    [Header("UI prefabs")]
    public GameObject Camera;

    [Header("UI prefabs")]
    public GameObject[] UIPrefabs;

    // Use this for initialization
    void Start() {
        
        foreach (GameObject ui in UIPrefabs) { 
            GameObject _ui = (GameObject)Instantiate(ui);
        }
    }
}
