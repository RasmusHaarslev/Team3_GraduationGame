using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PalleteType
{
	Pallete1 = 1,
	Pallete2 = 2,
	Pallete3 = 3
}

[System.Serializable]
public class PalleteSettings
{
	public PalleteType palleteType;
	public Texture pallete;
	public float exposure=1;
}

public class ControlPallete : MonoBehaviour {
	AmplifyColorEffect _amplifyColorEffect;
	public Texture pallete1,pallete2,pallete3;
	public List<PalleteSettings> palleteSettings;

	void OnEnable()
	{
		_amplifyColorEffect = GetComponent<AmplifyColorEffect> ();
	}

	void Start () 
	{
		ChangeColorPallete (pallete1);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump"))
		{
			ChangeColorPallete (pallete2);
		}
	}

	public void ChangeColorPallete(Texture newPallet)
	{
		if(newPallet != null)
		{
			_amplifyColorEffect.LutTexture = newPallet;
		}
	}

}
