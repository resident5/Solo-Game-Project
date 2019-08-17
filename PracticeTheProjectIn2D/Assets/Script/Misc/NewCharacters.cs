﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class NewCharacters : MonoBehaviour
{
	public Stat healthStat;

	public float speed;
	public int damage;

	public bool facingRight;

	public SpriteRenderer sprite;

	public bool attack;
	public bool damaged;

	public bool IsDead
	{
		get 
		{
			return healthStat.CurrentVal <= 0;
		}
	}

	public bool stunned;
	public bool grabbled;

	public Animator animator;

    public AudioSource soundSource;
    public AudioClip[] hitSounds;

    public List<IStatusEffects> status;

	[SerializeField]
	private List<Collider2D> damageSources;

	public Collider2D attackCollider;

	public virtual void Start ()
	{
		facingRight = true;
		sprite = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		healthStat.Initialize ();
	}

	public virtual void ChangeDirection ()
	{	
		facingRight = !facingRight; //Set facingright to false (Left)
		Vector3 curScale = transform.localScale; //Take the current scale of the character
		curScale.x *= -1; //Multiply the X value by -1 to flip it (Use this to keep the scale of y and z if they are not equal)
		transform.localScale = curScale; //Set the localscale to the new flipped scale

	}

	public virtual void OnTriggerEnter2D (Collider2D other)
	{
		//other = the object that hit you
		//myhitbox = what the object has to hit

		//other == PlayerSword

		//If I was hit by a damage source 
		if (damageSources.Contains (other))
		{
			NewCharacters attacker = other.transform.parent.GetComponent<NewCharacters> ();
            if(attacker.hitSounds != null)
            {
                soundSource.PlayOneShot(attacker.hitSounds[Random.Range(0, hitSounds.Length)]);
            }
			StartCoroutine (TakeDamage (attacker.damage));
		}
	}

	#region All the things both player and enemy should do but differently

	//Methods for determining the damage the character will take
	public abstract IEnumerator TakeDamage (int x);

	//Methods for how much damage the character will deal
	public abstract void Attack ();

	#endregion
}

