using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{

	private NewEnemy enemy;

	private float idleTimer;

	private float idleDuration;

	public void Start (NewEnemy enemy)
	{

		idleDuration = Random.Range (2, 6);
		idleTimer = 0;

		this.enemy = enemy;
	}

	public void StateUpdate ()
	{
		Debug.Log ("IN IDLE");

		if (enemy.target != null)
		{
			enemy.StateChange (new PatrolState ());
		}

		Idle ();


	}

	public void End ()
	{

	}

	public void OnTriggerEnter2D (Collider2D other)
	{

	}

	private void Idle ()
	{
		enemy.animator.SetFloat ("speed", 0f);

		idleTimer += Time.deltaTime;

		if (idleTimer >= idleDuration) //After idle ends, start patroling
		{
			enemy.StateChange (new PatrolState ());
		}
	}
}