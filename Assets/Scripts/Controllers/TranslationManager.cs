using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TranslationManager : MonoBehaviour
{
    public Dictionary<string, string> LoadedTranlations = new Dictionary<string, string>();
    private bool English;

    #region Setup Instance
    private static TranslationManager _instance;

    public static TranslationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Translator");
                var manager = go.AddComponent<TranslationManager>();
                manager.LoadLanguage();

                _instance = manager;
            }
            return _instance;
        }
    }
    #endregion

    private void LoadLanguage(bool isEnglish = false)
    {
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

    private void ProcessLine(string line)
    {
        var splitted = line.Split('\t');

        LoadedTranlations.Add(splitted[0], English ? splitted[1] : splitted[2]);
    }

    public string GetTranslation(string tranlationKey)
    {
        return LoadedTranlations[tranlationKey];
    }
}
