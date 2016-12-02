using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmPanel : MonoBehaviour
{

    public GameObject btnYes;
    public GameObject btnNo;

    public Text txtBtnYes;
    public Text txtBtnNo;
    public Text txtHeader;

    public void SetupText(GameObject node, string mode, int amount = 0)
    {
        txtBtnYes.text = TranslationManager.Instance.GetTranslation("Yes");
        txtBtnNo.text = TranslationManager.Instance.GetTranslation("No");
        if (mode == "play")
        {
            if (amount > -1)
            {
                if (node.GetComponent<Node>() != null)
                {
                    txtHeader.text = TranslationManager.Instance.GetTranslation("ConfirmSure") + " " +
                                     node.GetComponent<Node>().TravelCost + " " +
                                     TranslationManager.Instance.GetTranslation("Food") + " " +
                                     TranslationManager.Instance.GetTranslation("OnPlaying");
                }
                else
                {
                    txtHeader.text = TranslationManager.Instance.GetTranslation("ConfirmSure") + " " +
                                     node.GetComponent<NodeTutorial>().TravelCost + " " +
                                     TranslationManager.Instance.GetTranslation("Food") + " " +
                                     TranslationManager.Instance.GetTranslation("OnPlaying");
                }
            }
            else
            {
                txtHeader.text = TranslationManager.Instance.GetTranslation("LoseVillager");            
            }
        }
        else if (mode == "scout")
        {
            if (node.GetComponent<Node>() != null)
            {
                txtHeader.text = TranslationManager.Instance.GetTranslation("ConfirmSure") + " " +
                             node.GetComponent<Node>().scoutCost + " " +
                             TranslationManager.Instance.GetTranslation("Food") + " " +
                             TranslationManager.Instance.GetTranslation("OnScouting");
            }
            else
            {
                txtHeader.text = TranslationManager.Instance.GetTranslation("ConfirmSure") + " " +
                                 TranslationManager.Instance.GetTranslation("Food") + " " +
                                 TranslationManager.Instance.GetTranslation("OnScouting");
            }
        }
        else if (mode == "flee")
        {
            txtHeader.text = TranslationManager.Instance.GetTranslation("ConfirmFlee");
        }
        else if (mode == "BuyTeeth")
        {
            txtHeader.text = TranslationManager.Instance.GetTranslation("BuyTeeth") + " " + amount + " " + TranslationManager.Instance.GetTranslation("GoldTeeths");
        }
    }
}
