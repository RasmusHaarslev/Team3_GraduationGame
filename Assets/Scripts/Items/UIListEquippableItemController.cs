using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIListEquippableItemController : MonoBehaviour, IPointerClickHandler
{
    public RawImage Icon;
    public Text name, damage, damageSpeed, range, health;
    public EquippableitemValues itemValues;
    public GameObject weaponCams;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemValues.Type == EquippableitemValues.type.rifle) {
            Manager_Audio.PlaySound(Manager_Audio.play_pickRiffle, gameObject);
        } else if (itemValues.Type == EquippableitemValues.type.polearm)
        {
            Manager_Audio.PlaySound(Manager_Audio.play_pickSpear, gameObject);
        } else if (itemValues.Type == EquippableitemValues.type.shield)
        {
            Manager_Audio.PlaySound(Manager_Audio.play_pickShield, gameObject);
        }

        Object.FindObjectOfType<PanelScript>().AssignWeaponToSoldier(itemValues);
    }

}