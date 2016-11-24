using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmPanel : MonoBehaviour {

    public GameObject btnYes;
    public GameObject btnNo;

    public Text txtBtnYes;
    public Text txtBtnNo;

    public Text txtPlay;
    public Text txtScout;    

    public void SetupText(GameObject node, string mode)
    {
        txtBtnYes.text = TranslationManager.Instance.GetTranslation("Yes");
        txtBtnNo.text = TranslationManager.Instance.GetTranslation("No");
        if (mode == "play")
        {
            txtPlay.text = TranslationManager.Instance.GetTranslation("ConfirmSure") + " " + node.GetComponent<Node>().TravelCost + " " + TranslationManager.Instance.GetTranslation("OnPlaying");
        } else if (mode == "scout")
        {
            txtScout.text = TranslationManager.Instance.GetTranslation("ConfirmSure") + " " + node.GetComponent<Node>().scoutCost + " " + TranslationManager.Instance.GetTranslation("OnScouting");
        }
    }
}
