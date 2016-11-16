using UnityEngine;
using System.Collections;

public class EquippableItemListController : MonoBehaviour
{

    public Sprite[] AnimalImages;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

    ArrayList itemsValues;

    void Start()
    {
        ListItemPrefab = Resources.Load(StringResources.uiPrefabsPath + "EquippableItemUIScrollElement") as GameObject;
        // 1. Get the data to be displayed
        itemsValues = 

        // 2. Iterate through the data, 
        //	  instantiate prefab, 
        //	  set the data, 
        //	  add it to panel
        foreach (EquippableitemValues values in itemsValues)
        {
            GameObject newAnimal = Instantiate( ListItemPrefab) as GameObject;
            ListItemController controller = newAnimal.GetComponent();
            controller.Icon.sprite = animal.Icon;
            controller.Name.text = animal.Name;
            controller.Description.text = animal.Description;
            newAnimal.transform.parent = ContentPanel.transform;
            newAnimal.transform.localScale = Vector3.one;
        }
    }
}