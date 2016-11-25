using UnityEngine;
using System.Collections;

public class MaterialControl : MonoBehaviour {
	public Material[] possibleMaterials;
	public SkinnedMeshRenderer mesh;
	void OnEnable () 
	{
		mesh = GetComponentInChildren<SkinnedMeshRenderer> ();
		mesh.sharedMaterial = possibleMaterials[Random.Range(0,possibleMaterials.Length)];
	}

}
