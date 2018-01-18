using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : IEnemyState {
	private NewEnemy enemy;

	private float stunTimer;

	private float stunDuration = 1f;

	public void Start(NewEnemy enemy)
	{
		this.enemy = enemy;
	}

	public void StateUpdate ()
	{
		Debug.Log("STUNNED");

		Stun ();
	}
	public void End()
	{

	}
	public void OnTriggerEnter2D (Collider2D other)
	{

	}

	private void Stun()
	{
		enemy.animator.SetFloat ("speed", 0f);

		stunTimer += Time.deltaTime;

		if (stunTimer >= stunDuration) {
			enemy.StateChange (new PatrolState());
		}
	}
}
