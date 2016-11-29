using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GetFreeGold : MonoBehaviour
{
    public GameObject confirmPanel;

    public void GetGoldTeeth(int amount)
    {
        confirmPanel.SetActive(true);
        confirmPanel.GetComponent<ConfirmPanel>().SetupText(null, "BuyTeeth");
        confirmPanel.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmPanel.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.AddListener(delegate { BuyCurrency(amount); });
    }

    public void BuyCurrency(int teeths)
    {
        EventManager.Instance.TriggerEvent(new ChangeResources(premium: teeths));
    }
}