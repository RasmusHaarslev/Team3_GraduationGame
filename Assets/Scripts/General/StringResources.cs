using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// This class gather all the main resources paths, to be used in other scripts.
/// </summary>
public class StringResources
{
    public static string databaseName = "gameDatabase.db";

    #region RESOURCES
    
        public static string NodeIdPrefsName = "NodeId";
        public static string LevelDifficultyPrefsName = "LevelDifficulty";
        public static string TribeCampsPrefsName = "TribeCamps";
        public static string FoodAmountPrefsName = "FoodAmount";
        public static string ScrapAmountPrefsName = "ScrapAmount";
        public static string ItemDropAmountPrefsName = "ItemDropAmount";
        public static string DaysSurvived = "DaysSurvived";

    #endregion

    #region PREFABS

    public static string uiPrefabsPath = "Prefabs/UI/";

    public static string charactersPrefabsPath = "Prefabs/Characters/";

    public static string equippableItemsPrefabsPath = "Prefabs/Items/EquippableItems/";

    public static string hardnessLevel = "LevelDifficulty";

    public static string playerPrefabName = "Player";

    public static string follower1PrefabName = "Follower";

    public static string follower2PrefabName = "Follower";

    public static string tribesman1PrefabName = "Rival";

    public static string rifle1PrefabName = "Rifle";

    public static string polearm1PrefabName = "Stick";

    public static string shield1PrefabName = "Shield";
    #endregion



    #region MATERIALS

    public static string charactersMaterialsPath = "Materials/Characters/";

    public static string itemsMaterialsPath = "Materials/Items/";

    public static string playerMaterialName = "LeaderMaterial";

    public static string follower1MaterialName = "Follower1Material";

    public static string follower2MaterialName = "Follower2Material";


    public static Dictionary<EquippableitemValues.type, string[][]> equipItemsModelsStrings = // shield -> ["shield name","prefab name","material name"]
        new Dictionary<EquippableitemValues.type, string[][]>
        {
            {
                EquippableitemValues.type.shield, new[]
                {
                    new[] { "Riot Shield", "Shield1", "Shield1AMaterial" },
                    new[] { "Rusted Barrier", "Shield1", "Shield1BMaterial" },
                    new[] { "Moss-covered Plate", "Shield1", "Shield1CMaterial" },
                    new[] { "Stop Sign", "Shield2", "Shield2AMaterial" }            
                }
            },
            {
                EquippableitemValues.type.polearm, new[]
                {
                    new[] { "Stick Blade", "Polearm1", "Polearm1AMaterial" },
                    new[] { "Army Pike", "Polearm1", "Polearm1BMaterial" },
                    new[] { "Ranged Cutter", "Polearm1", "Polearm1CMaterial" }
                }
            },
            {
                EquippableitemValues.type.rifle, new[]
                {
                    new[] { "Hunting Rifle", "Rifle1", "Rifle1AMaterial" },
                    new[] { "Carabine", "Rifle1", "Rifle1BMaterial" },
                    new[] { "Old Musket", "Rifle1", "Rifle1CMaterial" }
                }
            }
        };
    
    

    public static string[] maleHuntersMaterials = new[] { "Hunter_Male_1", "Hunter_Male_2", "Hunter_Male_3", "Hunter_Male_4", "Hunter_Male_5", "Hunter_Male_6", "Hunter_Male_7", "Hunter_Male_8", "Hunter_Male_9" };

    public static string[] femaleHuntersMaterials = new[] { "Hunter_Female_1", "Hunter_Female_2", "Hunter_Female_3", "Hunter_Female_4", "Hunter_Female_5", "Hunter_Female_6", "Hunter_Female_7", "Hunter_Female_8", "Hunter_Female_9" };


    #endregion



    #region ANIMATIONS

    public static string animControllerShieldName = "Humanoid_ShieldController";

    public static string animControllerRifleName = "Humanoid_RifleController";

    public static string animControllerSpearName = "Humanoid_SpearController";

    #endregion

    #region CharacterNames
    public static string[] maleNames = new[] { "THOMAS",
"NILS",
"KRISTIAN",
"CHRISTIAN",
"SEBASTIAN",
"VESELIN",
"DANIEL",
"JOHN",
"NICOLAI",
"REINHARDT",
"PETER",
"RASMUS",
"RICCARDO",
"TOBIAS",
"LUDVIG",
"BENJAMIN",
"MADS",
"HASSAN",
"MARK",
"MATHIAS",
"ANDRZEJ",
"BESNIK",
"LUCA",
"YOSKA",
"TOBAR",
"KASPER",
"JOACHIM",
"LARS",
"OTTO",
"OSCAR",
"MUHAMMED",
"MALIK",
"MORTEN",
"RIKARD",
"ALEKSY",
"KASIMIERZ",
"KONSTANTY",
"KORNELIUSZ",
"KRYSZTOF",
"AHMED",
"BAHADIR",
"BASAK",
"CHENGIZ",
"DENIZ",
"ERDEM",
"IBRAHIM",
"KHALID",
"SEBNEM",
"SULEIMAN",
"ZEHEB"};

    public static string[] femaleNames = new[] { "TEA",
"AGNES",
"ANDREA",
"YASMIN",
"SHAHIDA",
"MIRA",
"SELMA",
"AISHA",
"BESS",
"EMMA",
"IDA",
"LOUISE",
"CAMILLA",
"SUSI",
"DIKA",
"DONKA",
"ESMERALDA",
"LULUDJA",
"NADYA",
"AIDA",
"HABIBAH",
"ELIZA",
"ESHAL",
"INAAYA",
"CÆCILIA",
"URSULA",
"EMILY",
"ANNA",
"OLGA",
"KAREN",
"BENTE",
"BANAAN",
"MAYA",
"AGATA",
"ALESKA",
"EMILJA",
"ESTERA",
"OLESIA",
"ARUBA",
"DILARA",
"FATIMAH",
"FATMA",
"JAMILIA",
"LAMYA",
"MELEK",
"PINAR",
"SHAKELA",
"YAZ",
"ZULMA"};


    #endregion

     
}
