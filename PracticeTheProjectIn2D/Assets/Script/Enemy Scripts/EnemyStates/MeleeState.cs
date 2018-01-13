using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState {

	private NewEnemy enemy;

	private float attackTimer;
	private float attackCooldown = 2;
	public bool canAttack = true;

	public void Start(NewEnemy enemy)
	{
		this.enemy = enemy;
	}
	public void StateUpdate ()
	{
		Attack ();

		if (enemy.target == null)
		{
			enemy.StateChange (new IdleState ());
		}

		if (NewPlayer.Instance.stunned)
		{
			enemy.StateChange (new AdvantageState());
		}
			
//		if (enemy.Target != null)
//		{
//			enemy.ChangeState(new )
//		} else
//		{
//			enemy.ChangeState (new IdleState ());
//
//		}

	}
	public void End()
	{

	}
	public void OnTriggerEnter (Collider2D other)
	{

	}

	private void Attack()
	{

		attackTimer += Time.deltaTime;

		if (attackTimer >= attackCooldown) 
		{
			canAttack = true;
			attackTimer = 0;
		}

		if (canAttack)
		{
			canAttack = false;
			enemy.animator.SetTrigger("attack");
		}

	}
}
