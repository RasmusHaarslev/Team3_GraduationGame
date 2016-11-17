using UnityEngine;
using System.Collections;

public class Manager_Audio : MonoBehaviour {

    //	public static void PlaySound(string name)
    //	{
    //		AkSoundEngine.PostEvent (name, this.gameObject);
    //	}

    public static bool overAllSoundToggle = true;
    public static float currentOverAllSoundVolume = 1f;
	public static string menuMusic = "Play_MenuMusic0_4";
	public static string wind1 = "Play_WindDeep1";
	public static string leaderFootStep = "Play_LeaderFootStep";
	public static string footStepLoopStart = "Play_FootStepLoop";
	public static string footStepLoopStop = "Stop_FootStepLoop";
    public static string adjustOverallVolume = "MusicVol";

	public static void PlaySound(string name,GameObject objectPos)
	{
		AkSoundEngine.PostEvent (name, objectPos);
	}

	public static void SendParameterValue(string RTPCName,float value)
	{
		AkSoundEngine.SetRTPCValue (RTPCName, value);
	}

	public static void ChangeState(string stateGroupName,string stateName)
	{
		AkSoundEngine.SetState (stateGroupName, stateName);
	}

	public static void SetSwitch(string switchGroupName,string switchName)
	{
		AkSoundEngine.SetState (switchGroupName, switchName);
	}

	public static void MultiPosition(GameObject obj,AkPositionArray positions)
	{
		AkSoundEngine.SetMultiplePositions(obj,positions,(ushort)positions.Count, MultiPositionType.MultiPositionType_MultiDirections);
	}


}
