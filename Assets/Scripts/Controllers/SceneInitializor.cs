using UnityEngine;
using System.Collections;

public class SceneInitializor : MonoBehaviour {

    [Header("Camera prefabs")]
    public GameObject CameraPrefab;

    [Header("UI prefabs")]
    public GameObject[] UIPrefabs;

    // Use this for initialization
    void Start() {
        GameObject _camera = (GameObject)Instantiate(CameraPrefab);

        //_camera.GetComponent<CameraController>().

        foreach (GameObject ui in UIPrefabs) { 
            GameObject _ui = (GameObject)Instantiate(ui);
        }
    }
}
