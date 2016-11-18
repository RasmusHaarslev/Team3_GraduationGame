using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

class BuildScript
{
    [MenuItem("Build/Android")]
    static void PerformBuild()
    {
        List<string> scenes = new List<string>();
        addScenes(scenes);

        //string[] scenes = { "Assets/_Scenes/CampManagement.unity", "Assets/_Scenes/LevelSelection.unity", "Assets/_Scenes/Levels/LevelPrototype.unity"};

        // Todo
        string buildPath = "C:/GoogleDrive/DADIU2016T3/Builds/";
        string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + "_build.apk";

        // Create build folder if not yet exists
        var dir = Directory.CreateDirectory(buildPath);

        BuildPipeline.BuildPlayer(scenes.ToArray(), buildPath+fileName, BuildTarget.Android, BuildOptions.None);
    }

    static void addScenes(List<string> scenes)
    {
        scenes.Add("Assets/_Scenes/IntroCutscene");
        scenes.Add("Assets/_Scenes/LevelEnterCutscene");
        scenes.Add("Assets/_Scenes/LevelFleeCutscene");
        scenes.Add("Assets/_Scenes/LevelWinCutscene");
        scenes.Add("Assets/_Scenes/AllyDeathCutscene");
        scenes.Add("Assets/_Scenes/PlayerDeathCutscene");

        var sceneDirectory = Directory.CreateDirectory("Assets/_Scenes/");
        foreach (var scene in sceneDirectory.GetFiles())
        {
            if (scene.Name.EndsWith(".unity"))
            {
                scenes.Add("Assets/_Scenes/" + scene.Name);
            }
        }

        sceneDirectory = Directory.CreateDirectory("Assets/_Scenes/Tutorial");
        foreach (var scene in sceneDirectory.GetFiles())
        {
            if (scene.Name.EndsWith(".unity"))
            {
                scenes.Add("Assets/_Scenes/Tutorial/" + scene.Name);
            }
        }

        sceneDirectory = Directory.CreateDirectory("Assets/_Scenes/Levels");
        foreach (var scene in sceneDirectory.GetFiles())
        {
            if (scene.Name.EndsWith(".unity"))
            {
                scenes.Add("Assets/_Scenes/Levels/" + scene.Name);
            }
        }
    }

    
}
