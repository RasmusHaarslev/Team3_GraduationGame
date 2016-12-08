using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TranslationManager : MonoBehaviour
{
    public Dictionary<string, string> LoadedTranlations = new Dictionary<string, string>();
    public bool English;

    #region Setup Instance
    private static TranslationManager _instance;

    public static TranslationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var manager = GameController.Instance.gameObject.AddComponent<TranslationManager>();
                manager.LoadLanguage();

                _instance = manager;
            }
            return _instance;
        }
    }
    #endregion

    public void LoadLanguage(bool isEnglish = true)
    {
        if (LoadedTranlations.Count > 0)
        {
            LoadedTranlations.Clear();
        }
        English = isEnglish;
        
        string line;

        // Read the file and display it line by line.
        TextAsset file = (TextAsset)Resources.Load("Translation/Translation", typeof(TextAsset));
        string text = file.text;

        System.IO.StringReader reader = new System.IO.StringReader(text);

        while ((line = reader.ReadLine()) != null)
        {
            ProcessLine(line);
        }
    }

    public string GetCurrentLanguage()
    {
        if (English)
        {
            return "English";
        } else
        {
            return "Danish";
        }
    }

    private void ProcessLine(string line)
    {
        var splitted = line.Split('\t');

        if (splitted.Length != 3)
        {
            Debug.LogError("Skipping translation, error in the following line (missing a tab?): " + line);
            return;
        }

        LoadedTranlations.Add(splitted[0], English ? splitted[1] : splitted[2]);
    }

    public string GetTranslation(string translationKey)
    {
        if (!LoadedTranlations.ContainsKey(translationKey))
        {
            Debug.LogError("MISSING " + translationKey);
            return "**" + translationKey + "**";
        }

        return LoadedTranlations[translationKey];
    }
}
