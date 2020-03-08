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

		if (enemy.isHorny && NewPlayer.Instance.isStunned && !NewPlayer.Instance.isBeingUsed)
		{
			Sex ();
			enemy.usePlayer = true;
			NewPlayer.Instance.isBeingUsed = true;
		}
			

	}

	public void End ()
	{
		Debug.Log ("Sex End");
		NewPlayer.Instance.StatusEffects(NewPlayer.Instance.gameObject.AddComponent<Frosted>());
		enemy.usePlayer = false;
		NewPlayer.Instance.isBeingUsed = false;

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
