using UnityEngine;
using System.Collections;

public class EquippableItemListController : MonoBehaviour
{

    public Sprite[] AnimalImages;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

    EquippableitemValues[] itemsValues;

    void Start()
    {
        ListItemPrefab = Resources.Load(StringResources.uiPrefabsPath + "EquippableItemUIScrollElement") as GameObject;
        // 1. Get the data to be displayed
        itemsValues = new[] {
            new EquippableitemValues
        {
            id = 4,
            name = "Rifle of the Git Master Rasmus",
            Type = EquippableitemValues.type.rifle,
            Slot = EquippableitemValues.slot.rightHand,
            health = 25,
            damage = 15,
            damageSpeed = 15,
            range = 15,
            prefabName = "Rifle"
        },
             new EquippableitemValues
             {
                 id = 5,
                 name = "Mighty power Stick",
                 Type = EquippableitemValues.type.polearm,
                 Slot = EquippableitemValues.slot.rightHand,
                 health = 20,
                 damage = 20,
                 damageSpeed = 9,
                 range = 5,
                 prefabName = "Stick"
             },
        new EquippableitemValues
        {
            id = 6,
            name = "Romanian Steel Bar",
            Type = EquippableitemValues.type.polearm,
            Slot = EquippableitemValues.slot.rightHand,
            health = 20,
            damage = 25,
            damageSpeed = 9,
            range = 2,
            prefabName = "Stick"
        }
        };

        // 2. Iterate through the data, 
        //	  instantiate prefab, 
        //	  set the data, 
        //	  add it to panel
        foreach (EquippableitemValues values in itemsValues)
        {
            GameObject newItem = Instantiate( ListItemPrefab) as GameObject;
            UIListEquippableItemController controller = newItem.GetComponent<UIListEquippableItemController>();
            //controller.Icon.sprite = animal.Icon;
            controller.Name.text = values.name;
            controller.Description.text = "Damage: " + values.damage;
            newItem.transform.parent = ContentPanel.transform;
            newItem.transform.localScale = Vector3.one;
        }

    }



}