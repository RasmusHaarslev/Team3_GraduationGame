using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using UnityEngine.Networking;
using System.Threading;

public class PanelScript : MonoBehaviour
{
    CampTopPanel campTopPanelScript;
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
    List<EquippableitemValues> newWeaponsSoldierList = new List<EquippableitemValues>();  
    public int newCharPoints = 15;
    [Range(0f, 1f)]
    public float damagePointsChance = 0.5f;
    public Transform camsAndNewSoldiersPosition;
    public GameObject silhouette;
    public bool alreadyGeneratedNewSoldiers = false;
    List<GameObject> newSoldiersList = new List<GameObject>();
    public GameObject silhouetteGO;
    public List<Transform> silhouettePosList = new List<Transform>();
    bool reroll = false;
    List<int> soldierTierList = new List<int>();
    Vector3 MoveText = new Vector3(0, 300, 0);
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
        campTopPanelScript = GetComponentInChildren<CampTopPanel>();
        dataService = new DataService(StringResources.databaseName);
        dataService.CreateDB();
        charactersFellowship = dataService.GetPlayerFellowshipInPosition(solidersSpawnPosition);
        InitializeSoldiers();
        InitializeNewHunters();
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
        /*
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
        */
    }

    public void DeactivateNewSoldiers()
    {
        /*
        foreach (GameObject soldier in newSoldiersList)
        {
            if (soldier.activeSelf == true)
            {
                soldier.SetActive(false);
            }
        }
        
        foreach (Transform trans in camsAndNewSoldiersPosition)
        {
            if (trans.gameObject.GetComponentInChildren<Camera>().isActiveAndEnabled == true && trans.gameObject.GetComponentInChildren<Light>().isActiveAndEnabled == true)
            {
                trans.gameObject.GetComponentInChildren<Camera>().enabled = false;
                trans.gameObject.GetComponentInChildren<Light>().enabled = false;
            }
        }
        */
    }

    public void InitializeNewHunters(bool isFirstTime = true)
    {
        var start1 = DateTime.Now;
        CharacterValues[] newHuntersValues = new CharacterValues[1];
        if (isFirstTime)
            newHuntersValues = dataService.GetNewHuntersValues();
        EquippableitemValues[] newHunterEquipsValues;
        if (newHuntersValues.Length < 3 || reroll)
        {
            var start = DateTime.Now;
            //creating new values for hunters
            newSoldiersList.Clear(); //clears the old list
            //add 3 new hunters
            newHuntersValues = new CharacterValues[3];
            newHunterEquipsValues = new EquippableitemValues[3];
            int i = 0;
            GameObject newHunter;
            foreach (Transform trans in camsAndNewSoldiersPosition)
            {
                newHunter = GenerateNewHunterGameObject(trans);
                newSoldiersList.Add(newHunter);
                newHuntersValues[i] = newHunter.GetComponent<Character>().characterBaseValues;
                newHunterEquipsValues[i] = newHunter.GetComponentInChildren<EquippableItem>().itemValues;
                newHuntersValues[i].id = 20 + i; //todo id handled here
                newHunterEquipsValues[i].id = 20 + i;
                newHunterEquipsValues[i].characterId = 20 + i;
                //print(newHuntersValues[i].id);
                i++;

            }
            print("Creation of 3 models " + (DateTime.Now - start).TotalMilliseconds);

            dataService.UpdateCharactersValues(newHuntersValues);
            dataService.UpdateEquipItemsValues(newHunterEquipsValues);
            
        }
        else
        {
            EquippableitemValues[] newhuntersEquips = dataService.GetNewHuntersEquipValues();
            newCharacterSoldierList = newHuntersValues.ToList();
            newWeaponsSoldierList = newhuntersEquips.ToList();
            int i = 0;
            foreach (Transform trans in camsAndNewSoldiersPosition)
            { //if we have 3 new hunters values, generate them and add to the list
                newSoldiersList.Add(dataService.spawnCharacterGameObject(newHuntersValues[i], newhuntersEquips[i], trans));

                i++;
            }
        }

        foreach (GameObject newSoldier in newSoldiersList)
        {
            if (newSoldier.GetComponent<NavMeshAgent>().enabled == true && newSoldier.GetComponent<HunterStateMachine>().enabled == true)
            {
                newSoldier.GetComponent<NavMeshAgent>().enabled = false;
                newSoldier.GetComponent<HunterStateMachine>().enabled = false;
                newSoldier.GetComponent<PlayFootStepParticles>().enabled = false;
                
                if (newSoldier.GetComponentsInChildren<ShootRifle>().Count() > 0)
                {
                    newSoldier.GetComponentsInChildren<ShootRifle>()[0].enabled = false;
                }
            }
        }
        newCharacterSoldierList = newHuntersValues.ToList();
        reroll = false;
    }

    public void SpawnNewSoldier(int index)
    {
        var start = DateTime.Now;
        for (int i = 0; i < newSoldiersList.Count; i++)
        {
            if (index == i)
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
                        valuesToKeep.id = soldiertrans.GetComponent<CharacterSpawner>().tier ;
                        EquippableitemValues equipValuesToKeep = newSoldiersList[i].GetComponentInChildren<EquippableItem>().itemValues;
                        equipValuesToKeep.id = soldiertrans.GetComponent<CharacterSpawner>().tier ;
                        equipValuesToKeep.characterId = equipValuesToKeep.id;
                       
                        print("updating char id "+valuesToKeep.id);
                        dataService.UpdateCharacterValuesInDb(valuesToKeep);
                        dataService.UpdateEquipItemValues(equipValuesToKeep);
                       
                        /*
                                                GameObject newWeapon = dataService.GenerateNewEquippableItemFromValues(newSoldiersList[i].GetComponentInChildren<EquippableItem>().itemValues);
                                                print("id of the weapon " + newWeapon.GetComponent<EquippableItem>().itemValues.id);

                                                //destroy current puppet weapon
                                                Destroy(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject);
                                                //equip weapon
                                                print(newSoldiersList[i].GetComponent<Character>().characterBaseValues.name);

                                                dataService.equipItemsToCharacter(new[] { newWeapon }, newSoldiersList[i].GetComponent<Character>());
                                                */
                        //newSoldiersList[i].transform.parent = soldiertrans;
                        newSoldiersList[i].transform.position = soldiertrans.position;
                        newSoldiersList[i].transform.rotation = soldiertrans.rotation;
                        /* newSoldiersList[i].transform.localPosition = soldiertrans.localPosition;
                         newSoldiersList[i].transform.localRotation = soldiertrans.localRotation;*/
                        newSoldiersList[i].AddComponent<PanelController>();
                        //newSoldiersList[i].AddComponent<shaderGlow>();
                        SetCampAnimation(newSoldiersList[i].GetComponent<Character>());
                        if (soldiertrans.localPosition == solidersSpawnPosition.GetChild(1).localPosition)
                        {
                            newSoldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
                            //newWeapon.layer = LayerMask.NameToLayer("Hunter1"); 
                            newSoldiersList[i].GetComponentsInChildren<EquippableItem>()[0].gameObject.layer = LayerMask.NameToLayer("Hunter1");
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.name);
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.layer);
                            
                        }
                        if (soldiertrans.localPosition == solidersSpawnPosition.GetChild(2).localPosition)
                        {
                            newSoldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter2");
                            //newWeapon.layer = LayerMask.NameToLayer("Hunter1");
                            newSoldiersList[i].GetComponentsInChildren<EquippableItem>()[0].gameObject.layer = LayerMask.NameToLayer("Hunter2");
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.name);
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.layer);
                        }
                        if (soldiertrans.localPosition == solidersSpawnPosition.GetChild(3).localPosition)
                        {
                            newSoldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter3");
                            //newWeapon.layer = LayerMask.NameToLayer("Hunter3");
                            newSoldiersList[i].GetComponentsInChildren<EquippableItem>()[0].gameObject.layer = LayerMask.NameToLayer("Hunter3");
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject);
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.layer);
                        }
                        campTopPanelScript.ScaleVillageCount();
                        EventManager.Instance.TriggerEvent(new ChangeResources(villager: -1));
                    }
                }



            }
            else
            {
                Destroy(newSoldiersList[i]);
                //TODO can improve !!
                //dataService.DeleteCharactersValuesFromDb(newSoldiersList[i].GetComponent<Character>().characterBaseValues);
            }
        }
        //after the hunter is placed 
        newSoldiersList.Clear();
        alreadyGeneratedNewSoldiers = false;
        Destroy(silhouetteGO);

        InitializeNewHunters(false);
        //FillInNewSoldierStats();

        print("whole thing " + (DateTime.Now - start).TotalMilliseconds);
    }

    /*
     public void SpawnNewSoldier(int index)
    {
        for (int i = 0; i < newSoldiersList.Count; i++)
        {
            if (index == i)
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
                        print("id of the weapon " + newWeapon.GetComponent<EquippableItem>().itemValues.id);
                        //destroy current puppet weapon
                        Destroy(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject);
                        //equip weapon
                        print(newSoldiersList[i].GetComponent<Character>().characterBaseValues.name);
                        
                        dataService.equipItemsToCharacter(new[] { newWeapon }, newSoldiersList[i].GetComponent<Character>());

                        newSoldiersList[i].transform.localPosition = soldiertrans.localPosition;
                        newSoldiersList[i].transform.localRotation = soldiertrans.localRotation;
                        newSoldiersList[i].AddComponent<PanelController>();
                        SetCampAnimation(newSoldiersList[i].GetComponent<Character>());
                        if (soldiertrans.localPosition == solidersSpawnPosition.GetChild(1).localPosition)
                        {
                            newSoldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
                            //newWeapon.layer = LayerMask.NameToLayer("Hunter1"); 
                            //newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.layer = LayerMask.NameToLayer("Hunter1");
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.name);
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.layer);
                        }
                        if (soldiertrans.localPosition == solidersSpawnPosition.GetChild(2).localPosition)
                        {
                            newSoldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter2");
                            //newWeapon.layer = LayerMask.NameToLayer("Hunter1");
                            //newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.layer = LayerMask.NameToLayer("Hunter2");
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.name);
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.layer);
                        }
                        if (soldiertrans.localPosition == solidersSpawnPosition.GetChild(3).localPosition)
                        {
                            newSoldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter3");
                            //newWeapon.layer = LayerMask.NameToLayer("Hunter3");
                            //newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.layer = LayerMask.NameToLayer("Hunter3");
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject);
                            //print(newSoldiersList[i].GetComponentInChildren<EquippableItem>().gameObject.layer);
                        }
                        EventManager.Instance.TriggerEvent(new ChangeResources(villager: -1));
                    }
                }



            }
            else
            {
                Destroy(newSoldiersList[i]);
                
                dataService.DeleteCharactersValuesFromDb(newSoldiersList[i].GetComponent<Character>().characterBaseValues);
            }
        }
        newSoldiersList.Clear();
        alreadyGeneratedNewSoldiers = false;
        Destroy(silhouetteGO);
    } 
     * */

    public void RerollSoldiers()
    {
        print("doing reroll");
        if (GameController.Instance._PREMIUM >= 200)
        {
            reroll = true;
            /**/
            foreach (GameObject soldier in newSoldiersList)
            {
                //dataService.DeleteCharactersValuesFromDb(soldier.GetComponent<Character>().characterBaseValues);
                Destroy(soldier);

            }
            
            newSoldiersList.Clear();
            InitializeNewHunters(false);
            //GetNewSoldiers();
            FillInNewSoldierStats();

            campTopPanelScript.ScalePremiumCount();
            EventManager.Instance.TriggerEvent(new ChangeResources(premium: -200));
        }
        else
        {

        }


    }
    //happens when clicking on an empty figure
    public void GetNewSoldiers()
    {

        //if reroll was pressed
        if (reroll)
        {
            print("doing reroll");
            newSoldiersList.Clear(); //clears the old list
            //add 3 new hunters
            foreach (Transform trans in camsAndNewSoldiersPosition)
            {
                newSoldiersList.Add(GenerateNewHunterGameObject(trans));
            }
            //TODO save new values into database async
        }/*
        else
        {
            newCharacterSoldierList = newHuntersValues.ToList();
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
        */
        FillInNewSoldierStats();

        reroll = false;

    }
    /*
    public void GetNewSoldiers()
    {
        var start = DateTime.Now;

        CharacterValues[] newHuntersValues = dataService.GetNewHuntersValues();

        //check if there are already new hunters into database
        if (reroll)
        {
            start = DateTime.Now;
            //add one new hunter
            foreach (Transform trans in camsAndNewSoldiersPosition)
            {
                newSoldiersList.Add(GenerateNewHunterGameObject(trans));
            }
            //print("Generating" + (DateTime.Now - start).TotalSeconds);
        }
        else
        {
            newCharacterSoldierList = newHuntersValues.ToList<CharacterValues>();
            int i = 0;
            foreach (Transform trans in camsAndNewSoldiersPosition)
            { //if we have 3 new hunters values, generate them and add to the list

                newSoldiersList.Add(dataService.GenerateCharacterFromValues(newHuntersValues[i], trans.position));
                i++;
            }
        }

        start = DateTime.Now;
        foreach (GameObject newSoldier in newSoldiersList)
        {
            if (newSoldier.GetComponent<NavMeshAgent>().enabled == true && newSoldier.GetComponent<HunterStateMachine>().enabled == true)
            {
                newSoldier.GetComponent<NavMeshAgent>().enabled = false;
                newSoldier.GetComponent<HunterStateMachine>().enabled = false;
            }
        }
        //print("Removing navmesh " + (DateTime.Now - start).TotalSeconds);
        start = DateTime.Now;
        //newSoldiersList[0].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter1");
        //newSoldiersList[1].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter2");
        //newSoldiersList[2].transform.GetChild(2).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hunter3");

        FillInNewSoldierStats();
        //print("After filling " + (DateTime.Now - start).TotalSeconds);
        reroll = false;

        ItemController.SaveItem(ItemController.ItemsLoaded);
        CharacterController.SaveCharacters(CharacterController.CharactersLoaded);
    }

*/

    void InitializeSoldiers()
    {
        int[] layersIndices = new[] { 9, 10, 11, 12 };


        for (int i = 0; i < charactersFellowship.transform.childCount; i++)
        {
            soldiersList.Add(charactersFellowship.transform.GetChild(i).gameObject);
        }
       
        for (int i = 0; i < soldiersList.Count; i++)
        {

            soldiersList[i].AddComponent<PanelController>();
            //soldiersList[i].AddComponent<shaderGlow>();
            soldiersList[i].GetComponent<NavMeshAgent>().enabled = false;
            soldiersList[i].GetComponent<PlayFootStepParticles>().enabled = false;
            if (soldiersList[i].GetComponentsInChildren<ShootRifle>().Count() > 0)
            {
               
                soldiersList[i].GetComponentsInChildren<ShootRifle>()[0].enabled = false;
            }

            if (soldiersList[i].GetComponent<Character>().characterBaseValues.Type == CharacterValues.type.Player)
            {
                soldiersList[i].GetComponent<MoveScript>().enabled = false;
                soldiersList[i].GetComponent<Formation>().enabled = false;
            }
            else
            {
                soldiersList[i].GetComponent<HunterStateMachine>().enabled = false;
            }
            if (soldiersList[i].GetComponent<Character>().characterBaseValues.id == 1)
            {
                soldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = 9;
                soldiersList[i].GetComponentsInChildren<EquippableItem>()[0].gameObject.layer = 9;
            }
            if (soldiersList[i].GetComponent<Character>().characterBaseValues.id == 2)
            {
                soldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = 10;
                soldiersList[i].GetComponentsInChildren<EquippableItem>()[0].gameObject.layer = 10;
            }
            if (soldiersList[i].GetComponent<Character>().characterBaseValues.id == 3)
            {
                soldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = 11;
                soldiersList[i].GetComponentsInChildren<EquippableItem>()[0].gameObject.layer = 11;
            }
            if (soldiersList[i].GetComponent<Character>().characterBaseValues.id == 4)
            {
                soldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = 12;
                soldiersList[i].GetComponentsInChildren<EquippableItem>()[0].gameObject.layer = 12;
            }
            //soldiersList[i].transform.GetChild(2).transform.GetChild(0).gameObject.layer = layersIndices[i];
            //soldiersList[i].GetComponentsInChildren<EquippableItem>()[0].gameObject.layer = layersIndices[i];
            

            // Switches the animator out with the camp animator.
            SetCampAnimation(soldiersList[i].GetComponent<Character>());
        }
        
        if (solidersSpawnPosition.childCount != soldiersList.Count)
        {

            
            foreach (GameObject soldier in soldiersList)
            {
                soldierTierList.Add(soldier.GetComponent<Character>().characterBaseValues.id);

            }
            print(soldierTierList.Count());
            foreach (var v in soldierTierList)
            {
                print(v);
            }
            //print(solidersSpawnPosition.childCount);
            for (int i = 1; i < solidersSpawnPosition.childCount + 1; i++)
            {
                if (!soldierTierList.Contains(i))
                {
                    print(i);
                    Instantiate(silhouette, solidersSpawnPosition.GetChild(i - 1).position + Vector3.up, Quaternion.identity);
                }
            }


        }

    }

    private void SetCampAnimation(Character soldier)
    {
        // Switches the animator out with the camp animator.
        RuntimeAnimatorController newController = (RuntimeAnimatorController)Resources.Load("CampIdleControllers/CampIdleControllerUpdated");
        soldier.GetComponent<Animator>().runtimeAnimatorController = newController;

        if (soldier.transform.position == solidersSpawnPosition.GetChild(0).position)
        {
            switch (soldier.equippedWeaponType)
            {
                case EquippableitemValues.type.polearm:
                    soldier.animator.Play("StandingIdleSpear1");

                    Vector3 newPosition1 = new Vector3(0.034f, -0.061f, -0.092f);
                    Quaternion newRotation1 = Quaternion.Euler(88.096f, -230.06f, -55.345f);

                    if (soldier.GetComponentsInChildren<EquippableItem>().Length > 1)
                    {
                        soldier.GetComponentsInChildren<EquippableItem>()[1].transform.localPosition = newPosition1;
                        soldier.GetComponentsInChildren<EquippableItem>()[1].transform.localRotation = newRotation1;
                    }
                    else
                    {
                        soldier.GetComponentInChildren<EquippableItem>().transform.localPosition = newPosition1;
                        soldier.GetComponentInChildren<EquippableItem>().transform.localRotation = newRotation1;
                    }
                    break;
                case EquippableitemValues.type.rifle:
                    soldier.animator.Play("StandingIdleRifle1");

                    Vector3 newPosition2 = new Vector3(0.019f, -0.041f, -0.022f);
                    Quaternion newRotation2 = Quaternion.Euler(6.115f, 168.97f, -33.529f);

                    if (soldier.GetComponentsInChildren<EquippableItem>().Length > 1)
                    {
                        soldier.GetComponentsInChildren<EquippableItem>()[1].transform.localPosition = newPosition2;
                        soldier.GetComponentsInChildren<EquippableItem>()[1].transform.localRotation = newRotation2;
                    }
                    else
                    {
                        soldier.GetComponentInChildren<EquippableItem>().transform.localPosition = newPosition2;
                        soldier.GetComponentInChildren<EquippableItem>().transform.localRotation = newRotation2;
                    }
                    break;
                case EquippableitemValues.type.shield:
                    soldier.animator.Play("StandingIdleShield1");
                    break;
            }
        }
        else if (soldier.transform.position == solidersSpawnPosition.GetChild(1).position)
        {
            switch (soldier.equippedWeaponType)
            {
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
        else if (soldier.transform.position == solidersSpawnPosition.GetChild(2).position)
        {
            switch (soldier.equippedWeaponType)
            {
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
            switch (soldier.equippedWeaponType)
            {
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

        if (soldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer == 9)
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
        for (int i = 0; i < 4; i++)
        {
            if (i != cameraIndex)
            {

                soldierCameraList[i].enabled = false;
            }
        }
    }

    public void DeactivateSpotligths()
    {
        foreach (Light spotlight in SpotLightList)
        {
            if (spotlight.enabled == true)
            {
                spotlight.enabled = false;
            }
        }
    }
    #endregion

    void FillInNewSoldierStats()
    {
        

        int i = 0;
        foreach (var newSoldierStats in newSoldierStatsList)
        {

            foreach (var stat in newSoldierStats)
            {

                if (stat.name == "Damage")
                {
                    stat.GetComponent<Text>().text = newCharacterSoldierList[i].damage.ToString(); 
                }
                if (stat.name == "Soldier Name")
                {
                    stat.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation(newCharacterSoldierList[i].name); 
                }
                if (stat.name == "Health")
                {
                    stat.GetComponent<Text>().text = newCharacterSoldierList[i].health.ToString();
                }               
                if (stat.name == "Combat Trait")
                {
                    stat.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation(newCharacterSoldierList[i].combatTrait.ToString() + "Description");
                }
                if (stat.name == "Target Trait")
                {
                    stat.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation(newCharacterSoldierList[i].targetTrait.ToString() + "Description");
                }
                if (stat.name == "CTrait")
                {
                    stat.GetComponent<Text>().text = " " + TranslationManager.Instance.GetTranslation(newCharacterSoldierList[i].combatTrait.ToString()) + ":\n";
                }
                if (stat.name == "TTrait")
                {
                    stat.GetComponent<Text>().text = " " + TranslationManager.Instance.GetTranslation(newCharacterSoldierList[i].targetTrait.ToString()) + ":\n";
                }
            
                if (stat.name == "Weapon Description")
                {
                    stat.GetComponent<Text>().text = newWeaponsSoldierList[i].name;
                }

            }
            i++;
        }
        newCharacterSoldierList.Clear();
        newWeaponsSoldierList.Clear();
    }
  
    public void UpdateSoldierStats(GameObject soldier, EquippableitemValues wepValues = null)
    {
        currentSoldier = soldier.GetComponent<Character>();
        foreach (var stat in soldierStatsList)
        {

            EquippableitemValues characterWeaponValues = new EquippableitemValues();

            if (wepValues == null)
                characterWeaponValues = currentSoldier.GetComponentInChildren<EquippableItem>().itemValues;
            else
                characterWeaponValues = wepValues;

           
            if (currentSoldier.characterBaseValues.Type.ToString() == "Hunter")
            {
                if (stat.name == "Damage")
                {
                    if(stat.GetComponent<Text>().enabled == false)
                    {
                        stat.GetComponent<Text>().enabled = true;
                    }
                    
                    stat.GetComponent<Text>().text = (currentSoldier.damage - characterWeaponValues.damage).ToString() + " + " + characterWeaponValues.damage;
                }
                if (stat.name == "Soldier Name")
                {

                    stat.GetComponent<Text>().text = currentSoldier.characterBaseValues.name;
                }
                if (stat.name == "Health")
                {
                   
                    stat.GetComponent<Text>().text = (currentSoldier.health - characterWeaponValues.health).ToString() + " + " + characterWeaponValues.health;
                }
                if (stat.name == "Damage Speed")
                {
                    
                    stat.GetComponent<Text>().text = currentSoldier.damageSpeed.ToString();
                }
                if (stat.name == "Range")
                {
                   
                    stat.GetComponent<Text>().text = currentSoldier.range.ToString();
                }
                if (stat.name == "Combat Trait")
                {
                    if (stat.GetComponent<Text>().enabled == false)
                    {
                        stat.GetComponent<Text>().enabled = true;
                    }
                    stat.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation(currentSoldier.characterBaseValues.combatTrait.ToString() + "Description");
                }
                if (stat.name == "Target Trait")
                {
                    if (stat.GetComponent<Text>().enabled == false)
                    {
                        stat.GetComponent<Text>().enabled = true;
                    }
                    stat.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation(currentSoldier.characterBaseValues.targetTrait.ToString() + "Description");
                }
                if (stat.name == "CTrait")
                {
                    if (stat.GetComponent<Text>().enabled == false)
                    {
                        stat.GetComponent<Text>().enabled = true;
                    }
                    stat.GetComponent<Text>().text = " " + TranslationManager.Instance.GetTranslation(currentSoldier.characterBaseValues.combatTrait.ToString()) + ":\n";
                }
                if (stat.name == "TTrait")
                {
                    if (stat.GetComponent<Text>().enabled == false)
                    {
                        stat.GetComponent<Text>().enabled = true;
                    }
                    stat.GetComponent<Text>().text = " " + TranslationManager.Instance.GetTranslation(currentSoldier.characterBaseValues.targetTrait.ToString()) + ":\n";
                }
                if (stat.name == "Weapon Description")
                {
                   
                    stat.GetComponent<Text>().text = characterWeaponValues.Type.ToString();
                }
                if (stat.name == "Type")
                {
                    stat.GetComponent<Text>().enabled = false;
                }
             
            }

            if (currentSoldier.characterBaseValues.Type.ToString() == "Player")
            {            
                if (stat.name == "Damage")
                {
                        
                    stat.GetComponent<Text>().text = (currentSoldier.damage - characterWeaponValues.damage).ToString() + " + " + characterWeaponValues.damage;
                }
                if (stat.name == "Soldier Name")
                {
                    stat.GetComponent<Text>().text = currentSoldier.characterBaseValues.name;
                }
                if (stat.name == "Health")
                {
                 
                    stat.GetComponent<Text>().text = (currentSoldier.health - characterWeaponValues.health).ToString() + " + " + characterWeaponValues.health;
                }
                if (stat.name == "Damage Speed")
                {
                  
                    stat.GetComponent<Text>().text = currentSoldier.damageSpeed.ToString();
                }
                if (stat.name == "Range")
                {
                   
                    stat.GetComponent<Text>().text = currentSoldier.range.ToString();
                }
                if (stat.name == "Combat Trait")
                {
                  
                    stat.GetComponent<Text>().enabled = false;
                }
                if (stat.name == "Target Trait")
                {
                 
                    stat.GetComponent<Text>().enabled = false;
                }
                if (stat.name == "CTrait")
                {
                  
                    stat.GetComponent<Text>().enabled = false;
                }
                if (stat.name == "TTrait")
                {
                   
                    stat.GetComponent<Text>().enabled = false;
                }
                if (stat.name == "Weapon Description")
                {
                   
                    stat.GetComponent<Text>().text = characterWeaponValues.Type.ToString();
                }
                if (stat.name == "Type")
                {
                    if (stat.GetComponent<Text>().enabled == false)
                    {
                        stat.GetComponent<Text>().enabled = true;
                    }               
                }

            }
        }

    }

    #region InventoryMethods

    public void ActivateInventoryPanel()
    {

        //panelList[4].GetComponent<EquippableItemUIListController>().GenerateItemsList(dataService.GetEquippableItemsValuesFromInventory().ToList().Add(dataService.GetCharacterEquippableItemsValues(currentSoldier.characterBaseValues.id).FirstOrDefault()).ToArray());
        List<EquippableitemValues> weaponsList = dataService.GetEquippableItemsValuesFromInventory().ToList();
        weaponsList = weaponsList.OrderByDescending(o => o.level).ToList();
        weaponsList.Insert(0, currentSoldier.GetComponentInChildren<EquippableItem>().itemValues);

        panelList[4].GetComponent<EquippableItemUIListController>().GenerateItemsList(weaponsList.ToArray());

        panelList[4].SetActive(true);

    }

    public void AssignWeaponToSoldier(EquippableitemValues weaponValues)
    {
        IEnumerable<GameObject> weapon = dataService.GenerateEquippableItemsFromValues(new[] { weaponValues });
        weapon.FirstOrDefault<GameObject>().layer = currentSoldier.transform.GetChild(2).transform.GetChild(0).gameObject.layer;

        dataService.equipItemsToCharacter(weapon, currentSoldier);
        //print(currentSoldier.gameObject.layer);
        //print(currentSoldier.GetComponentsInChildren<EquippableItem>()[0].gameObject.layer);
        
        UpdateSoldierStats(currentSoldier.gameObject, weaponValues);
        SetCampAnimation(currentSoldier);
        //currentSoldier.GetComponentsInChildren<EquippableItem>()[0].gameObject.layer = currentSoldier.gameObject.layer;
        //print(currentSoldier.GetComponentsInChildren<EquippableItem>()[0].gameObject.layer);



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

    public GameObject GenerateNewHunterGameObject(Transform newSoldierTrans)
    {
        print("helo");
        var weaponGenerator = GetComponent<WeaponGenerator>();

        CharacterValues newCharValues = GenerateNewHunterValues();
        //create new weapon for new soldier
        Array itemValues = Enum.GetValues(typeof(EquippableitemValues.type));
        EquippableitemValues newWeaponValues = weaponGenerator.GenerateEquippableItem((EquippableitemValues.type)itemValues.GetValue(UnityEngine.Random.Range(0, itemValues.Length)), 1);

        GameObject hunter = dataService.spawnCharacterGameObject(newCharValues, newWeaponValues, newSoldierTrans);
        hunter.transform.localPosition = Vector3.zero;
        newCharacterSoldierList.Add(newCharValues);
        newWeaponsSoldierList.Add(newWeaponValues);
        print(newWeaponsSoldierList);
        return hunter;
    }
    /*
    public GameObject GenerateNewHunterGameObject(Transform newSoldierTrans)
    {
        var start = DateTime.Now;

        var characterGenerator = new CharacterGenerator();
        var weaponGenerator = new WeaponGenerator();

        CharacterValues newCharValues = characterGenerator.GenerateNewHunterValues();

        GameObject hunter = characterGenerator.GenerateCharacterFromValues(newCharValues, newSoldierTrans.position);

        //create new weapon for new soldier
        Array itemValues = Enum.GetValues(typeof(EquippableitemValues.type));
        EquippableitemValues newWeaponValues = weaponGenerator.GenerateEquippableItem((EquippableitemValues.type)itemValues.GetValue(UnityEngine.Random.Range(0, itemValues.Length)), 1);

        //save the weapon in db
        //newWeaponValues.id = dataService.AddWeaponInDbByValues(newWeaponValues);

        //spawn weapon
        IEnumerable<GameObject> newWeapon = characterGenerator.GenerateEquippableItemsFromValues(new[] { newWeaponValues });
        Character hunterChar = hunter.GetComponent<Character>();
        //attach the weapon

        characterGenerator.equipItemsToCharacter(newWeapon, hunterChar);
        newWeaponValues.characterId = newCharValues.id;

        ItemController.ItemsLoaded.Add(newWeaponValues);
        CharacterController.CharactersLoaded.Add(newCharValues);

        newCharacterSoldierList.Add(newCharValues);
        print("Total time " + (DateTime.Now - start).TotalSeconds);

        return hunter;
    }
    */
    //public GameObject GenerateNewHunterGameObject(Transform newSoldierTrans)
    //{
    //    print("---------------------------------------------------------------");
    //    var start = DateTime.Now;
    //    CharacterValues newCharValues = GenerateNewHunterValues();
    //    //add to database
    //    dataService.AddcharacterToDbByValues(newCharValues);
    //    GameObject hunter = dataService.GenerateCharacterFromValues(newCharValues, newSoldierTrans.position);

    //    //create new weapon for new soldier
    //    Array itemValues = Enum.GetValues(typeof(EquippableitemValues.type));
    //    EquippableitemValues newWeaponValues = GetComponent<WeaponGenerator>().GenerateEquippableItem((EquippableitemValues.type)itemValues.GetValue(UnityEngine.Random.Range(0, itemValues.Length)), 1);
    //    //save the weapon in db
    //    newWeaponValues.id = dataService.AddWeaponInDbByValues(newWeaponValues);
    //    //spawn weapon
    //    IEnumerable<GameObject> newWeapon = dataService.GenerateEquippableItemsFromValues(new[] { newWeaponValues });
    //    Character hunterChar = hunter.GetComponent<Character>();
    //    //attach the weapon

    //    dataService.equipItemsToCharacter(newWeapon, hunterChar);
    //    print("Total " + (DateTime.Now - start).TotalSeconds);
    //    return hunter;
    //}

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
        if (randomValue > 0.5f)//we create a Male
        {
            characterName = StringResources.maleNames[UnityEngine.Random.Range(0, StringResources.maleNames.Length)];
            newCharValues.isMale = true;
            newCharValues.materialName = StringResources.maleHuntersMaterials[UnityEngine.Random.Range(0, StringResources.maleHuntersMaterials.Length)];
        }
        else //we create a female
        {
            characterName = StringResources.femaleNames[UnityEngine.Random.Range(0, StringResources.femaleNames.Length)];
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

        //newCharacterSoldierList.Add(newCharValues);

        return newCharValues;
    }

    CharacterValues GenerateNewCharacterStats(CharacterValues charValues)
    {
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        for (int i = 0; i < newCharPoints; i++)
        {
            if (randomValue < damagePointsChance)
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