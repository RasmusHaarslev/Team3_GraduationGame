using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceTextController : MonoBehaviour
{
    public string resourceName;

	// Use this for initialization
	void Start ()
	{
	    int resourceQuantity = PlayerPrefs.GetInt(resourceName, 0);

        GetComponent<Text>().text = resourceQuantity.ToString();
	}	
}
