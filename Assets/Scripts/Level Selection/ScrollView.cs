using UnityEngine;
using System.Collections;

public class ScrollView : MonoBehaviour {

    void OnTriggerExit2D(Collider2D other)
    {
       // other.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      //  other.gameObject.SetActive(true);
    }
}
