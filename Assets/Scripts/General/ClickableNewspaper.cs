using UnityEngine;
using System.Collections;

public class ClickableNewspaper : ClickableItem
{
    public Sprite pageImage;

    void OnEnable()
    {
        EventManager.Instance.StartListening<ItemClicked>(Click);
    }

    void OnDisable()
    {
        EventManager.Instance.StopListening<ItemClicked>(Click);
    }

    void OnApplicationQuit()
    {
        this.enabled = false;
    }

    public void Click(ItemClicked e)
    {
        Manager_Audio.PlaySound("Play_PaperPickup", this.gameObject);
        Manager_Audio.ChangeState(Manager_Audio.commandWheelContainer, Manager_Audio.openWheel);
        GameObject.Find("NewspaperPanel").GetComponent<NewspaperPanelScript>().SetNewspaperImage(pageImage);
    }
}
