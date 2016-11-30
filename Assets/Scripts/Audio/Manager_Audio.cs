using UnityEngine;
using System.Collections;

public class Manager_Audio : MonoBehaviour {

    //	public static void PlaySound(string name)
    //	{
    //		AkSoundEngine.PostEvent (name, this.gameObject);
    //	}

    public static bool musicToggle = true;
    public static bool fxToggle = true;
	public static string bridgeState = "Bridge";
	public static string bridgeOn = "OnBridge";
	public static string bridgeOff = "OffBridge";

	//state container
	public static string playStateGroupContainer = "ExploreState";
	public static string winState = "Win";
	public static string loseState = "Lose";
	public static string exploreSnapshot = "Exploring";
	public static string fightSnapshot = "InCombat";

	public static string baseAmbiencePlay = "Play_GroundAmbience";
	public static string baseAmbienceStop = "Stop_GroundAmbience";
	public static string musicExploreStart = "Play_ExploreMusic";
	public static string musicExploreStop = "Stop_ExploreMusic";
	public static string walkTapUISound = "Play_WalkTapUI";

	public static string commandWheelContainer = "CommandWheel";
	public static string openWheel = "On";
	public static string closeWheel = "Off";

	public static string attackMale1 = "Play_MaleWarrior1Attack";
	public static string attackMale2 = "Play_MaleWarrior2Attack";
	public static string attackFemale1 = "Play_FemaleWarrior1Attack";
	public static string attackFemale2 = "Play_FemaleWarrior2Attack";

	public static string deathMale1 = "Play_MaleWarrior1Death";
	public static string deathMale2 = "Play_MaleWarrior2Death";
	public static string deathFemale1 = "Play_FemaleWarrior1Death";
	public static string deathFemale2 = "Play_FemaleWarrior2Death";

	public static string evilAttackMale1 = "Play_EvilMaleWarrior1Attack";
	public static string evilAttackMale2 = "Play_EvilMaleWarrior2Attack";
	public static string evilAttackFemale1 = "Play_EvilFemaleWarrior1Attack";
	public static string evilAttackFemale2 = "Play_EvilFemaleWarrior2Attack";

	public static string evilDeathMale1 = "Play_EvilMaleWarrior1Death";
	public static string evilDeathMale2 = "Play_EvilMaleWarrior2Death";
	public static string evilDeathFemale1 = "Play_EvilFemaleWarrior1Death";
	public static string evilDeathFemale2 = "Play_EvilFemaleWarrior2Death";

	public static string genericHit = "Play_GenericHit";
	public static string shieldHit = "Play_ShieldHit";

	public static string attackSpear = "Play_UseSpear";
	public static string attackShield = "Play_UseShield";
	public static string attackRiffle = "Play_UseRiffle";

	public static string wind1 = "Play_WindDeep1";
	public static string leaderFootStep = "Play_LeaderFootStep";
	public static string footStepLoopStart = "Play_FootStepLoop";
	public static string footStepLoopStop = "Stop_FootStepLoop";

    public static string play_menuClick = "Play_MenuUI";
    public static string play_menuMusic = "Play_MenuMusic";
    public static string stop_menuMusic = "Stop_MenuMusic";

	public static string play_menuAmbience = "Play_MenuAmbience";
    public static string stop_menuAmbience = "Stop_MenuAmbience";

    public static string play_pickShield = "Play_PickShield";
    public static string play_pickSpear = "Play_PickSpear";
    public static string play_pickRiffle = "Play_PickRiffle";

    public static string play_openMap = "Play_OpenMapUI";
    public static string play_scrollMap = "Play_ScrollMap";
    public static string stop_scrollMap = "Stop_ScrollMap";

    public static string play_fadeNode = "Play_NodeFade";
    public static string play_clearMap = "Play_ClearMap";
    public static string play_unlockNewMaps = "Play_UnlockNewMaps";
    public static string play_lostMap = "Play_NonClearMap";
    public static string play_clickClearedNode = "Play_ClickClearedMap";
    public static string play_campUpgrade = "Play_CampUpgrade";
    public static string play_intoLevel = "Play_IntoLevel";

    public static string play_charSel = "Play_CharSel";

    public static string adjustMusicVolume = "MusicVol";
    public static string adjustFXVolume = "SFXVol";
    public static string adjustScrollPitch = "ScrollPitch";

    public static string discoverFriendly = "DiscoverFriendly";
	public static string discoverEnemy = "DiscoverEnemy";
	public static string friendlyDeath = "CombatFriendlyDie";

	public static string CommandUI = "Play_CommandUI";
	public static string HoverCommandUI = "Play_CommandSel";


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
