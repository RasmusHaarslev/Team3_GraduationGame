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

    public Text Amoumt;

    public void SetupText(GameObject node, string mode, int amount = 0)
    {
        Amoumt.text = amount+"";
        txtBtnYes.text = TranslationManager.Instance.GetTranslation("Yes");
        txtBtnNo.text = TranslationManager.Instance.GetTranslation("No");
    }
}
