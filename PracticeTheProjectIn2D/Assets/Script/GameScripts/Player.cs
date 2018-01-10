using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void DeadEventHandler ();


public class Player : Characters
{

	#region Player Variables

	private static Player instance;

	public event DeadEventHandler Dead;

	public static Player Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<Player> ();
			}

			return instance;
		}
	}

	public override bool IsDead
	{
		get
		{
			if (healthStat.CurrentVal <= 0)
			{
				OnDead ();
			}
			return healthStat.CurrentVal <= 0;
		}
	}

	public Rigidbody2D MyRigidbody { get; set; }

	public bool Jump { get; set; }

	public bool DoubleJump { get; set;}

	public bool OnGround { get; set; }

	public bool immortal = false;


	[SerializeField]
	private Collider2D col1;

	[SerializeField]
	private Collider2D col2;

	[Header("Player Information")]

	[SerializeField]
	private Transform[] groundPoints;

	[SerializeField]
	private float groundRadius;

	[SerializeField]
	private LayerMask whatIsGround;

	[SerializeField]
	private bool airControl;

	[Header("Player Forces")]

	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private float bounceForce;

	[SerializeField]
	private float dashForce;

	[SerializeField]
	private float buttonInputLag;

	[SerializeField]
	private int rightDashCounter;

	[SerializeField]
	private int leftDashCounter;

	[SerializeField]
	private float knockBack;

	[SerializeField]
	private Vector2 startPos;

	[SerializeField]
	private float immortalTime;

	#endregion

	public override void Start ()
	{
		base.Start ();

		InvokeRepeating ("resetInputLag", 0.1f, buttonInputLag);
		MyRigidbody = GetComponent<Rigidbody2D> ();
		stunMeterMax = 100;
	}

	void Update ()
	{
		if (!TakingDamage && !IsDead)
		{
			if (transform.position.y <= -14f)
			{
				Death ();
			}
		}

		Stun ();

		PlayerInput ();

	}
		
	void FixedUpdate ()
	{
		if (!IsDead)
		{
			if (!TakingDamage)
			{
				if (!Stunned)
				{
					float move = Input.GetAxis ("Horizontal");

					OnGround = IsGrounded ();
	
					PlayerMovement (move);

					Flip (move);
				}
			}
		}
	}

	#region Inputs/Movement

	private void PlayerMovement (float move)
	{

		if (MyRigidbody.velocity.y < 0)
		{
			MyAnimator.SetBool ("land", true);
			Jump = false;
		}

		if (OnGround || airControl)
		{
			MyRigidbody.velocity = new Vector2 (move * maxSpeed, MyRigidbody.velocity.y);
		}

		if ((Jump && MyRigidbody.velocity.y == 0))
		{
			MyRigidbody.AddForce (new Vector2 (0, jumpForce));
		}
		MyAnimator.SetFloat ("speed", Mathf.Abs (move));

	}

	private void PlayerInput ()
	{
		if (Input.GetKeyDown (KeyCode.Z))
		{
			MyAnimator.SetTrigger ("attack");
		}

		if (Input.GetKeyDown (KeyCode.UpArrow))
		{
			MyAnimator.SetTrigger ("jump");
		}

		if (Input.GetKeyDown (KeyCode.RightArrow))
		{
			rightDashCounter += 1;

			if (rightDashCounter > 1)
				MyRigidbody.AddForce (new Vector2(dashForce, 0f));
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow))
		{
			leftDashCounter += 1;

			if (leftDashCounter > 1)
				MyRigidbody.AddForce (new Vector2(-dashForce, 0f));
		}


		if (Input.GetKeyDown (KeyCode.Q))
		{
			healthStat.CurrentVal -= 10;
		}

		if (Input.GetKeyDown (KeyCode.E))
		{
			healthStat.CurrentVal += 10;
		}

		if (Input.GetKeyUp (KeyCode.A))
		{
			TakeDamage ();
		}
	}

	#endregion

	public void resetInputLag()
	{
		leftDashCounter = 0;
		rightDashCounter = 0;
	
	}

	public void OnDead ()
	{
		if (Dead != null)
		{
			Dead ();
		}
	}

	private void Flip (float move)
	{
		if (move > 0 && !faceRight || move < 0 && faceRight)
		{
			ChangeDirection ();
		}
	}

	private bool IsGrounded ()
	{
		if (MyRigidbody.velocity.y <= 0)
		{
			foreach (Transform point in groundPoints)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
				for (int i = 0; i < colliders.Length; i++)
				{
					if (colliders [i].gameObject != gameObject)
					{
						return true;
					}

				}

			}
		}
		return false;
	}

	#region Status Effects

	private IEnumerator IndicateImmortal ()
	{
		while (immortal && !Stunned)
		{

			mySprite.enabled = false;	
			yield return new WaitForSeconds (.1f);
			mySprite.enabled = true;
			yield return new WaitForSeconds (.1f);
		}
	}

	private void Stun()
	{
		if (stunMeter >= stunMeterMax)
		{
			Debug.Log ("Stunned");
			Stunned = true;
			MyAnimator.SetBool ("stunned", true);
		}
	}
		
	#endregion

	#region OnTrigger / Collision Methods

	#endregion

	#region Player Health Changes

	public override void Death ()
	{
		MyAnimator.ResetTrigger ("die");
		MyAnimator.SetTrigger ("idle");
		healthStat.CurrentVal = healthStat.MaxVal;
		transform.position = startPos;
		turnOnCollisions ();
	}

	public override IEnumerator TakeDamage ()
	{
		if (!immortal )
		{
			
			if (!IsDead)
			{
				
				healthStat.CurrentVal -= 10;
				stunMeter += 40;
				MyAnimator.SetTrigger ("damage");

				immortal = true;
				StartCoroutine (IndicateImmortal ());
				yield return new WaitForSeconds (immortalTime);
				immortal = false;


			} else
			{
				MyAnimator.SetLayerWeight (1, 0);
				turnOffCollisions ();
				MyAnimator.SetTrigger ("die");
				attackCollider.enabled = false;
			}	
		}
	}

	#endregion

	#region Turn On/Off Collisions

	private void turnOffCollisions ()
	{
		col1.enabled = false;
		col2.enabled = false;
		MyRigidbody.velocity = Vector2.zero;
		MyRigidbody.isKinematic = true;

	}

	private void turnOnCollisions ()
	{
		col1.enabled = true;
		col2.enabled = true;
		//MyRigidbody.velocity = Vector2.zero;
		MyRigidbody.isKinematic = false;

	}

	#endregion
}
