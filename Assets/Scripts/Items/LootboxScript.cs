using UnityEngine;
using System.Collections;

public class LootboxScript : MonoBehaviour
{
    public int MinimumScrap;
    public int MaxScrap;
    public float ProbabilityOfSpawn;
    public float ScaleByLevel;

    public ParticleSystem glow;
    public ParticleSystem received;

    public TextMesh text;

    private int count;
    private bool PickedUp = false;
    private int randomizedAmount;

    // Use this for initialization
    void Start()
    {
        var spawn = Random.Range(0.0f, 1.0f);
        if (spawn > ProbabilityOfSpawn)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);

            randomizedAmount = Random.Range((int) (MinimumScrap*ScaleByLevel), (int) (MaxScrap * ScaleByLevel));

            text.text = randomizedAmount + " scrap";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!PickedUp && other.tag == "Player")
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
            glow.Stop();
            received.Play();
            StartCoroutine(Pickup());
            PlayerPrefs.SetInt("GainedScrap", PlayerPrefs.GetInt("GainedScrap") + randomizedAmount);
            PickedUp = true;
        }
    }

    IEnumerator Pickup()
    {
        while (text.color.a > 0.01f)
        {
            text.transform.position += new Vector3(0f, 0.01f, 0f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.005f);
            yield return new WaitForSeconds(0.01f);
        }
        DestroyBox();
    }

    public void DestroyBox()
    {
        Destroy(this.gameObject);
    }
}
