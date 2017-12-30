using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState {

	private Enemy enemy;

	private float patrolTimer;

	private float patrolDuration;

	public void Enter(Enemy enemy)
	{
		patrolDuration = Random.Range (1, 11);
		this.enemy = enemy;
	}

	public void Execute ()
	{
		if (!enemy.InMeleeRange)
		{
			Patrol ();
			enemy.Move ();
		}

		if (enemy.Target != null && enemy.InMeleeRange)
		{
			enemy.ChangeState (new MeleeState ());
		}
	}

	public void Exit()
	{

	}
	public void OnTriggerEnter (Collider2D other)
	{
		if (other.tag == "Edge") {
			enemy.ChangeDirection();
		}
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == 15)
		{
			enemy.ChangeDirection ();
		}
	}

	private void Patrol()
	{

		patrolTimer += Time.deltaTime;

		if (patrolTimer >= patrolDuration) {
			enemy.ChangeState (new IdleState());
		}
	}
		
}
