using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvantageState : IEnemyState {
	private NewEnemy enemy;

	private float sexTimer;

	private float sexCooldown = 9f;

	private int sexLevel = 1;

	private int maxSexLevel = 3;

	public void Start(NewEnemy enemy)
	{
		this.enemy = enemy;
	}

	public void StateUpdate ()
	{
		if (enemy.target != null && NewPlayer.Instance.stunned)
		{
			Sex ();
		}
	}
	public void End()
	{

	}
	public void OnTriggerEnter (Collider2D other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().Stunned)
		{
			Debug.Log ("FUCK HIm");
			Sex ();
			Player.Instance.transform.position = enemy.transform.position;
			Player.Instance.mySprite.enabled = false;
		}
	}

	private void Sex()
	{
		if(sexLevel == 1)
			enemy.animator.SetTrigger ("advantage");

		if(sexLevel == 2)
			enemy.animator.SetTrigger ("advantage 2");

		if(sexLevel == 3)
			enemy.animator.SetTrigger ("advantage 3");
		
	}
}
