using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Characters {

	#region Variables
	[Header("Static")]

	public static int enemyCounter;

	[Space]
	[Header("Private")]

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

	private IEnemyState currentState;

	private Canvas healthCanvas;

	public bool InMeleeRange
	{
		get
		{
			if (Target != null)
			{
				return Vector2.Distance (transform.position, Target.transform.position) <= meleeRange;
			}

			return false;
		}
	}


	public bool InRangedRange
	{
		get
		{
			if (Target != null)
			{
				return Vector2.Distance (transform.position, Target.transform.position) <= rangedRange;
			}

			return false;
		}
	}


	public override bool IsDead
	{
		get
		{
			return healthStat.CurrentVal <= 0;
		}
	}


	public GameObject Target { get; set;} 

	#endregion

	public override void Start(){
		base.Start ();
		ChangeState (new IdleState());
		enemyCounter++;

		healthCanvas = transform.GetComponentInChildren<Canvas> ();
	}
		
	void Update () {
		
		if (!IsDead)
		{
			if (!TakingDamage)
			{
				currentState.Execute ();

				LookAtTarget ();
			}
		}
			
	}

	public void ChangeState(IEnemyState newState)
	{
		if (currentState != null) {
			currentState.Exit ();
		}

		currentState = newState;

		currentState.Enter (this);
	}

	#region SetTrigger Methods

	public void Move()
	{
		if (!Attack)
		{
			MyAnimator.SetFloat ("speed", 0f);
			transform.Translate (GetDirection () * (maxSpeed * Time.deltaTime));
		}
	}

	#endregion

	public Vector2 GetDirection()
	{
		return faceRight ? Vector2.right : Vector2.left;
	}

	private void LookAtTarget()
	{

		if (Target != null)
		{
			float xDir = Target.transform.position.x - transform.position.x;

			if (xDir < 0 && faceRight || xDir > 0 && !faceRight)
			{
				ChangeDirection ();
			}
		}
	}

	public override void ChangeDirection()
	{
		Transform tmp = transform.Find("EnemyHealthBarCanvas").transform;
		Vector3 pos = tmp.position;
		tmp.SetParent(null);

		base.ChangeDirection ();

		tmp.SetParent (transform);

		tmp.position = pos;

	}

	#region OnTrigger Methods

	public override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D (other);
		currentState.OnTriggerEnter (other);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Projectile") {
			Destroy (other.gameObject);
			ChangeState(new StunState ());
		}
	}

	#endregion

	public override void Death()
	{
		MyAnimator.SetTrigger ("die");
		attackCollider.enabled = false;
	}

	public void playerGrab()
	{
		MyAnimator.SetTrigger ("advantage");
	}
		
	public override IEnumerator TakeDamage()
	{
		if (!healthCanvas.isActiveAndEnabled)
		{
			healthCanvas.enabled = true;
		}

		if (!IsDead) {
			yield return new WaitForSeconds (.1f);

			healthStat.CurrentVal -= 10;
			//Debug.Log ("Enemy OUCH Damage");
			MyAnimator.SetTrigger ("damage");

		} else {
			yield return new WaitForSeconds (.1f);
			turnOffCollisions ();
			Death ();
			enemyCounter--;

		}

	}

	#region Things I may not need anymore


	private void turnOffCollisions()
	{
		col.enabled = false;
		//myRigidBody.velocity = Vector2.zero;
		myRigidBody.isKinematic = true;

	}

	#endregion
}
