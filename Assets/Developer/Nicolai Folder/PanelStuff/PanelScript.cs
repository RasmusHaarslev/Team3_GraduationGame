using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using UnityEngine.Networking;

public class PanelScript : MonoBehaviour {

    public List<GameObject> panelList = new List<GameObject>();
    public List<GameObject> soldierStatsList = new List<GameObject>();
    List<GameObject> soldiersList = new List<GameObject>();
    private DataService dataService;
    public Transform solidersSpawnPosition;
    GameObject charactersFellowship;
    public List<Camera> soldierCameraList = new List<Camera>();
    public List<Light> SpotLightList = new List<Light>();
    Character currentSoldier;
    public List<NewSoldierList> newSoldierStatsList;
    List<CharacterValues> newCharacterSoldierList = new List<CharacterValues>();
    public int newCharPoints = 15;
    [Range (0f, 1f)]
    public float damagePointsChance = 0.5f;
    public Transform camsAndNewSoldiersPosition;
    public GameObject silhouette;
    public bool alreadyGeneratedNewSoldiers = false;
    List<GameObject> newSoldiersList = new List<GameObject>();
    public GameObject silhouetteGO;
    public List<Transform> silhouettePosList = new List<Transform>();
    [Serializable]
    public class NewSoldierList : IEnumerable<GameObject>
    {
        public List<GameObject> newIndividualSoldierStatsList;

        #region Implementation of IEnumerable
        public IEnumerator<GameObject> GetEnumerator()
        {
            return newIndividualSoldierStatsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }

    void Start()
    {
        dataService = new DataService(StringResources.databaseName);
        dataService.CreateDB();
        charactersFellowship = dataService.GetPlayerFellowshipInPosition(solidersSpawnPosition);
        InitializeSoldiers();
    }

    public void ActivateNewSoldiers(Transform silhouetteTrans)
    {
        //foreach(Transform silTrans in silhouettePosList)
        //{
        //    print(silTrans.position);
        //    if (silhouetteGO.transform.position == silTrans.position)
        //    {
        //        print(silhouetteGO.transform.position);


        //        for (int i = silhouettePosList.IndexOf(silTrans) * 3; i < silhouettePosList.IndexOf(silTrans) * 3 + 3; i++)
        //        {

        //            if (newSoldiersList[i].activeSelf == false)
        //            {
        //                newSoldiersList[i].SetActive(true);
        //            }

        //        }

        //    }
        //}
        foreach (GameObject soldier in newSoldiersList)
        {
            if (soldier.activeSelf == false)
            {
                soldier.SetActive(true);
            }
        }
        /////
        foreach (Transform trans in camsAndNewSoldiersPosition)
        {
            if (trans.gameObject.GetComponentInChildren<Camera>().isActiveAndEnabled == false && trans.gameObject.GetComponentInChildren<Light>().isActiveAndEnabled == false)
            {
                trans.gameObject.GetComponentInChildren<Camera>().enabled = true;
                trans.gameObject.GetComponentInChildren<Light>().enabled = true;
            }
        }
    }

    public void DeactivateNewSoldiers()
    {
        foreach(GameObject soldier in newSoldiersList)
        {
            if(soldier.activeSelf == true)
            {
                soldier.SetActive(false);
            }
        }

        foreach(Transform trans in camsAndNewSoldiersPosition)
        {
            if (trans.gameObject.GetComponentInChildren<Camera>().isActiveAndEnabled == true && trans.gameObject.GetComponentInChildren<Light>().isActiveAndEnabled == true)
            {
                trans.gameObject.GetComponentInChildren<Camera>().enabled = false;
                trans.gameObject.GetComponentInChildren<Light>().enabled = false;
            }
        }
    }

