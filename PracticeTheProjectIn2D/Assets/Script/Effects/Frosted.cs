using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frosted : IStatusEffects
{
	//Dot damage
	private NewCharacters afflicted;

	public float duration = 8f;
	public float timer = 0;
	public float tickRate = 1;

	public float tickCounter;

	public void Start (NewCharacters character)
	{
		afflicted = character;
		afflicted.GetComponent<SpriteRenderer> ().color = Color.blue;
	}

	public void Effect ()
	{
		Debug.Log ("Player Inflicted with Frosted");
		afflicted.healthStat.CurrentVal -= 5;

		tickCounter = 0;
	}

	public void Duration ()
	{
		timer += Time.deltaTime;
		tickCounter += Time.deltaTime;

		Debug.Log ("Timer " + timer);
		//If the timer is pass the duration
		if (timer >= duration)
		{
			//Stop the effect

			Stop ();

			return;
		}

		//If the timer is below the duration
		if (timer < duration)
		{
			if (tickCounter >= tickRate)
			{
				Debug.Log ("DOT");

				Effect ();
			}
		}


	}

	public void Stop ()
	{
		Debug.Log ("Ending Frosted");

		afflicted.status = null;
		afflicted.GetComponent<SpriteRenderer> ().color = Color.white;

	}
}
