using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{

	private NewEnemy enemy;

	public void Start (NewEnemy enemy)
	{

		this.enemy = enemy;

	}

	public void StateUpdate ()
	{
		//Debug.Log ("Chasing");

		if (enemy.target != null && enemy.InMeleeRange && !NewPlayer.Instance.stunned)
		{
			enemy.StateChange (new MeleeState ());
		} else if (enemy.target != null && !enemy.InMeleeRange)
		{
			LookAtTarget ();
			enemy.Move ();
		} else if (enemy.target != null && enemy.InMeleeRange && NewPlayer.Instance.stunned)
		{
			enemy.Move ();
		}

	}

	public void End ()
	{

	}

	public void OnTriggerEnter2D (Collider2D other)
	{
		Collider2D col = enemy.transform.Find ("Enemy_Sight").GetComponent<BoxCollider2D> ();

		//This should return MagmaDoll and should go to advantage state immediately
		//Debug.Log (col.name);
		if (other.gameObject.tag == "Player" && NewPlayer.Instance.stunned && col.name != "Enemy_Sight")
		{
			//Debug.Log ("HERE I COME");
			enemy.StateChange (new AdvantageState ());
		}
	}

	public void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.layer == 15)
		{
			enemy.ChangeDirection ();
		}


	}

	public void LookAtTarget ()
	{

		if (enemy.target != null)
		{
			float xDir = enemy.target.transform.position.x - enemy.transform.position.x;

			if (xDir < 0 && enemy.facingRight || xDir > 0 && !enemy.facingRight)
			{
				enemy.ChangeDirection ();
			}
		}
	}
		
}
