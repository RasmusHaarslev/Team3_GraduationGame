using UnityEngine;
using System.Collections;


public class PointOfInterestManager : MonoBehaviour
{

    public EncounterType type;
    [Range(3, 20)]
    public int radius = 8;

    public enum EncounterType
    {
        choice,
        rival,
        wolf
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
        
    }

    // Use this for initialization
    void Start () {

        //activate the point of interest decoraction
        switch (type)
        {
          case EncounterType.choice:
                transform.Find("ChoiceDecor").gameObject.SetActive(true);
                break;
            case EncounterType.rival:
                transform.Find("RivalDecor").gameObject.SetActive(true); 
                break;
            case EncounterType.wolf:
                transform.Find("WolfDecor").gameObject.SetActive(true); 
                break;
        }
        
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}

}
