using System.Collections;
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

    [SerializeField] protected AudioSource soundSource;
    [SerializeField] protected AudioClip[] hitSounds;

    public List<IStatusEffects> status;

    public List<string> damageSourcesTags;
    public List<string> damageTargetsTags;

    public Collider2D attackCollider;

    public virtual void Start()
    {
        facingRight = true;
        soundSource = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthStat.Initialize();
    }

    public virtual void ChangeDirection()
    {
        facingRight = !facingRight; //Set facingright to false (Left)
        Vector3 curScale = transform.localScale; //Take the current scale of the character
        curScale.x *= -1; //Multiply the X value by -1 to flip it (Use this to keep the scale of y and z if they are not equal)
        transform.localScale = curScale; //Set the localscale to the new flipped scale

    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSourcesTags.Contains(other.gameObject.tag))
        {
            NewCharacters attacker = other.transform.parent.GetComponent<NewCharacters>();
            AudioClip clip = attacker.hitSounds[Random.Range(0, hitSounds.Length)];
            attacker.soundSource.PlayOneShot(clip);
            StartCoroutine(TakeDamage(attacker.damage));
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (damageSourcesTags.Contains(other.gameObject.tag))
        {
            NewCharacters attacker = other.transform.parent.GetComponent<NewCharacters>();
            AudioClip clip = attacker.hitSounds[Random.Range(0, hitSounds.Length)];
            attacker.soundSource.PlayOneShot(clip);
            StartCoroutine(TakeDamage(attacker.damage));
        }
    }

    #region All the things both player and enemy should do but differently

    //Methods for determining the damage the character will take
    public abstract IEnumerator TakeDamage(int x);

    //Methods for how much damage the character will deal
    public abstract void Attack();

    #endregion
}

