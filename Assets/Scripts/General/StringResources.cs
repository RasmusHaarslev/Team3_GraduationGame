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

    #region PREFABS

    public static string uiPrefabsPath = "Prefabs/UI/";

    public static string charactersPrefabsPath = "Prefabs/Characters/";

    public static string equippableItemsPrefabsPath = "Prefabs/Items/EquippableItems/";

    public static string hardnessLevel = "LevelDifficulty";

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

    public static string follower1MaterialName = "Follower1Material";

    public static string follower2MaterialName = "Follower2Material";

    public static string rifle1MaterialName = "Rifle1Material";

    public static string polearm1MaterialName = "Polearm1Material";

    public static string shield1MaterialName = "Shield1Material";

    #endregion


    #region ANIMATIONS

    public static string animControllerShieldName = "Humanoid_Shield";

    public static string animControllerRifleName = "Humanoid_Riffle";

    public static string animControllerSpearName = "Humanoid_Spear";

    #endregion



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
