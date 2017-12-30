using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : IEnemyState {
	private Enemy enemy;

	private float stunTimer;

	private float stunDuration = 1f;

	public void Enter(Enemy enemy)
	{
		this.enemy = enemy;
	}

	public void Execute ()
	{
		Debug.Log("STUNNED");

		Stun ();
	}
	public void Exit()
	{

	}
	public void OnTriggerEnter (Collider2D other)
	{

	}

	private void Stun()
	{
		enemy.MyAnimator.SetFloat ("speed", 0f);

		stunTimer += Time.deltaTime;

		if (stunTimer >= stunDuration) {
			enemy.ChangeState (new PatrolState());
		}
	}
}
