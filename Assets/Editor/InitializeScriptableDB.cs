using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

class InitializeScriptableDB
{
    [MenuItem("Assets/Scriptable Objects/Create ScriptableDBs")]
    static void InitializeDBs()
    {
        LevelsDatabase levelsDatabase = ScriptableObject.CreateInstance<LevelsDatabase>();
        AssetDatabase.CreateAsset(levelsDatabase, "Assets/Resources/ScriptableObjects/LevelsDatabase.asset");
        AssetDatabase.SaveAssets();

        UpgradesDatabase upgradesDatabase = ScriptableObject.CreateInstance<UpgradesDatabase>();
        AssetDatabase.CreateAsset(upgradesDatabase, "Assets/Resources/ScriptableObjects/UpgradesDatabase.asset");
        AssetDatabase.SaveAssets();

       
    }


    [MenuItem("Assets/Scriptable Objects/Create TranslationDB")]
    static void TranslationDB()
    {
        TranslationDatabase translationDatabase = ScriptableObject.CreateInstance<TranslationDatabase>();
        AssetDatabase.CreateAsset(translationDatabase, "Assets/Resources/ScriptableObjects/TranslationDatabase.asset");
        AssetDatabase.SaveAssets();
    }

}
