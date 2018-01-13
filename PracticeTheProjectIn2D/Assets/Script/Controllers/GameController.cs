using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	#region Singleton
	private static GameController instance;

	public static GameController Instance
	{
		get 
		{ 
			if (instance == null)
			{
				instance = FindObjectOfType<GameController> ();
			}
			return instance;
		}
	}
	#endregion

	bool paused = false;

	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
	}

	void Start ()
	{
		
	}

	void Update ()
	{
		PlayerInputs ();
	}

	void FixedUpdate()
	{
		InputData ();
	}

	void PlayerInputs ()
	{
		if (Input.GetKeyDown (KeyboardInputs.Instance.keybinder ["PAUSE"]))
		{
			paused = !paused;
		}
	}

	void InputData ()
	{
		if (paused)
		{
			Time.timeScale = 0;
		} else
		{
			Time.timeScale = 1;
		}
	}
}
