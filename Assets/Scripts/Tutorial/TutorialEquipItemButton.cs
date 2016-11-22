using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialEquipItemButton : MonoBehaviour {
    
    public void ClickItem()
    { 

        DataService dataService = new DataService(StringResources.databaseName);

        List<EquippableitemValues> inventoryItems = new List<EquippableitemValues>(dataService.GetEquippableItemsValuesFromInventory());

        if (inventoryItems[0].Type == EquippableitemValues.type.rifle)
        {
            Manager_Audio.PlaySound(Manager_Audio.play_pickRiffle, gameObject);
        }
        else if (inventoryItems[0].Type == EquippableitemValues.type.polearm)
        {
            Manager_Audio.PlaySound(Manager_Audio.play_pickSpear, gameObject);
        }
        else if (inventoryItems[0].Type == EquippableitemValues.type.shield)
        {
            Manager_Audio.PlaySound(Manager_Audio.play_pickShield, gameObject);
        }

        Object.FindObjectOfType<PanelScript>().AssignWeaponToSoldier(inventoryItems[0]);
    }
}
