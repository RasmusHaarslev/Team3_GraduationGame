using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIListEquippableItemController : MonoBehaviour, IPointerClickHandler
{

    public RawImage Icon;
    public Text name, damage, damageSpeed, range, health;
    public EquippableitemValues itemValues;

    public void OnPointerClick(PointerEventData eventData)
    {
        Object.FindObjectOfType<PanelScript>().AssignWeaponToSoldier(itemValues);

    }

}