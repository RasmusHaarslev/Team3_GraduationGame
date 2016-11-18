using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpFunctionality : MonoBehaviour {

    public GameObject PopUpPanel;
    public GameObject btnScout;
    public GameObject btnPlay;
    public GameObject invisPanel;

    // Left side shown when you have scouted
    public GameObject LeftPanel;
    public Text wolveText;
    public Text tribeText;
    public Text choiceText;

    // Right side shown when you have scouted
    public GameObject RightPanel;
    public Text foodText;
    public Text coinsText;

    // Not scouted panel
    public GameObject NotScouted;
    public Text interestPointsText;

    //Scout Cost
    public int minimumScoutCost;
    public int maximumScoutCost;
    int scoutCost;

    void OnEnable()
    {
        EventManager.Instance.StartListening<SetupPopUp>(InitialisePopUP);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<SetupPopUp>(InitialisePopUP);
    }

    void Start()
    {
        scoutCost = Random.Range(minimumScoutCost, maximumScoutCost);
    }

    public void InitialisePopUP(SetupPopUp e)
    {
        GameObject node = e.node;
        Node nodeScript = node.GetComponent<Node>();

        if (nodeScript.isCleared)
            return;

        invisPanel.SetActive(true);

        PopUpPanel.SetActive(true);
        btnPlay.GetComponent<Button>().onClick.RemoveAllListeners();
        btnScout.GetComponent<Button>().onClick.RemoveAllListeners();

        btnPlay.GetComponent<Button>().onClick.AddListener(delegate { Play(node); });
        btnScout.GetComponent<Button>().onClick.AddListener(delegate { Scout(node); });

        btnScout.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Scout For") + " : " + scoutCost;
        btnPlay.GetComponent<Text>().text = TranslationManager.Instance.GetTranslation("Enter For") + " : " + nodeScript.TravelCost;
        wolveText.text = TranslationManager.Instance.GetTranslation("Wolve Dens") + " : " + nodeScript.wolveCamps;
        tribeText.text = TranslationManager.Instance.GetTranslation("Tribe Camps") + " : " + nodeScript.tribeCamps;
        choiceText.text = TranslationManager.Instance.GetTranslation("Choice Camps") + " : " + nodeScript.choiceCamps;
        foodText.text = TranslationManager.Instance.GetTranslation("Food") + " : " + nodeScript.foodAmount;
        coinsText.text = TranslationManager.Instance.GetTranslation("Coins") + " : " + nodeScript.coinAmount;
        interestPointsText.text = TranslationManager.Instance.GetTranslation("Interest Points") + " : " + nodeScript.CampsInNode;

        

        if (nodeScript.canPlay && !nodeScript.isCleared)
        {
            if (nodeScript.isScouted)
            {
                btnPlay.SetActive(true);
                // Left Panel
                LeftPanel.SetActive(true);
                // Right Panel
                RightPanel.SetActive(true);
            }
            else
            {
                btnPlay.SetActive(true);
                btnScout.SetActive(true);
                NotScouted.SetActive(true);
            }
        }
        else if (!nodeScript.canPlay)
        {
            if (nodeScript.isScouted)
            {
                // Left Panel
                LeftPanel.SetActive(true);
                // Right Panel
                RightPanel.SetActive(true);
            }
            else
            {
                btnScout.SetActive(true);
                NotScouted.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Show Panel - You have cleared this level");
        }
    }

    public void Play(GameObject node)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        EventManager.Instance.TriggerEvent(new ChangeResources(node.GetComponent<Node>().TravelCost));
        EventManager.Instance.TriggerEvent(new SaveLevelsToXML());

        /*           
        public int Level;
        public int wolveCamps;
        public int tribeCamps;
        public int choiceCamps;
        public int foodAmount;
        public int coinAmount;
        public int itemDropAmount;
        */

        PlayerPrefs.SetInt("NodeId", node.GetComponent<Node>().NodeId);
        PlayerPrefs.SetInt("LevelDifficulty", node.GetComponent<Node>().Level);
        PlayerPrefs.SetInt("WolveCamps", node.GetComponent<Node>().wolveCamps);
        PlayerPrefs.SetInt("TribeCamps", node.GetComponent<Node>().tribeCamps);
        PlayerPrefs.SetInt("ChoiceCamps", node.GetComponent<Node>().choiceCamps);
        PlayerPrefs.SetInt("FoodAmount", node.GetComponent<Node>().foodAmount);
        PlayerPrefs.SetInt("CoinAmount", node.GetComponent<Node>().coinAmount);
        PlayerPrefs.SetInt("ItemDropAmount", node.GetComponent<Node>().itemDropAmount);

        // IF NOT DEBUG USE THIS
        GameController.Instance.LoadScene("LevelEnterCutscene");
        //GameController.Instance.LoadScene(node.GetComponent<Node>().sceneSelection);
        //GameController.Instance.LoadLevel();
    }

    public void Scout(GameObject node)
    {
        Manager_Audio.PlaySound(Manager_Audio.play_menuClick, gameObject);
        EventManager.Instance.TriggerEvent(new ChangeResources(scoutCost));

        btnScout.SetActive(false);
        NotScouted.SetActive(false);

        // Left Panel
        LeftPanel.SetActive(true);

        // Right Panel
        RightPanel.SetActive(true);

        node.GetComponent<Node>().isScouted = true;

        foreach (RectTransform child in node.transform)
        {
           if(child.name == "InfoPanel")
            {
                child.gameObject.SetActive(true);
                node.GetComponent<Node>().txtFood.text = node.GetComponent<Node>().foodAmount.ToString();
                node.GetComponent<Node>().txtCoins.text = node.GetComponent<Node>().coinAmount.ToString();
                node.GetComponent<Node>().txtTribes.text = node.GetComponent<Node>().tribeCamps.ToString();
                node.GetComponent<Node>().txtWolves.text = node.GetComponent<Node>().wolveCamps.ToString();
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }

        EventManager.Instance.TriggerEvent(new SaveLevelsToXML());        
    }
}
