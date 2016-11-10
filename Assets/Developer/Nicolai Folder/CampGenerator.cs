using UnityEngine;
using System.Collections;

public class CampGenerator : MonoBehaviour
{

    private DataService dataService;

    // Use this for initialization
    void Start()
    {

        dataService = new DataService("tempDatabase.db");
        //dataService.CreateDB();
        GameObject daniel = dataService.GenerateCharacterByName("Daniel", new Vector3(0, 0, 0.1f), Quaternion.Euler(0, -205, 0));
        GameObject john = dataService.GenerateCharacterByName("John", new Vector3(-0.8f, 0, -1), Quaternion.Euler(0, -225, 0));
        GameObject nicolai = dataService.GenerateCharacterByName("Nicolai", new Vector3(1.1f, 0, 0), Quaternion.Euler(0, 208, 0));
        GameObject peter = dataService.GenerateCharacterByName("Peter", new Vector3(1.7f, 0, -0.7f), Quaternion.Euler(0, 222, 0));

        daniel.gameObject.GetComponent<MoveScript>().enabled = false;
        daniel.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        john.gameObject.GetComponent<HunterStateMachine>().enabled = false;
        john.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        nicolai.gameObject.GetComponent<HunterStateMachine>().enabled = false;
        nicolai.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        peter.gameObject.GetComponent<HunterStateMachine>().enabled = false;
        peter.gameObject.GetComponent<NavMeshAgent>().enabled = false;


        print(john.GetComponent<Character>().characterBaseValues.name);

    }
}
