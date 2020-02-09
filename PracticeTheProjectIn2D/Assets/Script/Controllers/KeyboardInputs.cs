using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyboardInputs : MonoBehaviour
{
	#region Singletion
	private static KeyboardInputs instance;

	public static KeyboardInputs Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<KeyboardInputs> ();
			}
			return instance;
		}
	}
	#endregion

	public Dictionary<string, KeyCode> keybinder;


	void Start ()
	{
		keybinder = new Dictionary<string, KeyCode> ();

		keybinder.Add ("JUMP",KeyCode.UpArrow);
		keybinder.Add ("LEFT",KeyCode.LeftArrow);
		keybinder.Add ("RIGHT",KeyCode.RightArrow);
		keybinder.Add ("CROUCH",KeyCode.DownArrow);

		keybinder.Add ("ATTACK",KeyCode.Z);

		keybinder.Add ("PAUSE",KeyCode.Escape);


	}

    public void RemoveKeybind(string key)
	{
		keybinder.Remove (key);
		ReplaceKeybind (key);
	}

	private void ReplaceKeybind(string key)
	{
		Event e = new Event ();

		if (e.isKey)
		{
			keybinder.Add (key, e.keyCode);
		}
	}
}
