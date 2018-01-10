using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : NewCharacters
{

	#region Player Variables

	#region Singleton

	private static NewPlayer instance;

	public static NewPlayer Instance
	{
		get {
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<NewPlayer> ();
			}
			return instance;
		}
	}

	#endregion

	#region Colliders

	public Rigidbody2D myRigidbody;

	[SerializeField]
	private Collider2D col1;

	[SerializeField]
	private Collider2D col2;

	#endregion

	#region Booleans

	public bool jump;
	public bool OnGround;
	public bool immortal = false;

	#endregion

	[Space]
	#region Player Info
	[Header ("Player Information")]

	[SerializeField]
	private Transform[] groundPoints;

	[SerializeField]
	private float groundRadius;

	[SerializeField]
	private LayerMask groundMask;

	[SerializeField]
	private int stunMeter;

	#endregion

	[Space]
	#region Player forces
	[Header ("Player Forces")]

	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private float bounceForce;

	[SerializeField]
	private float dashForce;

	[SerializeField]
	private float knockBackForce;

	#endregion

	[Space]
	#region Inputs
	[Header ("Inputs")]
	[SerializeField]
	private float buttonInputLag;

	[SerializeField]
	private int rightDashCounter;

	[SerializeField]
	private int leftDashCounter;

	[SerializeField]
	private float immortalTime;

	#endregion

	#endregion

	//	[SerializeField]
	//	public KeyboardInputs playerKeys;

	public override void Start ()
	{
		base.Start ();

		myRigidbody = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		PlayerInput ();
	
	}

	void FixedUpdate ()
	{
		//You can move if
		if (!IsDead)  //You're not dead
		{
			if (!damaged) //You're not taking damage
			{ 
				if (!stunned) //You're not stunned
				{
					float move = Input.GetAxis ("Horizontal");

					OnGround = IsGrounded ();

					PlayerMovement (move);

					ChangeLayerWeight ();
				}
			}
		}
	
	}

	private void PlayerInput ()
	{
		if (Input.GetKeyDown (KeyCode.UpArrow))
		{
			animator.SetTrigger ("jump");

		}

		if (Input.GetKeyDown (KeyCode.Z))
		{
			animator.SetTrigger ("attack");

		}

//		if (Input.GetKeyDown (KeyCode.RightArrow))
//		{
//			rightDashCounter += 1;
//
//			if (rightDashCounter > 1)
//			{
//				myRigidbody.AddForce (new Vector2 (dashForce, 0f));
//			}
//		}
//
//		if (Input.GetKeyDown (KeyCode.LeftArrow))
//		{
//			leftDashCounter += 1;
//
//			if (leftDashCounter > 1)
//			{
//				myRigidbody.AddForce (new Vector2 (-dashForce, 0f));
//			}
//		}
//

	}

	private void PlayerMovement (float move)
	{
		if (myRigidbody.velocity.y < 0) //if the player is falling set the animation to landing
		{
			animator.SetBool ("land", true);
			jump = false;
		}

		if (jump && myRigidbody.velocity.y == 0) //if the player pressed jump key and they are not moving vertically
		{
			myRigidbody.AddForce (new Vector2 (0, jumpForce));
		}

		if (OnGround) //if the player is on the ground then move
		{
			myRigidbody.velocity = new Vector2 (move * speed, myRigidbody.velocity.y);
		}

		Turn (move);

		animator.SetFloat ("speed", Mathf.Abs (move));


	}

	private bool IsGrounded ()
	{
		if (myRigidbody.velocity.y <= 0)
		{
			foreach (Transform point in groundPoints)
			{
				//Get collider information for each transform groundpoint
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, groundMask);

				//Loop through each collider
				for (int i = 0; i < colliders.Length; i++)
				{
					//If the collider collided with anything thats not itself return true
					if (colliders [i].gameObject != gameObject)
					{
						return true;
					}
				}
			}
		}

		return false;

	}

	private void Turn (float move)
	{
		if (move > 0 && !facingRight || move < 0 && facingRight)
		{
			ChangeDirection ();
		}
	}

	private IEnumerator IndicateImmortal ()
	{
		while (immortal && !stunned)
		{
			sprite.enabled = false;	
			yield return new WaitForSeconds (.1f);
			sprite.enabled = true;
			yield return new WaitForSeconds (.1f);
		}
	}

	private void Respawn ()
	{
		//Reset die animation
		animator.ResetTrigger ("die");

		//Set idle animation
		animator.SetTrigger ("idle");

		//Max out health
		healthStat.CurrentVal = healthStat.MaxVal;

		//Turn back on the colliders to continue colliding
		turnOnColliders ();

	}

	void ChangeLayerWeight ()
	{
		if (!OnGround)
		{
			animator.SetLayerWeight (1, 1);
		} else
		{
			animator.SetLayerWeight (1, 0);
		}
	}

	private void Death ()
	{
		//Activate the new animation layer
		animator.SetLayerWeight (1, 0);

		//Change the animation to the die
		animator.SetTrigger ("die");

		//Turn off colliders
		turnOffColliders ();
	}

	public override void Attack ()
	{
		attackCollider.enabled = !attackCollider.enabled;
		Vector3 tmpPos = attackCollider.transform.position;
		attackCollider.transform.position = new Vector3(attackCollider.transform.position.x + 0,01, attackCollider.transform.position.y);
		attackCollider.transform.position = tmpPos;

	}

	public override IEnumerator TakeDamage (int x)
	{
		//If your getting hit but your not immortal
		if (!immortal)
		{
			//If you got hit but your not dead or immortal
			if (!IsDead)
			{
				//Determine how much damage the player will take
				healthStat.CurrentVal -= 10;

				//Increase stunmeter with each hit
				stunMeter += 40;

				//Animate after the hit
				animator.SetTrigger ("damage");

				//Set immortal to true to avoid getting spammed
				//Make the player sprite flash on and off for the duration of their invuln

				immortal = true;
				StartCoroutine (IndicateImmortal ());
				yield return new WaitForSeconds (immortalTime);
				immortal = false;

			} else //Otherwise if your not immortal but you are dead
			{
				Death ();
			}
		}
	}

	#region Colliders

	private void turnOffColliders ()
	{
		col1.enabled = false;
		col2.enabled = false;
		attackCollider.enabled = false;

		myRigidbody.velocity = Vector2.zero;
		myRigidbody.isKinematic = true;

	}

	private void turnOnColliders ()
	{
		col1.enabled = true;
		col2.enabled = true;
		myRigidbody.isKinematic = false;

	}

	#endregion
}
