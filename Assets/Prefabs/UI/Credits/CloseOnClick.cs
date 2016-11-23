using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CloseOnClick : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
