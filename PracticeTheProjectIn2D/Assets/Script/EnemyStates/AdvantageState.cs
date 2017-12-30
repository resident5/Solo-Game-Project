using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvantageState : IEnemyState {
	private Enemy enemy;

	private float sexTimer;

	private float sexCooldown = 9f;

	private int sexLevel = 1;

	private int maxSexLevel = 3;

	public void Enter(Enemy enemy)
	{
		this.enemy = enemy;
	}

	public void Execute ()
	{
		if (enemy.Target != null && Player.Instance.Stunned)
		{
			Sex ();
		}
	}
	public void Exit()
	{

	}
	public void OnTriggerEnter (Collider2D other)
	{
//		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Characters>().Stunned)
//		{
//			Sex ();
//			Player.Instance.transform.position = enemy.transform.position;
//			Player.Instance.mySprite.enabled = false;
//		}
	}

	private void Sex()
	{
		if(sexLevel == 1)
			enemy.MyAnimator.SetTrigger ("advantage");

		if(sexLevel == 2)
			enemy.MyAnimator.SetTrigger ("advantage 2");

		if(sexLevel == 3)
			enemy.MyAnimator.SetTrigger ("advantage 3");
		
	}
}
