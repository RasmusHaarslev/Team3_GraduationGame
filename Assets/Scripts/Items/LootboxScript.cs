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

    public bool isTutorial = false;

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

            randomizedAmount = Random.Range((int)(MinimumScrap * ScaleByLevel), (int)(MaxScrap * ScaleByLevel));

            text.text = randomizedAmount + " " + TranslationManager.Instance.GetTranslation("Scraps").ToLower();
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

            if (!isTutorial)
            {
                PlayerPrefs.SetInt("ScrapAmount", PlayerPrefs.GetInt("ScrapAmount") + randomizedAmount);
            }
            else
            {
                EventManager.Instance.TriggerEvent(new ChangeResources(scraps: randomizedAmount));
            }
            PickedUp = true;
        }
    }

    IEnumerator Pickup()
    {
        Manager_Audio.PlaySound("Play_BoxPickup", this.gameObject);
        while (text.color.a > 0.3f)
        {
            text.transform.position += new Vector3(0f, 0.01f, 0f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.005f);
            yield return new WaitForSeconds(0.003f);
        }
        DestroyBox();
    }

    public void DestroyBox()
    {
        Destroy(this.gameObject);
    }
}
