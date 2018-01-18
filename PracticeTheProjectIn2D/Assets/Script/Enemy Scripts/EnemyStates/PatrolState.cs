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

		patrolDuration = Random.Range (1, 5);
		patrolTimer = 0;

		this.enemy = enemy;

	}

	public void StateUpdate ()
	{

		if (enemy.target != null && enemy.canSex)
		{
			enemy.StateChange (new ChaseState ());
		}

		Patrol ();
		enemy.Move ();



	}

	public void End ()
	{

	}

	public void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Edge")
		{
			enemy.ChangeDirection ();
		}

//		if (other.gameObject.tag == "Player" && NewPlayer.Instance.stunned)
//		{
//			Debug.Log ("HERE I COME");
//			enemy.StateChange (new AdvantageState ());
//		}
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
			enemy.ChangeDirection ();
			enemy.StateChange (new IdleState ());
		}
	}
		
}
