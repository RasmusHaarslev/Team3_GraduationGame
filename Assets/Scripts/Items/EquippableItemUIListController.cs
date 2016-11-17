using UnityEngine;
using System.Collections;

public class EquippableItemUIListController : MonoBehaviour
{

    public Sprite[] AnimalImages;
    public GameObject ContentPanel;
    private GameObject ListItemPrefab;

    public EquippableitemValues[] itemsValues;

    void Start()
    {
        ListItemPrefab = Resources.Load(StringResources.uiPrefabsPath + "EquippableItemUIScrollElement") as GameObject;
        // 1. Get the data to be displayed
        //itemsValues = new[] {
        //      new EquippableitemValues
        // {
        //     id = 1,
        //     name = "Toothpick",
        //     Type = EquippableitemValues.type.polearm,
        //     Slot = EquippableitemValues.slot.rightHand,
        //     health = 20,
        //     damage = 10,
        //     damageSpeed = 9,
        //     range = 5,
        //     characterId = 1,
        //     prefabName = "Stick"
        // },
        //     new EquippableitemValues
        // {
        //     id = 2,
        //     name = "Plastic Shield",
        //     Type = EquippableitemValues.type.shield,
        //     Slot = EquippableitemValues.slot.rightHand,
        //     health = 20,
        //     damage = 10,
        //     damageSpeed = 9,
        //     range = 5,
        //     characterId = 2,
        //     prefabName = "Shield"
        // },
        //     new EquippableitemValues
        // {
        //     id = 3,
        //     name = "Laser Rifle 2000",
        //     Type = EquippableitemValues.type.rifle,
        //     Slot = EquippableitemValues.slot.rightHand,
        //     health = 20,
        //     damage = 10,
        //     damageSpeed = 9,
        //     range = 20,
        //     characterId = 3,
        //     prefabName = "Rifle"
        // },
        //    new EquippableitemValues
        //{
        //    id = 4,
        //    name = "Rifle of the Git Master Rasmus",
        //    Type = EquippableitemValues.type.rifle,
        //    Slot = EquippableitemValues.slot.rightHand,
        //    health = 25,
        //    damage = 15,
        //    damageSpeed = 15,
        //    range = 15,
        //    prefabName = "Rifle"
        //},
        //     new EquippableitemValues
        //     {
        //         id = 5,
        //         name = "Mighty power Stick",
        //         Type = EquippableitemValues.type.polearm,
        //         Slot = EquippableitemValues.slot.rightHand,
        //         health = 20,
        //         damage = 20,
        //         damageSpeed = 9,
        //         range = 5,
        //         prefabName = "Stick"
        //     },
        //new EquippableitemValues
        //{
        //    id = 6,
        //    name = "Romanian Steel Bar",
        //    Type = EquippableitemValues.type.polearm,
        //    Slot = EquippableitemValues.slot.rightHand,
        //    health = 20,
        //    damage = 25,
        //    damageSpeed = 9,
        //    range = 2,
        //    prefabName = "Stick"
        //}
        //};

        // 2. Iterate through the data, 
        //	  instantiate prefab, 
        //	  set the data, 
        //	  add it to panel
        foreach (EquippableitemValues values in itemsValues)
        {
            GameObject newItem = Instantiate( ListItemPrefab) as GameObject;
            UIListEquippableItemController controller = newItem.GetComponent<UIListEquippableItemController>();
            //controller.Icon.sprite = animal.Icon;
            controller.name.text = values.name;
            controller.damage.text = "Damage: " + values.damage;
            controller.damageSpeed.text = "Damage Speed: " + values.damageSpeed;
            controller.health.text = "Health: " + values.health;
            controller.range.text = "Range: " + values.range;
            controller.itemValues = values;
            newItem.transform.SetParent(ContentPanel.transform);
            newItem.transform.localScale = Vector3.one;
            newItem.transform.localPosition = Vector3.zero;
            newItem.transform.localRotation = Quaternion.Euler(0,0,0);
        }

    }



}