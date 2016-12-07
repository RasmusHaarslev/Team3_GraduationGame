using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmPanel : MonoBehaviour
{
    public GameObject btnYes;
    public GameObject btnNo;

    public GameObject btnYesVillage;
    public GameObject btnNoVillage;

    public Text txtBtnYes;
    public Text txtBtnNo;
    public Text txtBtnYesVillage;
    public Text txtBtnNoVillage;
    public Text txtHeader;

    public Text Amoumt;

    public void SetupText(GameObject node, string mode, int amount = 0)
    {
        Amoumt.text = amount+"";
        txtBtnYes.text = TranslationManager.Instance.GetTranslation("Yes");
        txtBtnNo.text = TranslationManager.Instance.GetTranslation("No");

        if (txtBtnYesVillage != null) { 
            txtBtnYesVillage.text = TranslationManager.Instance.GetTranslation("Yes");
        }

        if (txtBtnNoVillage != null ) { 
            txtBtnNoVillage.text = TranslationManager.Instance.GetTranslation("No");
        }

    }
}
