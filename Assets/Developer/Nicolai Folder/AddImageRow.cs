using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AddImageRow : MonoBehaviour {

    /*
     0 - 1 to 2 
     1 - 1 to 3
     2 - 1 to 4
     3 - 2 to 3
     4 - 2 to 4
     5 - 3 to 4
     */
    public List<Sprite> rowImages = new List<Sprite>();

    public void InsertImage(int childrenNodes, int parentNodes)
    {
        Debug.Log(childrenNodes + " " + parentNodes);
        if (parentNodes > childrenNodes)
        {
            GetComponent<RectTransform>().localRotation = Quaternion.Euler(180,0,0);

            if (childrenNodes == 1 && parentNodes == 4)
            {                
                GetComponent<Image>().sprite = rowImages[2];
            }
            else if (childrenNodes == 1 && parentNodes == 3)
            {
                GetComponent<Image>().sprite = rowImages[1];
            }
            else if (childrenNodes == 1 && parentNodes == 2)
            {
                GetComponent<Image>().sprite = rowImages[0];
            }
            else if (childrenNodes == 2 && parentNodes == 3)
            {
                GetComponent<Image>().sprite = rowImages[3];
            }
            else if (childrenNodes == 2 && parentNodes == 4)
            {
                GetComponent<Image>().sprite = rowImages[4];
            }
            else if (childrenNodes == 3 && parentNodes == 4)
            {
                GetComponent<Image>().sprite = rowImages[5];
            }
        } else
        {
            if (parentNodes == 1 && childrenNodes == 4)
            {
                GetComponent<Image>().sprite = rowImages[2];
            }
            else if (parentNodes == 1 && childrenNodes == 3)
            {
                GetComponent<Image>().sprite = rowImages[1];
            }
            else if (parentNodes == 1 && childrenNodes == 2)
            {
                GetComponent<Image>().sprite = rowImages[0];
            }
            else if (parentNodes == 2 && childrenNodes == 3)
            {
                GetComponent<Image>().sprite = rowImages[3];
            }
            else if (parentNodes == 2 && childrenNodes == 4)
            {
                GetComponent<Image>().sprite = rowImages[4];
            }
            else if (parentNodes == 3 && childrenNodes == 4)
            {
                GetComponent<Image>().sprite = rowImages[5];
            }
        }
    }
}
