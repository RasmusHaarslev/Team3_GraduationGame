using UnityEngine;
using System.Collections;

public class Manager_Audio : MonoBehaviour {

    //	public static void PlaySound(string name)
    //	{
    //		AkSoundEngine.PostEvent (name, this.gameObject);
    //	}

    public static bool musicToggle = true;
    public static bool fxToggle = true;

	public static string baseAmbiencePlay = "Play_GroundAmbience";
	public static string baseAmbienceStop = "Stop_GroundAmbience";
	public static string musicExploreStart = "Play_ExploreMusic";
	public static string musicExploreStop = "Stop_ExploreMusic";
	public static string walkTapUISound = "Play_WalkTapUI";
	public static string playStateGroupContainer = "ExploreState";
	public static string exploreSnapshot = "Exploring";
	public static string fightSnapshot = "inCombat";

	public static string commandWheelContainer = "CommandWheel";
	public static string openWheel = "On";
	public static string closeWheel = "Off";

	public static string attackMale1 = "Play_WarriorMale1Attack";
	public static string attackMale2 = "Play_WarriorMale2Attack";
	public static string attackFemale1 = "Play_WarriorFemale1Attack";
	public static string attackFemale2 = "Play_WarriorFemale2Attack";

	public static string deathMale1 = "Play_WarriorMale1Death";
	public static string deathMale2 = "Play_WarriorMale2Death";
	public static string deathFemale1 = "Play_WarriorFemale1Death";
	public static string deathFemale2 = "Play_WarriorFemale2Death";

	public static string genericHit = "Play_GenericHit";

	public static string attackSpear = "Play_UseSpear";
	public static string attackShield = "Play_UseShield";
	public static string attackRiffle = "Play_UseRiffle";


	public static string wind1 = "Play_WindDeep1";
	public static string leaderFootStep = "Play_LeaderFootStep";
	public static string footStepLoopStart = "Play_FootStepLoop";
	public static string footStepLoopStop = "Stop_FootStepLoop";

    public static string play_menuClick = "Play_MenuUI";
    public static string play_menuMusic = "Play_MenuMusic";
    public static string play_menuAmbience = "Play_MenuAmbience";
    public static string stop_menuMusic = "Stop_MenuMusic";
    public static string stop_menuAmbience = "Stop_MenuAmbience";

    public static string adjustMusicVolume = "MusicVol";
    public static string adjustFXVolume = "SFXVol";

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
