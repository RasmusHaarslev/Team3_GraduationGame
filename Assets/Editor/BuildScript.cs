using System;
using System.IO;
using UnityEditor;

//Sorry; this was bugging out, not allowing us to run anything. -Nils
namespace Assets.Editor
{
    class BuildScript
    {
        [MenuItem("Build/Android")]
        static void PerformBuild()
        {
            string[] scenes = { "Assets/_Scenes/Levels/LevelPrototype.unity"};

            // Todo
            string buildPath = "C:/GoogleDrive/DADIU2016T3/Builds/";
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + "_build.apk";

            // Create build folder if not yet exists
            var dir = Directory.CreateDirectory(buildPath);

            BuildPipeline.BuildPlayer(scenes, buildPath+fileName, BuildTarget.Android, BuildOptions.None);
        
        }
    }
}
