using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TranslationHelper : MonoBehaviour
{
    [Tooltip("Supply the translation key here")]
    public string TranslationKey;

    void Start()
    {
        this.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation(TranslationKey);
    }
}
