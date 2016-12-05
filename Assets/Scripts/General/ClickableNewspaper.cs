using UnityEngine;
using System.Collections;

public class ClickableNewspaper : ClickableItem
{
    public Sprite pageImage_Danish;
    public Sprite pageImage_English;

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
        if (TranslationManager.Instance.English)
        {
            GameObject.Find("NewspaperPanel").GetComponent<NewspaperPanelScript>().SetNewspaperImage(pageImage_English);
        }
        else
        {
            GameObject.Find("NewspaperPanel").GetComponent<NewspaperPanelScript>().SetNewspaperImage(pageImage_Danish);
        }
    }
}
