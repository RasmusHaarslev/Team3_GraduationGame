using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewspaperPanelScript : MonoBehaviour {

    public Image NewspaperImage;
    public GameObject ContentPanel;

    public void SetNewspaperImage(Sprite newspaper)
    {
        NewspaperImage.sprite = newspaper;
        ContentPanel.SetActive(true);
    }
}
