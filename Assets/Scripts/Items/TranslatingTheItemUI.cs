using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TranslatingTheItemUI : MonoBehaviour {

    public Text txtHealth;
    public Text txtDamage;
    public Text txtAttackSpeed;
    public Text txtRange;
    
	void Start () {
        txtHealth.text = TranslationManager.Instance.GetTranslation("Health");
        txtDamage.text = TranslationManager.Instance.GetTranslation("Damage");
        txtAttackSpeed.text = TranslationManager.Instance.GetTranslation("AttackSpeed");
        txtRange.text = TranslationManager.Instance.GetTranslation("Range");        
    }
}
