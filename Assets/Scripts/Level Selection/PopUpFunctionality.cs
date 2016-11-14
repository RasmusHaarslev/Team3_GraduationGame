using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpFunctionality : MonoBehaviour {

    public GameObject PopUpPanel;
    public GameObject btnScout;
    public GameObject btnPlay;

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

    public void InitialisePopUP(SetupPopUp e)
    {
        GameObject node = e.node;
        Node nodeScript = node.GetComponent<Node>();
        scoutCost = Random.Range(minimumScoutCost, maximumScoutCost);

        PopUpPanel.SetActive(true);
        btnPlay.GetComponent<Button>().onClick.AddListener(delegate { Play(node); });
        btnScout.GetComponent<Button>().onClick.AddListener(delegate { Scout(node); });
        btnScout.GetComponent<Text>().text = "Scout for : " + scoutCost;
        btnPlay.GetComponent<Text>().text = "Enter for : " + nodeScript.TravelCost;
        wolveText.text = "Wolve dens : " + nodeScript.wolveCamps;
        tribeText.text = "Tribe camps : " + nodeScript.tribeCamps;
        choiceText.text = "Choice camps : " + nodeScript.choiceCamps;
        foodText.text = "Food : " + nodeScript.foodAmount;
        coinsText.text = "Coins : " + nodeScript.coinAmount;
        interestPointsText.text = "Interest points : " + nodeScript.CampsInNode;

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
        /*           
        public int Level;
        public int wolveCamps;
        public int tribeCamps;
        public int choiceCamps;
        public int foodAmount;
        public int coinAmount;
        public int itemDropAmount;
        */

        Debug.Log(node.name);
    }

    public void Scout(GameObject node)
    {
        btnScout.SetActive(false);
        NotScouted.SetActive(false);

        // Left Panel
        LeftPanel.SetActive(true);
        wolveText.text = "Wolve dens : " + node.GetComponent<Node>().wolveCamps;
        tribeText.text = "Tribe camps : " + node.GetComponent<Node>().tribeCamps;
        choiceText.text = "Choice camps : " + node.GetComponent<Node>().choiceCamps;
        // Right Panel
        RightPanel.SetActive(true);
        foodText.text = "Food : " + node.GetComponent<Node>().foodAmount;
        coinsText.text = "Coins : " + node.GetComponent<Node>().coinAmount;

        GameController.Instance._FOOD -= scoutCost;

        Debug.Log(node.name);
    }
}
