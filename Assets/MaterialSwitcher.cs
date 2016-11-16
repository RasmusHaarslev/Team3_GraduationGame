using UnityEngine;
using System.Collections;

public class MaterialSwitcher : MonoBehaviour {

    public Material standard;
    public Material targeted;
    public MeshRenderer meshRenderer;
    private bool target = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    public void SwitchMaterial()
    {
        target = !target;
        meshRenderer.material = target ? targeted : standard;
    }

}

