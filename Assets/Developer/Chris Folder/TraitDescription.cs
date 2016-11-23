using UnityEngine;
using System.Collections;

public class TraitDescription : MonoBehaviour {

	public static string noTrait = "Acts normally";
	public static string codependant = "Attacks the leader's target";
	public static string lowAttentionSpan = "Changes target after each hit";
	public static string loyal = "Attacks foes that attack the leader";
	public static string braveFool = "Never flees from combat";
	public static string fearful = "Flees on low health";
	public static string clingy = "Always follows the leader";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string chooseCombatTraitDescription(CharacterValues characterValues)
	{
		switch (characterValues.combatTrait)
		{
			case CharacterValues.CombatTrait.BraveFool:
				return braveFool;
			case CharacterValues.CombatTrait.Clingy:
				return clingy;
			case CharacterValues.CombatTrait.Fearful:
				return fearful;
			case CharacterValues.CombatTrait.NoTrait:
				return noTrait;
		}
		return "yo mama";
	}

	public string chooseTargetTraitDescription(CharacterValues characterValues)
	{
		switch (characterValues.targetTrait)
		{
			case CharacterValues.TargetTrait.Codependant:
				return codependant;
			case CharacterValues.TargetTrait.LowAttentionSpan:
				return lowAttentionSpan;
			case CharacterValues.TargetTrait.Loyal:
				return loyal;
			case CharacterValues.TargetTrait.NoTrait:
				return noTrait;
		}
		return "yo mama";
	}
}
