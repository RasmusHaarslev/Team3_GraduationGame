using UnityEngine;
using System.Collections;

public class MaterialSwitcher : MonoBehaviour {

    public Material standard;
    public Material targeted;
    public SkinnedMeshRenderer meshRenderer;
    private bool target = false;

    void Start()
    {
        //meshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
    
    public void SwitchMaterial()
    {
        target = !target;
        meshRenderer.material = target ? targeted : standard;
    }

}

