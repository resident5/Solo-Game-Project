using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : NewCharacters
{

	#region Variables

	[Header ("Static")]

	public static int enemyCounter;

	[SerializeField]
	private Collider2D col;

	[SerializeField]
	private Rigidbody2D myRigidBody;

	[SerializeField]
	private float enemyReactionTime;

	[SerializeField]
	private float meleeRange;

	[SerializeField]
	private float rangedRange;

	public bool InMeleeRange
	{
		get {
			if (target != null)
			{
				return Vector2.Distance (transform.position, target.transform.position) <= meleeRange;
			}

			return false;
		}
	}

	public bool InRangedRange
	{
		get {
			if (target != null)
			{
				return Vector2.Distance (transform.position, target.transform.position) <= rangedRange;
			}

			return false;
		}
	}

	private IEnemyState currentState;

	private Canvas healthCanvas;

	#endregion

	public GameObject target;

	public override void Start ()
	{
		base.Start ();

		StateChange (new IdleState ());

		healthCanvas = transform.GetComponentInChildren<Canvas> ();
	}

	void Update ()
	{

		if (!IsDead)
		{
			if (!damaged)
			{
				currentState.StateUpdate ();

				LookAtTarget ();
			}
		}

	}

	public void StateChange (IEnemyState newEnemyState)
	{
		if (currentState != null)
		{
			currentState.End ();
		}
		currentState = newEnemyState;

		currentState.Start (this);

	}

	public void Move ()
	{
		if (!attack)
		{
			animator.SetFloat ("speed", 0f);
			transform.Translate (GetDirection () * (speed * Time.deltaTime));
		}
	}

	public Vector2 GetDirection ()
	{
		return facingRight ? Vector2.right : Vector2.left;
	}

	private void LookAtTarget ()
	{

		if (target != null)
		{
			float xDir = target.transform.position.x - transform.position.x;

			if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
			{
				ChangeDirection ();
			}
		}
	}

	//This is to change the direction of the Enemy Health bar without flipping it
	public override void ChangeDirection ()
	{
		//Get the healthbar canvas attached to this enemy
		Transform tmp = transform.Find ("EnemyHealthBarCanvas").transform;

		//Get the original position before flipping the enemy
		Vector3 pos = tmp.position;

		//Unparent the canvas quickly
		tmp.SetParent (null);

		base.ChangeDirection ();

		//Parent the canvas quickly
		tmp.SetParent (transform);

		//Set the canvas's position to its original
		tmp.position = pos;

	}

	#region OnTrigger Methods

	public override void OnTriggerEnter2D (Collider2D other)
	{
		base.OnTriggerEnter2D (other);
		currentState.OnTriggerEnter (other);
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Projectile")
		{
			Destroy (other.gameObject);
			StateChange (new StunState ());
		}
	}

	#endregion

	public void Death ()
	{
		Debug.Log ("Enemy should be playing dead now");
		animator.SetTrigger ("die");
		enemyCounter--;
		turnOffCollisions ();

	}

	public void PlayerGrab ()
	{
		animator.SetTrigger ("advantage");
	}

	public override void Attack ()
	{
		attackCollider.enabled = !attackCollider.enabled;
		Vector3 tmpPos = attackCollider.transform.position;
		attackCollider.transform.position = new Vector3 (attackCollider.transform.position.x + 0, 01, attackCollider.transform.position.y);
		attackCollider.transform.position = tmpPos;

	}

	public override IEnumerator TakeDamage (int dmg)
	{
		if (!healthCanvas.isActiveAndEnabled)
		{
			healthCanvas.enabled = true;
		}

		if (!IsDead)
		{
			yield return null;

			healthStat.CurrentVal -= 10;
			animator.SetTrigger ("damage");

			if (healthStat.CurrentVal <= 0)
			{
				Death ();
			}
		} 
	}

	private void turnOffCollisions ()
	{
		col.enabled = false;
		//myRigidBody.velocity = Vector2.zero;
		attackCollider.enabled = false;

		myRigidBody.isKinematic = true;

	}
				
}
