using UnityEngine;
using System.Collections;

public class CameraDeathEffect : MonoBehaviour
{
    private Material mat;
    public Shader CustomShader;
    public float amount;
    private bool enabled = false;

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mat == null)
        {
            mat = new Material(CustomShader);
            mat.SetFloat("_Amount", 0);
        }

        mat.SetFloat("_Amount", amount);
        Graphics.Blit(source, destination, mat);
    }

    void Update()
    {
        if (enabled && amount < 1.0f) amount += 0.001f;
    }

    public void TriggerDeath()
    {
        enabled = true;
    }
}