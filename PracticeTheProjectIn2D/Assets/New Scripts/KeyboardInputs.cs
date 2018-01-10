using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyboardInputs
{

	public string MoveLeftKey;
	public string MoveRightKey;
	public string JumpKey;
	public string CrouchKey;

	public string AttackKey;

	public KeyboardInputs ()
	{
		MoveLeftKey = "a";
		MoveRightKey = "s";
		JumpKey = "w";
		CrouchKey = "s";
		AttackKey = "f";
	}
}