    public void SpawnNewSoldier(int index)
    {
        for(int i = 0; i < newSoldiersList.Count; i++)
        {
            if(index == i)
            {
                foreach (Transform soldiertrans in solidersSpawnPosition)
                {
                    if (silhouetteGO.transform.localPosition - Vector3.up == soldiertrans.localPosition)
                    {

                        //soldier.animator.SetInteger("IdleAction", 8);

                        //print("adding as new Character");
                        //newSoldiersList[i].GetComponent<Character>().characterBaseValues.id = dataService.AddcharacterToDbByValues(newSoldiersList[i].GetComponent<Character>().characterBaseValues);
                        //print("id of the new character " + newSoldiersList[i].GetComponent<Character>().characterBaseValues.id);
                        CharacterValues valuesToKeep = newSoldiersList[i].GetComponent<Character>().characterBaseValues;
                        valuesToKeep.Type = CharacterValues.type.Hunter;
                        dataService.UpdateCharacterValuesInDb(valuesToKeep);

                        GameObject newWeapon = dataService.GenerateNewEquippableItemFromValues(newSoldiersList[i].GetComponentInChildren<EquippableItem>().itemValues);
                        print("id of the weapon "+newWeapon.GetComponent<EquippableItem>().itemValues.id );
                        //destroy current puppet weapon
                        Destroy(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject);
                        //equip weapon
                        print(newSoldiersList[i].GetComponent<Character>().characterBaseValues.name);
                        
                        dataService.equipItemsToCharacter(new[] { newWeapon },newSoldiersList[i].GetComponent<Character>());
                        
                        newSoldiersList[i].transform.localPosition = soldiertrans.localPosition;
                        newSoldiersList[i].transform.localRotation = soldiertrans.localRotation;
                        newSoldiersList[i].AddComponent<PanelController>();
                        SetCampAnimation(newSoldiersList[i].GetComponent<Character>());
                        if (soldiertrans.localPosition == solidersSpawnPosition.GetChild(1).localPosition)
                        {
                            newSoldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
                        }
                        if (soldiertrans.localPosition == solidersSpawnPosition.GetChild(2).localPosition)
                        {
                            newSoldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter2");
                        }
                        if (soldiertrans.localPosition == solidersSpawnPosition.GetChild(3).localPosition)
                        {
                            newSoldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter3");
                        }
                        EventManager.Instance.TriggerEvent(new ChangeResources(villager: -1));
                    }
                }
                
              
                
            }
            else
            {
                Destroy(newSoldiersList[i]);
                //TODO continue from here
                dataService.DeleteCharactersValuesFromDb(newSoldiersList[i].GetComponent<Character>().characterBaseValues);
            }
        }
        newSoldiersList.Clear();
        alreadyGeneratedNewSoldiers = false;
        Destroy(silhouetteGO);
    }

    public void RerollSoldiers()
    {
        foreach (GameObject soldier in newSoldiersList)
        {     
            Destroy(soldier);
        }
        newSoldiersList.Clear();
        
        GetNewSoldiers();

    }

    public void GetNewSoldiers()
    {

        
        CharacterValues[] newHuntersValues = dataService.GetNewHuntersValues();
        print(newHuntersValues.Length);
        //check if there are already new hunters into database
        if (newHuntersValues.Length < 3)
        { 
            //add one new hunter
            foreach (Transform trans in camsAndNewSoldiersPosition)
            {

                newSoldiersList.Add(GenerateNewHunter(trans));
            }

        }
        else {
            newCharacterSoldierList = newHuntersValues.ToList<CharacterValues>();
            int i = 0;
            foreach (Transform trans in camsAndNewSoldiersPosition)
            { //if we have 3 new hunters values, generate them and add to the list
                
                newSoldiersList.Add(dataService.GenerateCharacterFromValues(newHuntersValues[i], trans.position));
                i++;
            }
        }

        foreach (GameObject newSoldier in newSoldiersList)
        {
            if (newSoldier.GetComponent<NavMeshAgent>().enabled == true && newSoldier.GetComponent<HunterStateMachine>().enabled == true)
            { 
                newSoldier.GetComponent<NavMeshAgent>().enabled = false;
                newSoldier.GetComponent<HunterStateMachine>().enabled = false;
            }
        }   
        
        //newSoldiersList[0].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
        //newSoldiersList[1].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter2");
        //newSoldiersList[2].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter3");

        FillInNewSoldierStats();
    
    }



