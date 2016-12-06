using UnityEngine;
using System.Collections;

public class GoToCamp : MonoBehaviour {

    public void LoadCampManagement()
    {
        GameController.Instance.LoadScene("CampManagement");
    }
}
