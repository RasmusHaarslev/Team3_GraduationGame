using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkingEffect : MonoBehaviour {

    public Color lerpedColor;
    public Color beginColor = new Color(1F, 1F, 1F, 1F);
    public Color endColor = new Color(0F, 1F, 0F, 1F);

    public float speed = 1.0f;
  
    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1) / 2.0f;
        GetComponent<Image>().color = Color.Lerp(beginColor, endColor, t);
    }
}
