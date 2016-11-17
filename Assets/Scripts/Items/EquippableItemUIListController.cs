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