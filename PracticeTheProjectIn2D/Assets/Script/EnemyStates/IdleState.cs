﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState {

	private Enemy enemy;

	private float idleTimer;

	private float idleDuration;

	public void Enter(Enemy enemy)
	{
		idleDuration = Random.Range (1, 11);
		this.enemy = enemy;
	}

	public void Execute ()
	{
		Idle ();

		if (enemy.Target != null)
		{
			enemy.ChangeState (new PatrolState ());
		}
	}
	public void Exit()
	{

	}
	public void OnTriggerEnter (Collider2D other)
	{

	}

	private void Idle()
	{
		enemy.MyAnimator.SetFloat ("speed", 0f);

		idleTimer += Time.deltaTime;

		if (idleTimer >= idleDuration) {
			enemy.ChangeState (new PatrolState());
		}
	}
}