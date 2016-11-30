using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class RecruitementController
{
    public void SaveRecruitementSoldiers(List<GameObject> soldiers, List<GameObject> weapons)
    {
        RecruitementXML xml = new RecruitementXML();

        foreach (var soldier in soldiers)
        {
            var charXML = new CharacterXML();
            var soldierValues = soldier.GetComponent<CharacterValues>();

            charXML.damage = soldierValues.damage;
            charXML.damageSpeed = soldierValues.damageSpeed;
            charXML.description = soldierValues.description;
            charXML.health = soldierValues.health;
            charXML.isMale = soldierValues.isMale;
            charXML.materialName = soldierValues.materialName;
            charXML.name = soldierValues.name;
            charXML.prefabName = soldierValues.prefabName;
            charXML.range = soldierValues.range;
            charXML.tier = soldierValues.tier;


            // do something for type and both traits
            

            xml.Characters.Add(charXML);
        }

        foreach (var weapon in weapons)
        {
            var eqXML = new ItemXML();
            var equipment = weapon.GetComponent<EquippableitemValues>();

            eqXML.damage = equipment.damage;
            eqXML.damageSpeed  = equipment.damageSpeed;
            eqXML.description = equipment.description;
            eqXML.health = equipment.health;
            eqXML.level = equipment.level;
            eqXML.materialName = equipment.materialName;
            eqXML.name = equipment.name;
            eqXML.prefabName = equipment.prefabName;
            eqXML.range = equipment.range;
            eqXML.rarity = equipment.rarity;

            // do something for type and slot


            xml.Equipment.Add(eqXML);
        }

        var path = Path.Combine(PersistentData.GetPath(), "recruitement.xml");

        var serializer = new XmlSerializer(typeof(ItemsXML));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, xml);
        stream.Close();
    }
}

public class RecruitementXML
{
    [XmlArray("Characters")]
    [XmlArrayItem("Character")]
    public List<CharacterXML> Characters = new List<CharacterXML>();

    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<ItemXML> Equipment = new List<ItemXML>();
}

