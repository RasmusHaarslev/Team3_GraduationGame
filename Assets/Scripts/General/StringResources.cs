using UnityEngine;
using System.Collections;
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

    public static string rifle1MaterialName = "Rifle1Material";

    public static string polearm1MaterialName = "Polearm1Material";

    public static string shield1MaterialName = "Shield1Material";

    public static string[] maleHuntersMaterials = new[] { "MaleHunterMaterial1", "MaleHunterMaterial2", "MaleHunterMaterial3", "MaleHunterMaterial4", "MaleHunterMaterial5" };

    public static string[] femaleHuntersMaterials = new[] { "FemaleHunterMaterial1", "FemaleHunterMaterial2", "FemaleHunterMaterial3", "FemaleHunterMaterial4", "FemaleHunterMaterial5" };
    

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
"TOBAR"};

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
"INAAYA"};


    #endregion

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
