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

	public float meleeRange;

	public bool InMeleeRange
	{
		get
		{
			if (target != null)
			{
				return Vector2.Distance (transform.position, target.transform.position) <= meleeRange;
			}

			return false;
		}
	}

	public IEnemyState currentState;


	#endregion

	public Stat struggleBar;

	public GameObject myStuggleBar;

	private GameObject healthCanvas;

	public float struggleCap = 20;

	public GameObject target;

	public bool usePlayer;

	public bool canSex;
	public float sexCooldown = 8f;
	public float timeStampCD;

	public override void Start ()
	{
		base.Start ();
		StateChange (new IdleState ());

		healthCanvas = transform.GetComponentInChildren<Canvas> ().transform.Find ("EnemyHealth").gameObject;
		myStuggleBar = transform.GetComponentInChildren<Canvas> ().transform.Find ("StruggleBar").gameObject;

		healthCanvas.SetActive (false);
		myStuggleBar.SetActive (false);

	}

	void FixedUpdate ()
	{
		if (!IsDead)
		{
			if (!damaged)
			{
				currentState.StateUpdate ();

				//LookAtTarget ();
			}
		}

		if (!canSex && timeStampCD <= Time.time)
		{
			target = null;

			timeStampCD = Time.time + sexCooldown;

			canSex = true;
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

	#region OnTrigger Methods

	public override void OnTriggerEnter2D (Collider2D other)
	{
		base.OnTriggerEnter2D (other);
		currentState.OnTriggerEnter2D (other);
	}

	#endregion

	#region Do Not Touch Methods

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
		
	//This is to change the direction of the Enemy Health bar without flipping it
	public override void ChangeDirection ()
	{
		//Get the healthbar canvas attached to this enemy
		Transform tmp = transform.Find ("EnemyHealthUI").transform;

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

	public void Death ()
	{
		animator.SetTrigger ("die");
		enemyCounter--;
		turnOffCollisions ();

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
		if (!healthCanvas.activeInHierarchy)
		{
			healthCanvas.SetActive (true);
		}

		if (!IsDead)
		{
			yield return null;

			healthStat.CurrentVal -= dmg;
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

	#endregion
}
