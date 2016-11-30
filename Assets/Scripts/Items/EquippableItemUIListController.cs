using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class EquippableItemUIListController : MonoBehaviour
{
    
    public GameObject ContentPanel;
    private GameObject ListItemPrefab;
    //public GameObject cameraWeapons;
    bool firstDrawn = false;
   public void GenerateItemsList(IEnumerable<EquippableitemValues> itemsValues)
    {
        ListItemPrefab = Resources.Load(StringResources.uiPrefabsPath + "EquippableItemUIScrollElement") as GameObject;
        //removing previous list if present
        foreach (Transform child in ContentPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //adding the new list
        foreach (EquippableitemValues values in itemsValues)
        {
            
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            UIListEquippableItemController controller = newItem.GetComponent<UIListEquippableItemController>();
            if (!firstDrawn)
            {
                newItem.GetComponent<Image>().color = controller.selectedColor;
                firstDrawn = true;
            }
            //controller.Icon.sprite = animal.Icon;
            controller.type.text = "Type: " + values.Type.ToString();
            controller.level.text = "Level: " + values.level.ToString();
            controller.name.text = values.name;
            controller.damage.text = "Damage: " + values.damage;
            controller.damageSpeed.text = "Atk Speed: " + values.damageSpeed;
            controller.health.text = "Health: " + values.health;
            controller.range.text = "Range: " + values.range;
            controller.itemValues = values;
            
            Debug.Log(controller.weaponCams.transform.GetChild(2).gameObject.GetComponent<Camera>().targetTexture);
            Debug.Log(controller.Icon.texture);
            if (values.Type.ToString() == "rifle")
            {
                controller.Icon.texture = controller.weaponCams.transform.GetChild(0).gameObject.GetComponent<Camera>().targetTexture;
            }
            if (values.Type.ToString() == "shield")
            {
                controller.Icon.texture = controller.weaponCams.transform.GetChild(1).gameObject.GetComponent<Camera>().targetTexture;
            }
            if (values.Type.ToString() == "polearm")
            {
                controller.Icon.texture = controller.weaponCams.transform.GetChild(2).gameObject.GetComponent<Camera>().targetTexture;
            }

            newItem.transform.SetParent(ContentPanel.transform);
            newItem.transform.localScale = Vector3.one;
            newItem.transform.localPosition = Vector3.zero;
            newItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        firstDrawn = false;
        //GetComponentInChildren<UIListEquippableItemController>().GetComponent<Image>().color = new Color32(0, 85, 250, 116);
        //print(GetComponentInChildren<UIListEquippableItemController>().GetComponentInChildren<Text>().text);
    }


}