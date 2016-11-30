using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIListEquippableItemController : MonoBehaviour, IPointerClickHandler
{
    public RawImage Icon;
    public Text name, damage, damageSpeed, range, health, type, level;
    public EquippableitemValues itemValues;
    public GameObject weaponCams;
    Color32 selectedColor = new Color32(0, 85, 250, 116);
    Color32 defaultColor = new Color32(154, 154, 154, 116);
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Image>().color != selectedColor)
        {
            foreach(Transform child in gameObject.transform.parent.transform)
            {
                if(child.GetComponent<Image>().color == selectedColor)
                {
                    child.GetComponent<Image>().color = defaultColor;
                }
            }

            GetComponent<Image>().color = new Color32(0, 85, 250, 116);

            if (itemValues.Type == EquippableitemValues.type.rifle)
            {
                Manager_Audio.PlaySound(Manager_Audio.play_pickRiffle, gameObject);
            }
            else if (itemValues.Type == EquippableitemValues.type.polearm)
            {
                Manager_Audio.PlaySound(Manager_Audio.play_pickSpear, gameObject);
            }
            else if (itemValues.Type == EquippableitemValues.type.shield)
            {
                Manager_Audio.PlaySound(Manager_Audio.play_pickShield, gameObject);
            }

            Object.FindObjectOfType<PanelScript>().AssignWeaponToSoldier(itemValues);
        }
    }

}