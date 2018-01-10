using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState {

	private NewEnemy enemy;

	private float idleTimer;

	private float idleDuration;

	public void Start(NewEnemy enemy)
	{
		idleDuration = Random.Range (1, 11);
		this.enemy = enemy;
	}

	public void StateUpdate ()
	{
		Idle ();

		if (enemy.target != null)
		{
			enemy.StateChange (new PatrolState ());
		}
	}
	public void End()
	{

	}
	public void OnTriggerEnter (Collider2D other)
	{

	}

	private void Idle()
	{
		enemy.animator.SetFloat ("speed", 0f);

		idleTimer += Time.deltaTime;

		if (idleTimer >= idleDuration) {
			enemy.StateChange (new PatrolState());
		}
	}
}