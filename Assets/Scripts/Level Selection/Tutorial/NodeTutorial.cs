using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NodeTutorial : MonoBehaviour {

    public GameObject scrollingGrid;

    public GameObject infoPanel;
    public GameObject notScoutPanel;
    public GameObject circularProgress;
    public Text txtIntPoints;

    [Tooltip("Removes the panel on the right of the node")]
    public bool showInfoPanel = true;

    [Tooltip("Removes the scouting functionality")]
    public bool showScoutPanel = true;

    public int TravelCost;
    public int scoutCost;
    public int CampsInNode;

    public int foodAmount;
    public int scrapAmount;
    public int goldTeethAmount;

    /* AMOUNT OF ITEM DROPS */
    public int itemDropAmount;

    public bool isCleared = false;
    public bool isScouted = false;
    public bool canPlay = false;
    public bool isOpen = false;

    public GameObject PopUpPanel;
    public GameObject btnScout;
    public GameObject btnPlay;
    public GameObject invisPanel;

    public GameObject confirmPanel;

    // Left side shown when you have scouted
    public GameObject ScoutedPanel;
    public Text tribeText;

    // PopUp Text shown when you have scouted
    public Text foodText;
    public Text scrapsText;
    public Text goldTeethsText;
    public Text itemText;

    // Right side of node shown when you have scouted
    public Text txtFood;
    public Text txtScraps;
    public Text txtTribes;

    // Not scouted panel
    public GameObject NotScoutedPanel;
    public Text interestPointsText;    

    public List<Sprite> activationImages = new List<Sprite>();

    /* ROADS FROM THIS NODE */
    public List<GameObject> Links = new List<GameObject>();

    void Start()
    {
        SetScrollPosition(0);
        SetupNodes();
    }

    public void SetupNodes()
    {
        if (transform.name == "Tut1")
        {   
            ChangeRoot();
        }

        interestPointsText.text = CampsInNode.ToString();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(ClickNode);

        if (!showInfoPanel)
        {
            infoPanel.SetActive(false);
            notScoutPanel.SetActive(false);
        }
    }

    public void ChangeRoot()
    {
        gameObject.GetComponent<Image>().sprite = activationImages[1];
        canPlay = true;
        isOpen = true;
    }

    public void ClickNode()
    {
        InitialisePopUP(gameObject);
    }

    #region WIN ANIMATION
    IEnumerator initWin(GameObject node)
    {
        List<GameObject> nodeList = Links;

        Manager_Audio.PlaySound(Manager_Audio.play_fadeNode, gameObject);
        node.GetComponent<Animator>().SetTrigger("IsCleared");
        yield return new WaitForSeconds(1f);

        SetupUIText();
        node.transform.GetChild(2).gameObject.SetActive(true);

        foreach (var childNode in nodeList)
        {
            if (!childNode.GetComponent<NodeTutorial>().isCleared && !childNode.GetComponent<NodeTutorial>().isOpen)
            {
                childNode.GetComponent<NodeTutorial>().isOpen = true;
                Manager_Audio.PlaySound(Manager_Audio.play_fadeNode, gameObject);
                childNode.GetComponent<Animator>().SetTrigger("IsUnlocked");
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void SetupUIText()
    {
        if (isCleared)
        {
            notScoutPanel.SetActive(false);
            infoPanel.SetActive(false);
        }
        else if (isScouted)
        {
            notScoutPanel.SetActive(false);
            infoPanel.SetActive(true);

            foreach (RectTransform child in transform)
            {
                if (child.name == "InfoPanel")
                {
                    child.gameObject.SetActive(true);

                    GetComponent<NodeTutorial>().txtFood.text = GetComponent<NodeTutorial>().foodAmount.ToString();
                    GetComponent<NodeTutorial>().txtScraps.text = GetComponent<NodeTutorial>().scrapAmount.ToString();
                    GetComponent<NodeTutorial>().txtTribes.text = GetComponent<NodeTutorial>().CampsInNode.ToString();
                }
            }

        }
        else
        {
            infoPanel.SetActive(false);
            notScoutPanel.SetActive(true);
            txtIntPoints.text = CampsInNode.ToString();
        }
    }
    #endregion

    #region SCROLL TO
    public void SetScrollPosition(int rowNumber)
    {
        /* SETUP SCROLL POSITION */
        float rowHeight = 192*rowNumber;

        Vector2 initPos = new Vector2(scrollingGrid.GetComponent<RectTransform>().anchoredPosition.x, -500f);
        Vector2 desPos = new Vector2(scrollingGrid.GetComponent<RectTransform>().anchoredPosition.x, -500 + rowHeight);

        StartCoroutine(MoveFromTo(initPos, desPos, 0.2f));
    }

    IEnumerator MoveFromTo(Vector2 pointA, Vector2 pointB, float time)
    {
        float t = 0f;
        Manager_Audio.PlaySound(Manager_Audio.play_scrollMap, gameObject);
        while (t < 1.0f)
        {
            t += Time.deltaTime / time; // Sweeps from 0 to 1 in time seconds
            scrollingGrid.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(pointA, pointB, t); // Set position proportional to t
            Manager_Audio.SendParameterValue(Manager_Audio.adjustScrollPitch, t);
            yield return null;         // Leave the routine and return here in the next frame
        }
        Manager_Audio.PlaySound(Manager_Audio.stop_scrollMap, gameObject);
    }
    #endregion

    #region POPUP
    public void InitialisePopUP(GameObject e)
    {
        NodeTutorial nodeScript = gameObject.GetComponent<NodeTutorial>();

        if (nodeScript.isCleared)
        {
            Manager_Audio.PlaySound(Manager_Audio.play_clickClearedNode, gameObject);
            return;
        }

        invisPanel.SetActive(true);

        PopUpPanel.SetActive(true);
        btnPlay.GetComponent<Button>().onClick.RemoveAllListeners();
        btnScout.GetComponent<Button>().onClick.RemoveAllListeners();

        btnPlay.GetComponent<Button>().onClick.AddListener(Play);
        btnScout.GetComponent<Button>().onClick.AddListener(Scout);

        btnScout.transform.GetChild(1).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Scout For") + " : " + nodeScript.scoutCost;
        btnPlay.transform.GetChild(1).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Enter For") + " : " + nodeScript.TravelCost;
        tribeText.text = TranslationManager.Instance.GetTranslation("Tribe Camps") + " : " + nodeScript.CampsInNode;
        foodText.text = TranslationManager.Instance.GetTranslation("Food") + " : " + nodeScript.foodAmount;
        scrapsText.text = TranslationManager.Instance.GetTranslation("Scraps") + " : " + nodeScript.scrapAmount;
        itemText.text = nodeScript.itemDropAmount + " " + TranslationManager.Instance.GetTranslation("Items");
        goldTeethsText.text = nodeScript.goldTeethAmount + " " + TranslationManager.Instance.GetTranslation("GoldTeeths");

        NotScoutedPanel.transform.GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;

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

        if (!showScoutPanel)
        {
            btnScout.SetActive(false);
            ScoutedPanel.SetActive(false);
        }
    }

    public void Play()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);

        confirmPanel.SetActive(true);

        int value = GameController.Instance._FOOD - GetComponent<NodeTutorial>().TravelCost;

        confirmPanel.GetComponent<ConfirmPanel>().SetupText(gameObject, "play", value);

        confirmPanel.GetComponent<ConfirmPanel>().btnNo.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmPanel.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.RemoveAllListeners();

        confirmPanel.GetComponent<ConfirmPanel>().btnNo.GetComponent<Button>().onClick.AddListener(Deny);
        confirmPanel.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.AddListener(AcceptPlay);
    }

    public void Scout()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);

        confirmPanel.SetActive(true);

        confirmPanel.GetComponent<ConfirmPanel>().SetupText(gameObject, "scout");

        confirmPanel.GetComponent<ConfirmPanel>().btnNo.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmPanel.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.RemoveAllListeners();

        confirmPanel.GetComponent<ConfirmPanel>().btnNo.GetComponent<Button>().onClick.AddListener(Deny);
        confirmPanel.GetComponent<ConfirmPanel>().btnYes.GetComponent<Button>().onClick.AddListener(AcceptScout);
    }

    public void AcceptScout()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        //EventManager.Instance.TriggerEvent(new ChangeResources(-GetComponent<NodeTutorial>().scoutCost));

        btnScout.SetActive(false);
        NotScoutedPanel.SetActive(false);
        ScoutedPanel.SetActive(true);
        confirmPanel.SetActive(false);

        GetComponent<NodeTutorial>().isScouted = true;

        foreach (RectTransform child in transform)
        {
            if (child.name == "InfoPanel")
            {
                child.gameObject.SetActive(true);
                GetComponent<NodeTutorial>().txtFood.text = GetComponent<NodeTutorial>().foodAmount.ToString();
                GetComponent<NodeTutorial>().txtScraps.text = GetComponent<NodeTutorial>().scrapAmount.ToString();
                GetComponent<NodeTutorial>().txtTribes.text = GetComponent<NodeTutorial>().CampsInNode.ToString();
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
    }

    public void AcceptPlay()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_intoLevel, gameObject);
        //EventManager.Instance.TriggerEvent(new ChangeResources(-GetComponent<NodeTutorial>().TravelCost));

        /*PlayerPrefs.SetInt(StringResources.NodeIdPrefsName, GetComponent<NodeTutorial>().NodeId);
        PlayerPrefs.SetInt(StringResources.LevelDifficultyPrefsName, GetComponent<NodeTutorial>().Level);
        PlayerPrefs.SetInt(StringResources.TribeCampsPrefsName, GetComponent<NodeTutorial>().tribeCamps);
        PlayerPrefs.SetInt(StringResources.FoodAmountPrefsName, GetComponent<NodeTutorial>().foodAmount);
        PlayerPrefs.SetInt(StringResources.ScrapAmountPrefsName, GetComponent<NodeTutorial>().scrapAmount);
        PlayerPrefs.SetInt(StringResources.ItemDropAmountPrefsName, GetComponent<NodeTutorial>().itemDropAmount);

        EventManager.Instance.TriggerEvent(new ChangeResources(daysSurvived: 1));        

        //GameController.Instance.LoadScene("LevelEnterCutscene"); */
    }

    public void Deny()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        confirmPanel.SetActive(false);
    }
    #endregion
}
