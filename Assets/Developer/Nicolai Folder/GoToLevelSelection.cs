using UnityEngine;
using System.Collections;

public class GoToLevelSelection : MonoBehaviour {

    public void GoToCamp()
    {
        GameController.Instance.LoadScene("LevelSelection");
    }
}