    void InitializeSoldiers()
    {
        int[] layersIndices = new[] { 9, 10, 11, 12 };


        for(int i = 0; i < charactersFellowship.transform.childCount; i++)
        {
            soldiersList.Add(charactersFellowship.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < soldiersList.Count; i++)      
        {
            
            soldiersList[i].AddComponent<PanelController>();
            soldiersList[i].GetComponent<NavMeshAgent>().enabled = false;     

            
            if(soldiersList[i].GetComponent<Character>().characterBaseValues.Type == CharacterValues.type.Player)
            {
                soldiersList[i].GetComponent<MoveScript>().enabled = false;
                soldiersList[i].GetComponent<Formation>().enabled = false;
            }
            else
            {
                soldiersList[i].GetComponent<HunterStateMachine>().enabled = false;              
            }

            soldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = layersIndices[i];
            
            // Switches the animator out with the camp animator.
            SetCampAnimation(soldiersList[i].GetComponent<Character>());
        }
      
        if(solidersSpawnPosition.childCount != soldiersList.Count)
        {
            for (int i = solidersSpawnPosition.childCount - 1; i > soldiersList.Count - 1; i--)
            {
                Instantiate(silhouette, solidersSpawnPosition.GetChild(i).position + Vector3.up, Quaternion.identity);
                //silhouetteList.Add(silhouette);
            }
        }

    }

    private void SetCampAnimation(Character soldier)
    {
        // Switches the animator out with the camp animator.
        RuntimeAnimatorController newController = (RuntimeAnimatorController)Resources.Load("CampIdleControllers/CampIdleControllerUpdated");
        soldier.GetComponent<Animator>().runtimeAnimatorController = newController;
        
        if (soldier.transform.position == solidersSpawnPosition.GetChild(0).position) {
            switch (soldier.equippedWeaponType) {
                case EquippableitemValues.type.polearm:
                    soldier.animator.Play("StandingIdleSpear1");

                    Vector3 newPosition1 = new Vector3(0.034f, -0.061f, -0.092f);
                    Quaternion newRotation1 = Quaternion.Euler(88.096f, -230.06f, -55.345f);

                    if (soldier.GetComponentsInChildren<EquippableItem>().Length > 1) {
                        soldier.GetComponentsInChildren<EquippableItem>()[1].transform.localPosition = newPosition1;
                        soldier.GetComponentsInChildren<EquippableItem>()[1].transform.localRotation = newRotation1;
                    }
                    else {
                        soldier.GetComponentInChildren<EquippableItem>().transform.localPosition = newPosition1;
                        soldier.GetComponentInChildren<EquippableItem>().transform.localRotation = newRotation1;
                    }
                    break;
                case EquippableitemValues.type.rifle:
                    soldier.animator.Play("StandingIdleRifle1");

                    Vector3 newPosition2 = new Vector3(0.019f, -0.041f, -0.022f);
                    Quaternion newRotation2 = Quaternion.Euler(6.115f, 168.97f, -33.529f);

                    if (soldier.GetComponentsInChildren<EquippableItem>().Length > 1) {
                        soldier.GetComponentsInChildren<EquippableItem>()[1].transform.localPosition = newPosition2;
                        soldier.GetComponentsInChildren<EquippableItem>()[1].transform.localRotation = newRotation2;
                    }
                    else {
                        soldier.GetComponentInChildren<EquippableItem>().transform.localPosition = newPosition2;
                        soldier.GetComponentInChildren<EquippableItem>().transform.localRotation = newRotation2;
                    }
                    break;
                case EquippableitemValues.type.shield:
                    soldier.animator.Play("StandingIdleShield1");
                    break;
            }
        }
        else if (soldier.transform.position == solidersSpawnPosition.GetChild(1).position) {
            switch (soldier.equippedWeaponType) {
                case EquippableitemValues.type.polearm:
                    soldier.animator.Play("SpearBoxIdle");
                    break;
                case EquippableitemValues.type.rifle:
                    soldier.animator.Play("RifleBoxIdle");
                    break;
                case EquippableitemValues.type.shield:
                    soldier.animator.Play("ShieldBoxIdle");
                    break;
            }
        }
        else if (soldier.transform.position == solidersSpawnPosition.GetChild(2).position) {
            switch (soldier.equippedWeaponType) {
                case EquippableitemValues.type.polearm:
                    soldier.animator.Play("SpearSquatIdle");
                    break;
                case EquippableitemValues.type.rifle:
                    soldier.animator.Play("RifleSquatIdle");
                    break;
                case EquippableitemValues.type.shield:
                    soldier.animator.Play("ShieldSquatIdle");
                    break;
            }
        }
        else if (soldier.transform.position == solidersSpawnPosition.GetChild(3).position)
        {
            switch (soldier.equippedWeaponType) {
                case EquippableitemValues.type.polearm:
                    soldier.animator.Play("StandingIdleSpear2");
                    break;
                case EquippableitemValues.type.rifle:
                    soldier.animator.Play("StandingIdleRifle2");
                    break;
                case EquippableitemValues.type.shield:
                    soldier.animator.Play("StandingIdleShield2");
                    break;
            }
        }
    }

    #region Cameras
    public void ActivateCamera(GameObject soldier)
    {
        
        if(soldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer == 9)
        {
            soldierCameraList[0].enabled = true;
            SpotLightList[0].enabled = true;
            DeactivateCamera(0);
        }
        if (soldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer == 10)
        {
            soldierCameraList[1].enabled = true;
            SpotLightList[1].enabled = true;
            DeactivateCamera(1);
        }
        if (soldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer == 11)
        {
            soldierCameraList[2].enabled = true;
            SpotLightList[2].enabled = true;
            DeactivateCamera(2);
        }
        if (soldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer == 12)
        {
            soldierCameraList[3].enabled = true;
            SpotLightList[3].enabled = true;
            DeactivateCamera(3);
        }
    }

    public void DeactivateCamera(int cameraIndex)
    {
        for (int i = 0; i < charactersFellowship.transform.childCount ; i++)
        {
            if(i != cameraIndex)
            {
                soldierCameraList[i].enabled = false;
               
            }   
        }     
    }

    public void DeactivateSpotligths()
    {
       foreach(Light spotlight in SpotLightList)
        {
            if(spotlight.enabled == true)
            {
                spotlight.enabled = false;
            }
        }
    }
    #endregion

    void FillInNewSoldierStats()
    {
        print(newCharacterSoldierList.Count);
        int i = 0;
        foreach (var newSoldierStats in newSoldierStatsList)
        {
            
            foreach (var stat in newSoldierStats)
            {

                if (stat.name == "Damage")
                {
                    stat.GetComponent<Text>().text = "Damage: " + newCharacterSoldierList[i].damage.ToString();
                }
                if (stat.name == "Soldier Name")
                {
                    stat.GetComponent<Text>().text =  newCharacterSoldierList[i].name;
                }               
                if (stat.name == "Health")
                {
                    stat.GetComponent<Text>().text = "Health: " + newCharacterSoldierList[i].health.ToString();
                }
                if (stat.name == "Damage Speed")
                {
                    stat.GetComponent<Text>().text = "Attack speed: " + newCharacterSoldierList[i].damageSpeed.ToString();
                }
                if (stat.name == "Range")
                {
                    stat.GetComponent<Text>().text = "Range: " + newCharacterSoldierList[i].range.ToString();
                }
                if (stat.name == "Combat Trait")
                {
                    stat.GetComponent<Text>().text = " " + Regex.Replace(newCharacterSoldierList[i].combatTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2") + ":\n" + GetComponent<TraitDescription>().chooseCombatTraitDescription(newCharacterSoldierList[i]);
                }
                if (stat.name == "Target Trait")
                {
					stat.GetComponent<Text>().text = " " + Regex.Replace(newCharacterSoldierList[i].targetTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2") + ":\n" + GetComponent<TraitDescription>().chooseTargetTraitDescription(newCharacterSoldierList[i]);
                }

            }
            i++;
        }
        newCharacterSoldierList.Clear();
    }

    public void UpdateSoldierStats(GameObject soldier)
    {
        currentSoldier = soldier.GetComponent<Character>();
        foreach(var stat in soldierStatsList)
        {        
            //if (stat.name == "Type")
            //{
            //    if (currentSoldier.characterBaseValues.Type.ToString() == "Player")
            //    {
            //        stat.GetComponent<Text>().text = "Leader";
            //    }
            //    else
            //    {
            //        stat.GetComponent<Text>().text = currentSoldier.characterBaseValues.Type.ToString();
            //    }
            //}
            if (stat.name == "Damage")
            {
                stat.GetComponent<Text>().text = "Damage: " + currentSoldier.damage.ToString();
            }
            if (stat.name == "Soldier Name")
            {
                stat.GetComponent<Text>().text = currentSoldier.characterBaseValues.name;
            }
           
            if (stat.name == "Health")
            {
                stat.GetComponent<Text>().text = "Health: " + currentSoldier.health.ToString();
            }
            if (stat.name == "Damage Speed")
            {
                stat.GetComponent<Text>().text = "Attack speed: " + currentSoldier.damageSpeed.ToString();
            }
            if (stat.name == "Range")
            {
                stat.GetComponent<Text>().text = "Range: " + currentSoldier.range.ToString();
            }
            if (stat.name == "Combat Trait")
            {
                stat.GetComponent<Text>().text = Regex.Replace(currentSoldier.characterBaseValues.combatTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2") + ":\n" + GetComponent<TraitDescription>().chooseCombatTraitDescription(currentSoldier.characterBaseValues);
            }
            if (stat.name == "Target Trait")
            {
                stat.GetComponent<Text>().text = Regex.Replace(currentSoldier.characterBaseValues.targetTrait.ToString(), "([a-z])_?([A-Z])", "$1 $2") + ":\n" + GetComponent<TraitDescription>().chooseTargetTraitDescription(currentSoldier.characterBaseValues);
            }

        }
            
    }

    #region InventoryMethods

    public void ActivateInventoryPanel()
    {

        //panelList[4].GetComponent<EquippableItemUIListController>().GenerateItemsList(dataService.GetEquippableItemsValuesFromInventory().ToList().Add(dataService.GetCharacterEquippableItemsValues(currentSoldier.characterBaseValues.id).FirstOrDefault()).ToArray());
        List<EquippableitemValues> weaponsList = dataService.GetEquippableItemsValuesFromInventory().ToList();
        weaponsList.Insert(0, currentSoldier.GetComponentInChildren<EquippableItem>().itemValues);

        panelList[4].GetComponent< EquippableItemUIListController >().GenerateItemsList(weaponsList.ToArray());
        
        panelList[4].SetActive(true);
        
    }

    public void AssignWeaponToSoldier(EquippableitemValues weaponValues)
    {
        IEnumerable<GameObject> weapon = dataService.GenerateEquippableItemsFromValues(new[] { weaponValues });
        dataService.equipItemsToCharacter(weapon, currentSoldier);
        UpdateSoldierStats(currentSoldier.gameObject);
        SetCampAnimation(currentSoldier);

        //foreach (var camera in soldierCameraList)
        //{
        //    if (camera.enabled == true)
        //    {
        //        if (weaponValues.Type == EquippableitemValues.type.rifle)
        //        {
        //            camera.cullingMask = camera.cullingMask ^= (1 << LayerMask.NameToLayer("Rifle") | 1 << LayerMask.NameToLayer("Player"));
        //        }
        //        if (weaponValues.Type == EquippableitemValues.type.shield)
        //        {
        //            camera.cullingMask ^= (1 << LayerMask.NameToLayer("Shield") | 1 << LayerMask.NameToLayer("Player"));
        //        }
        //        if (weaponValues.Type == EquippableitemValues.type.polearm)
        //        {
        //            camera.cullingMask ^= (1 << LayerMask.NameToLayer("Stick") | 1 << LayerMask.NameToLayer("Player"));
        //        }
        //    }
        //}

        //panelList[4].SetActive(false);
        //panelList[5].SetActive(false);
        //panelList[1].SetActive(true);
    }

    #endregion

    public GameObject GenerateNewHunter(Transform newSoldierTrans)
    {
        CharacterValues newCharValues = GenerateNewHunterValues();
        //add to database
        dataService.AddcharacterToDbByValues(newCharValues);

        GameObject hunter = dataService.GenerateCharacterFromValues(newCharValues, newSoldierTrans.position);
       
        //create new weapon for new soldier
        Array itemValues = Enum.GetValues(typeof(EquippableitemValues.type));
        EquippableitemValues newWeaponValues = GetComponent<WeaponGenerator>().GenerateEquippableItem((EquippableitemValues.type)itemValues.GetValue(UnityEngine.Random.Range(0, itemValues.Length)), 1);
        //save the weapon in db
        newWeaponValues.id = dataService.AddWeaponInDbByValues(newWeaponValues);
        //spawn weapon
        IEnumerable<GameObject> newWeapon = dataService.GenerateEquippableItemsFromValues(new[] { newWeaponValues });
        Character hunterChar = hunter.GetComponent<Character>();
        //attach the weapon
        
        dataService.equipItemsToCharacter(newWeapon, hunterChar);

        return hunter;

    }

    public CharacterValues GenerateNewHunterValues(int points = 0, float strenghtProbab = 0)
    {
        if (points != 0)
            newCharPoints = points;
        if (strenghtProbab != 0)
            damagePointsChance = strenghtProbab;

        CharacterValues newCharValues = new CharacterValues();
        //generate prefab 
        newCharValues.prefabName = StringResources.follower1PrefabName;
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        //generate random name
        String characterName;
        if(randomValue > 0.5f)//we create a Male
        {
            characterName = StringResources.maleNames[UnityEngine.Random.Range(0, StringResources.maleNames.Length)];
            newCharValues.isMale = true;
            newCharValues.materialName = StringResources.maleHuntersMaterials[UnityEngine.Random.Range(0, StringResources.maleHuntersMaterials.Length)];
        }
        else //we create a female
        {
            characterName =  StringResources.femaleNames[UnityEngine.Random.Range(0, StringResources.femaleNames.Length)];
            newCharValues.isMale = false;
            newCharValues.materialName = StringResources.femaleHuntersMaterials[UnityEngine.Random.Range(0, StringResources.femaleHuntersMaterials.Length)];
        }
        
        //give name to soldier and assign type
        newCharValues.name = characterName;
        newCharValues.Type = CharacterValues.type.Hunter;

        Array values = Enum.GetValues(typeof(CharacterValues.CombatTrait));
        newCharValues.combatTrait = (CharacterValues.CombatTrait)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        values = Enum.GetValues(typeof(CharacterValues.TargetTrait));
        newCharValues.targetTrait = (CharacterValues.TargetTrait)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        //generate stats
        newCharValues = GenerateNewCharacterStats(newCharValues);

        newCharValues.Type = CharacterValues.type.NewHunter;

        newCharacterSoldierList.Add(newCharValues);
        
        return newCharValues;
    }

    CharacterValues GenerateNewCharacterStats(CharacterValues charValues)
    {
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        for (int i = 0; i < newCharPoints; i ++)
        {
            if(randomValue < damagePointsChance)
            {
                charValues.damage += 1;
            }
            else
            {
                charValues.health += 1;
            }
            randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        }
        
        //charValues.damageSpeed = 5; 
        //charValues.range = 5;    

        return charValues;

    }

}