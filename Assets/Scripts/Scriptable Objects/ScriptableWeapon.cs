using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon", order = 1)]
public class ScriptableWeapon : ScriptableObject
{
    public string name { get; set; }
    public string type { get; set; }
    public slot Slot { get; set; }
    public int characterId { get; set; }
    public string rarity { get; set; }
    public string description { get; set; }
    public int damage { get; set; }
    public int health { get; set; }
    public int damageSpeed { get; set; }
    public int range { get; set; }
    public string prefabName { get; set; }

    public enum slot
    {
        head,
        torso,
        leftHand,
        rightHand,
        legs
    }

    public enum WeaponType
    {
        Shield = 0,
        Spear = 1,
        Rifle = 2
    }

    public string objectName = "New MyScriptableObject";
    public bool colorIsRandom = false;
    public Color thisColor = Color.white;
    public Vector3[] spawnPoints;
}
