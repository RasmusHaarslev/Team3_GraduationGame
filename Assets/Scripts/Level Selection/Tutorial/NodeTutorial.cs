using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class NodeTutorial : MonoBehaviour {

    public GameObject scrollingGrid;

    public GameObject circularProgress;
    public Text txtIntPoints;

    [Tooltip("Removes the panel on the right of the node")]
    public bool showInfoPanel = true;

    [Tooltip("Removes the scouting functionality")]
    public bool showScoutPanel = true;

    public int PaletteNumber = 0;

    public int TravelCost;
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
    public GameObject btnPlay;
    public GameObject invisPanel;

    public GameObject confirmPanel;

    // PopUp Text shown when you have scouted
    public Text foodText;

    // Not scouted panel
    public GameObject NotScoutedPanel;
    public Text interestPointsText;    

    /* ROADS FROM THIS NODE */
    public List<GameObject> Links = new List<GameObject>();

    void OnEnable()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TutorialLevel01":
                SetScrollPosition(0);
                break;
            case "TutorialLevel02":
                SetScrollPosition(1);
                break;
            case "TutorialLevel03":
                SetScrollPosition(2);
                break;
            case "TutorialLevel04":
                SetScrollPosition(3);
                break;
            case "TutorialLevel05":
                SetScrollPosition(4);
                break;
        }
        DoneLevel();
    }

    void OnApplicationQuit()
    {
        this.enabled = false;
    }

    void Start()
    {
        SetupNodes();
    }

    public void SetupNodes()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(ClickNode);
    }

    public void ClickNode()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_openMap, gameObject);
        InitialisePopUP(gameObject);
    }

    public void DoneLevel()
    {
        string scene = SceneManager.GetActiveScene().name;

        int tutLevel = (int)Char.GetNumericValue(scene[scene.Length - 1]);
        string compare = "Tut" + tutLevel;

        if (compare == gameObject.name && compare != "Tut5")
        {
            StartCoroutine(initWin(gameObject));
            EventManager.Instance.TriggerEvent(new ChangeResources(food: gameObject.GetComponent<NodeTutorial>().foodAmount));
        } else if (gameObject.name == "Tut5" && compare == "Tut5")
        {
            EventManager.Instance.TriggerEvent(new ChangeResources(food: gameObject.GetComponent<NodeTutorial>().foodAmount));
            GameController.Instance.LoadScene("CampManagement");
        }

    }

    #region WIN ANIMATION
    IEnumerator initWin(GameObject node)
    {
        List<GameObject> nodeList = Links;

        Manager_Audio.PlaySound(Manager_Audio.play_fadeNode, gameObject);
        node.GetComponent<Animator>().SetTrigger("IsCleared");
        node.GetComponent<NodeTutorial>().isCleared = true;

        yield return new WaitForSeconds(1f);

        SetupUIText();

        foreach (var childNode in nodeList)
        {
            if (!childNode.GetComponent<NodeTutorial>().isCleared && !childNode.GetComponent<NodeTutorial>().isOpen)
            {
                childNode.GetComponent<NodeTutorial>().isOpen = true;
                childNode.GetComponent<NodeTutorial>().canPlay = true;
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
        }
        else if (isScouted)
        {
            foreach (RectTransform child in transform)
            {
                if (child.name == "InfoPanel")
                {
                    child.gameObject.SetActive(true);
                }
            }

        }
        else
        {
            txtIntPoints.text = CampsInNode.ToString();
        }
    }
    #endregion

    #region SCROLL TO
    public void SetScrollPosition(int rowNumber)
    {
        /* SETUP SCROLL POSITION */
        float rowHeight = 384*rowNumber;

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

        btnPlay.GetComponent<Button>().onClick.AddListener(Play);
        btnPlay.transform.GetChild(1).GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Enter For") + " : " + nodeScript.TravelCost;

        NotScoutedPanel.transform.GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;

        if (nodeScript.canPlay && !nodeScript.isCleared)
        {
            if (nodeScript.isScouted)
            {
                btnPlay.SetActive(true);
                btnPlay.GetComponent<RectTransform>().localPosition = new Vector3(0, -462, 0);

            }
            else
            {
                btnPlay.SetActive(true);
                NotScoutedPanel.SetActive(true);
                btnPlay.GetComponent<RectTransform>().localPosition = new Vector3(0, -323, 0);
                NotScoutedPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 150, 0);
            }
        }
        else if (!nodeScript.canPlay)
        {
            if (nodeScript.isScouted)
            {
                btnPlay.SetActive(false);
                NotScoutedPanel.SetActive(false);
            }
            else
            {
                btnPlay.SetActive(false);
                NotScoutedPanel.SetActive(true);
                btnPlay.GetComponent<RectTransform>().localPosition = new Vector3(0, -323, 0);
                NotScoutedPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 150, 0);
            }
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

    public void AcceptPlay() {
        Manager_Audio.PlaySound(Manager_Audio.play_intoLevel, gameObject);
        EventManager.Instance.TriggerEvent(new ChangeResources(-TravelCost));

        PlayerPrefs.SetInt(StringResources.LevelDifficultyPrefsName, PaletteNumber);

        /*  switch (gameObject.name)
          {
              case "Tut1":
                  GameController.Instance.LoadScene("TutorialLevel01");
                  break;
              case "Tut2":
                  GameController.Instance.LoadScene("TutorialLevel02");
                  break;
              case "Tut3":
                  GameController.Instance.LoadScene("TutorialLevel03");
                  break;
              case "Tut4":
                  GameController.Instance.LoadScene("TutorialLevel04");
                  break;
              case "Tut5":
                  GameController.Instance.LoadScene("CampManagement");
                  break;
          } */

        Debug.Log("NODES PALLET : " + PlayerPrefs.GetInt(StringResources.LevelDifficultyPrefsName));
    }

    public void Deny()
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        confirmPanel.SetActive(false);
    }
    #endregion

    #region TRIGGER SOUND
    public void TriggerSound(int changeSound)
    {
        // 0 - Lose
        // 1 - Unlock
        // 2 - Clear
        switch (changeSound)
        {
            case 0:
                Manager_Audio.PlaySound(Manager_Audio.play_lostMap, gameObject);
                break;
            case 1:
                Manager_Audio.PlaySound(Manager_Audio.play_unlockNewMaps, gameObject);
                break;
            case 2:
                Manager_Audio.PlaySound(Manager_Audio.play_clearMap, gameObject);
                break;
        }
    }
    #endregion

}
