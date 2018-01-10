using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{

	private NewEnemy enemy;

	private float patrolTimer;

	private float patrolDuration;

	public void Start (NewEnemy enemy)
	{
		patrolDuration = Random.Range (1, 11);
		this.enemy = enemy;
	}

	public void StateUpdate ()
	{
		if (!enemy.InMeleeRange)
		{
			Patrol ();
			enemy.Move ();
		}

		if (enemy.target != null && enemy.InMeleeRange)
		{
			enemy.StateChange (new MeleeState ());
		}


	}

	public void End ()
	{

	}

	public void OnTriggerEnter (Collider2D other)
	{
		if (other.tag == "Edge")
		{
			enemy.ChangeDirection ();
		}
	}

	public void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.layer == 15)
		{
			enemy.ChangeDirection ();
		}
	}

	private void Patrol ()
	{

		patrolTimer += Time.deltaTime;

		if (patrolTimer >= patrolDuration)
		{
			enemy.StateChange (new IdleState ());
		}
	}
		
}
