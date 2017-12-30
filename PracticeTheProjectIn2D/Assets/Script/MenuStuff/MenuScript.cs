using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MenuScript : MonoBehaviour {

	private float time;

	[SerializeField]
	private Text lastTime;

	[SerializeField]
	private Canvas titleScreen;

	[SerializeField]
	private Canvas instructionScreen;

	void Awake(){
		lastTime.enabled = false;
		titleScreen.enabled = true;
		instructionScreen.enabled = false;
	}

	void Start(){
		Debug.Log ("Starting");
		if (getTimer() > 0)
		{
			lastTime.enabled = true;
			lastTime.text = "You lasted: " + Mathf.Round(getTimer()) + " secs";
		}
	}

	public void goToInstructions()
	{
		titleScreen.enabled = false;
		instructionScreen.enabled = true;
	}

	public void goToGame()
	{
		SceneManager.LoadScene (1);
	}


	public void goToTitleScreen()
	{
		titleScreen.enabled = true;
		instructionScreen.enabled = false;
	}

	private float getTimer()
	{
		return PlayerPrefs.GetFloat ("time");
	}
}
