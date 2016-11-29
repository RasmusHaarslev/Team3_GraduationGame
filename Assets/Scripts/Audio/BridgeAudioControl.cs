using UnityEngine;
using System.Collections;

public class BridgeAudioControl : MonoBehaviour {

	bool isSoundChanged = false;

	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag("Player"))
		{
			if(!isSoundChanged)
			{
				isSoundChanged = true;
				Manager_Audio.ChangeState (Manager_Audio.bridgeState,Manager_Audio.bridgeOn);
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.CompareTag("Player"))
		{
			if(isSoundChanged)
			{
				isSoundChanged = false;
				Manager_Audio.ChangeState (Manager_Audio.bridgeState,Manager_Audio.bridgeOff);
			}
		}
	}
}
