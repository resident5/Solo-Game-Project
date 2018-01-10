using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Characters : MonoBehaviour {

	[SerializeField]
	protected float maxSpeed;

	[SerializeField]
	protected Stat healthStat;

	protected bool faceRight;

	public SpriteRenderer mySprite;

	public bool Attack { get; set;}
	public bool TakingDamage { get; set;}
	public abstract bool IsDead { get;}
	public Animator MyAnimator{ get; private set;}

	public bool Stunned { get; set;}
	public bool Grappled { get; set;}

	public int stunMeter;
	public int stunMeterMax;

	[SerializeField]
	private List<string> damageSources;

	[SerializeField]
	public EdgeCollider2D attackCollider;

	public virtual void Start () {
		faceRight = true;

		mySprite = GetComponent<SpriteRenderer> ();
		MyAnimator = GetComponent<Animator> ();
		healthStat.Initialize ();

	}

	public void MeleeAttack()
	{
		attackCollider.enabled = !attackCollider.enabled;
		Vector3 tmpPos = attackCollider.transform.position;
		attackCollider.transform.position = new Vector3(attackCollider.transform.position.x + 0,01, attackCollider.transform.position.y);
		attackCollider.transform.position = tmpPos;
	}

	public abstract IEnumerator TakeDamage ();

	public virtual void ChangeDirection()
	{
		faceRight = !faceRight;
		Vector3 thePlayerScale = transform.localScale;
		thePlayerScale.x *= -1;
		transform.localScale = thePlayerScale;
		
	}

	public abstract void Death();

	public virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (damageSources.Contains (other.tag))
		{
			Debug.Log ("Hit by " + other.name);

			StartCoroutine (TakeDamage ());
		}
	}



}