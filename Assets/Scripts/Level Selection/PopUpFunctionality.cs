using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpFunctionality : MonoBehaviour {

    public GameObject PopUpPanel;
    public GameObject btnScout;
    public GameObject btnPlay;
    public GameObject invisPanel;

    public GameObject confirmPanel;
    public GameObject confirmPanelScout;

    // Left side shown when you have scouted
    public GameObject ScoutedPanel;
    //public Text wolveText;
    public Text tribeText;
    //public Text choiceText;

    // Right side shown when you have scouted
    public Text foodText;
    public Text scrapsText;
    public Text goldTeethsText;
    public Text itemsDropText;

    // Not scouted panel
    public GameObject NotScoutedPanel;
    //public Text interestPointsText;

    //Scout Cost
    public int minimumScoutCost;
    public int maximumScoutCost;

    void OnEnable()
    {
        EventManager.Instance.StartListening<SetupPopUp>(InitialisePopUP);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<SetupPopUp>(InitialisePopUP);
    }

    void OnApplicationQuit()
    {
       // this.enabled = false;
    }

    public void InitialisePopUP(SetupPopUp e)
    {
        GameObject node = e.node;
        Node nodeScript = node.GetComponent<Node>();

        if (nodeScript.isCleared) { 
            Manager_Audio.PlaySound(Manager_Audio.play_clickClearedNode, gameObject);
            return;
        }

        invisPanel.SetActive(true);

        PopUpPanel.SetActive(true);
        btnPlay.GetComponent<Button>().onClick.RemoveAllListeners();
        btnScout.GetComponent<Button>().onClick.RemoveAllListeners();

        btnPlay.GetComponent<Button>().onClick.AddListener(delegate { Play(node); });
        btnScout.GetComponent<Button>().onClick.AddListener(delegate { Scout(node); });

        btnScout.transform.GetChild(1).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Scout For") + " : " + nodeScript.scoutCost;
        btnPlay.transform.GetChild(1).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Enter For") + " : " + nodeScript.TravelCost;
        tribeText.text = TranslationManager.Instance.GetTranslation("Tribe Camps") + " : " + nodeScript.tribeCamps;
        foodText.text = TranslationManager.Instance.GetTranslation("Food") + " : " + nodeScript.foodAmount;
        scrapsText.text = TranslationManager.Instance.GetTranslation("Scraps") + " : " + nodeScript.scrapAmount;
        goldTeethsText.text = nodeScript.goldTeethAmount + " " + TranslationManager.Instance.GetTranslation("GoldTeeths");

        //interestPointsText.text = TranslationManager.Instance.GetTranslation("Enemy Tribes") + " : " + nodeScript.CampsInNode;
        //choiceText.text = TranslationManager.Instance.GetTranslation("Choice Camps") + " : " + nodeScript.choiceCamps;
        //wolveText.text = TranslationManager.Instance.GetTranslation("Wolve Dens") + " : " + nodeScript.wolveCamps;        

        if (nodeScript.canPlay && !nodeScript.isCleared)
        {
            if (nodeScript.isScouted)
            {
                btnPlay.SetActive(true);
                btnScout.SetActive(false);
                ScoutedPanel.SetActive(true);
                btnPlay.GetComponent<RectTransform>().localPosition = new Vector3(0, -462, 0);
                ScoutedPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 29, 0);
                ScoutedPanel.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0, 170, 0);
                ScoutedPanel.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0, -39, 0);
                ScoutedPanel.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector3(0, -444, 0);
            }
            else
            {
                btnPlay.SetActive(true);
                btnScout.SetActive(true);
                NotScoutedPanel.SetActive(true);
                btnPlay.GetComponent<RectTransform>().localPosition = new Vector3(0, -323, 0);
                btnScout.GetComponent<RectTransform>().localPosition = new Vector3(0, -470, 0);
                NotScoutedPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 150, 0);
            }
        }
        else if (!nodeScript.canPlay)
        {
            if (nodeScript.isScouted)
            {
                btnPlay.SetActive(false);
                btnScout.SetActive(false);
                ScoutedPanel.SetActive(true);
                NotScoutedPanel.SetActive(false);
                ScoutedPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 29, 0);
                ScoutedPanel.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0, 170, 0);
                ScoutedPanel.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0, -39, 0);
                ScoutedPanel.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector3(0, -444, 0);
            }
            else
            {
                btnPlay.SetActive(false);
                btnScout.SetActive(true);
                ScoutedPanel.SetActive(false);
                NotScoutedPanel.SetActive(true);
                btnPlay.GetComponent<RectTransform>().localPosition = new Vector3(0, -323, 0);
                btnScout.GetComponent<RectTransform>().localPosition = new Vector3(0, -470, 0);
                NotScoutedPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 150, 0);
            }
        }
    }

    public void Play(GameObject node)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);

        confirmPanel.SetActive(true);
        int value = node.GetComponent<Node>().TravelCost;

        if (GameController.Instance._FOOD - value < 0)
        {
            confirmPanel.transform.GetChild(1).gameObject.SetActive(true);

            confirmPanel.GetComponent<ConfirmPanel>().btnNoVillage.GetComponent<Button>().onClick.RemoveAllListeners();
            confirmPanel.GetComponent<ConfirmPanel>().btnYesVillage.GetComponent<Button>().onClick.RemoveAllListeners();

            confirmPanel.GetComponent<ConfirmPanel>().btnNoVillage.GetComponent<Button>().onClick.AddListener(Deny);
            confirmPanel.GetComponent<ConfirmPanel>().btnYesVillage.GetComponent<Button>().onClick.AddListener(delegate { AcceptPlay(node); });
        }
        else
        {
            confirmPanel.transform.GetChild(0).gameObject.SetActive(true);

            confirmPanel.GetComponent<ConfirmPanel>().btnNo.GetComponent<Button>().onClick.RemoveAllListeners();
            confirmPanel.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.RemoveAllListeners();

            confirmPanel.GetComponent<ConfirmPanel>().btnNo.GetComponent<Button>().onClick.AddListener(Deny);
            confirmPanel.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.AddListener(delegate { AcceptPlay(node); });
        }

        confirmPanel.GetComponent<ConfirmPanel>().SetupText(node, "play", value); 


    }

    public void Scout(GameObject node)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);

        confirmPanelScout.SetActive(true);
        int value = node.GetComponent<Node>().scoutCost;

        if (GameController.Instance._FOOD - value < 0)
        {
            confirmPanelScout.transform.GetChild(1).gameObject.SetActive(true);
            confirmPanelScout.GetComponent<ConfirmPanel>().btnNoVillage.GetComponent<Button>().onClick.RemoveAllListeners();
            confirmPanelScout.GetComponent<ConfirmPanel>().btnYesVillage.GetComponent<Button>().onClick.RemoveAllListeners();

            confirmPanelScout.GetComponent<ConfirmPanel>().btnNoVillage.GetComponent<Button>().onClick.AddListener(Deny);
            confirmPanelScout.GetComponent<ConfirmPanel>().btnYesVillage.GetComponent<Button>().onClick.AddListener(delegate { AcceptScout(node); });
        } else
        {
            confirmPanelScout.transform.GetChild(0).gameObject.SetActive(true);
            confirmPanelScout.GetComponent<ConfirmPanel>().btnNo.GetComponent<Button>().onClick.RemoveAllListeners();
            confirmPanelScout.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.RemoveAllListeners();

            confirmPanelScout.GetComponent<ConfirmPanel>().btnNo.GetComponent<Button>().onClick.AddListener(Deny);
            confirmPanelScout.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.AddListener(delegate { AcceptScout(node); });
        }

        confirmPanelScout.GetComponent<ConfirmPanel>().SetupText(node, "scout", value);


    }

    public void AcceptScout(GameObject node)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_scouting, gameObject);
        EventManager.Instance.TriggerEvent(new ChangeResources(-node.GetComponent<Node>().scoutCost));

        btnScout.SetActive(false);
        NotScoutedPanel.SetActive(false);
        ScoutedPanel.SetActive(true);
        confirmPanel.SetActive(false);

        node.GetComponent<Node>().isScouted = true;

        foreach (RectTransform child in node.transform)
        {
            if (child.name == "InfoPanel")
            {
                child.gameObject.SetActive(true);
                foodText.text = node.GetComponent<Node>().foodAmount.ToString() + " " + TranslationManager.Instance.GetTranslation("Food");
                scrapsText.text = node.GetComponent<Node>().scrapAmount.ToString() + " " + TranslationManager.Instance.GetTranslation("Scraps");
                tribeText.text = node.GetComponent<Node>().tribeCamps.ToString() + " " + TranslationManager.Instance.GetTranslation("Tribe Camps");
                itemsDropText.text = node.GetComponent<Node>().itemDropAmount.ToString() + " " + TranslationManager.Instance.GetTranslation("Items");
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }

        btnPlay.GetComponent<RectTransform>().localPosition = new Vector3(0, -462, 0);
        ScoutedPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 29, 0);
        ScoutedPanel.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0, 170, 0);
        ScoutedPanel.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0, -39, 0);
        ScoutedPanel.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector3(0, -444, 0);

        EventManager.Instance.TriggerEvent(new SaveLevelsToXML());
    }

    public void AcceptPlay(GameObject node)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_intoLevel, gameObject);

        EventManager.Instance.TriggerEvent(new ChangeResources(-node.GetComponent<Node>().TravelCost));

        PlayerPrefs.SetInt(StringResources.NodeIdPrefsName, node.GetComponent<Node>().NodeId);
        PlayerPrefs.SetInt(StringResources.LevelDifficultyPrefsName, node.GetComponent<Node>().Level);
        PlayerPrefs.SetInt(StringResources.TribeCampsPrefsName, node.GetComponent<Node>().tribeCamps);
        PlayerPrefs.SetInt(StringResources.FoodAmountPrefsName, node.GetComponent<Node>().foodAmount);
        PlayerPrefs.SetInt(StringResources.ScrapAmountPrefsName, node.GetComponent<Node>().scrapAmount);
        PlayerPrefs.SetInt(StringResources.ItemDropAmountPrefsName, node.GetComponent<Node>().itemDropAmount);
        PlayerPrefs.SetInt(StringResources.PremiumDropAmountPrefsName, node.GetComponent<Node>().goldTeethAmount);

        EventManager.Instance.TriggerEvent(new LevelStarted());

        //GameController.Instance.LoadScene("LevelEnterCutscene");
        EventManager.Instance.TriggerEvent(new EndSceneTransitionEvent("LevelEnterCutscene"));
    }

    public void Deny()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        confirmPanel.SetActive(false);
    } 
}
