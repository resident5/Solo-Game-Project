using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvantageState : IEnemyState
{
	private NewEnemy enemy;

	private float sexTimer;

	private int sexLevel = 1;

	public void Start (NewEnemy enemy)
	{
		enemy.myStuggleBar.SetActive (true);

		enemy.struggleBar.MaxVal = enemy.struggleCap;

		this.enemy = enemy;
	}

	public void StateUpdate ()
	{
		Debug.Log ("IN SEX STATE");

		if (enemy.canSex && NewPlayer.Instance.stunned && !NewPlayer.Instance.beingUsed)
		{
			Sex ();
			enemy.usePlayer = true;
			NewPlayer.Instance.beingUsed = true;
		}
			

	}

	public void End ()
	{
		Debug.Log ("Sex End");

		enemy.usePlayer = false;
		NewPlayer.Instance.beingUsed = false;

	}

	public void OnTriggerEnter2D (Collider2D other)
	{
	}

	private void Sex ()
	{

		if (sexLevel == 1)
		{
			enemy.animator.SetTrigger ("advantage");
		}
		if (sexLevel == 2)
		{
			enemy.animator.SetTrigger ("advantage 2");
		}
		if (sexLevel == 3)
		{
			enemy.animator.SetTrigger ("advantage 3");
		}
	}
		
}
