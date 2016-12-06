using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TranslationDatabase : ScriptableObject {

    public List<Translation> translations;

}

[System.Serializable]
public class Translation
{
    public string Name;
    public string Danish;
    public string English;
}