﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : NewCharacters
{

    #region Player Variables

    #region Singleton

    private static NewPlayer instance;

    public static NewPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<NewPlayer>();
            }
            return instance;
        }
    }

    #endregion

    #region Colliders

    public Rigidbody2D myRigidbody;

    [SerializeField]
    protected Collider2D col1;

    [SerializeField]
    protected Collider2D col2;

    #endregion

    #region Booleans

    public bool jump;
    public bool OnGround;
    public bool immortal = false;

    #endregion

    [Space]
    #region Player Info
    [Header("Player Information")]

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask groundMask;

    private int stunMeter;
    private int stunMeterMax = 100;

    #endregion

    [Space]
    #region Player forces
    [Header("Player Forces")]

    [SerializeField] private float jumpForce;

    [SerializeField] private float bounceForce;

    [SerializeField] private float dashForce;

    [SerializeField] private float knockBackForce;

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float jumpMultiplier = 2f;


    #endregion

    [Space]
    #region Inputs
    [Header("Inputs")]

    [SerializeField] private float buttonInputLag;

    [SerializeField] private int rightDashCounter;

    [SerializeField] private int leftDashCounter;

    [SerializeField] private float immortalTime;

    #endregion

    [Space]
    #region Canvas
    public Transform statusParent;
    public GameObject placeholder;

    #endregion

    #endregion

    [SerializeField] private AdvancedJump advancedJump;

    public GameObject hitbox;

    public Attack startingAttack;

    public Attack currentAttack;
    public int currentAttackIndex;
    NewEnemy attacker;

    public bool beingUsed;

    public GameObject pregInfo;
    public Transform infoMarkerTarget;

    public Transform ui;
    public Canvas worldCanvas;

    public override void Start()
    {
        base.Start();

        status = new List<IStatusEffects>();

        StatusEffects(null);

        myRigidbody = GetComponent<Rigidbody2D>();

        beingUsed = false;
    }

    void Update()
    {
        PlayerInput();

        if (jump && myRigidbody.velocity.y == 0) //if the player pressed jump key and they are not moving vertically
        {
            Debug.Log("JUMP");
            advancedJump.Jump(fallMultiplier, jumpMultiplier);
            myRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        if (beingUsed)
        {
            Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 3f);

            foreach (Collider2D o in col)
            {
                if (o.GetComponent<NewEnemy>() != null && o.GetComponent<NewEnemy>().usePlayer)
                {
                    attacker = o.GetComponent<NewEnemy>();
                }
            }
        }
    }

    void FixedUpdate()
    {
        //You can move if
        if (!IsDead)  //You're not dead
        {
            if (!damaged) //You're not taking damage
            {
                if (!stunned) //You're not stunned
                {
                    float move = Input.GetAxis("Horizontal");

                    OnGround = IsGrounded();

                    PlayerMovement(move);

                    ChangeLayerWeight();

                    Stun();
                }
            }
        }

        foreach (IStatusEffects stat in status)
        {
            if (stat != null)
            {
                stat.Duration();
            }
        }
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ui = Instantiate(pregInfo, worldCanvas.transform).transform;
        }

        if (Input.GetKeyDown(KeyboardInputs.Instance.keybinder["JUMP"]))
        {
            animator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyboardInputs.Instance.keybinder["ATTACK"]))
        {

            animator.Play(currentAttack.name); // Plays animation
            Attack(); // Creates hitbox
            if (currentAttack.nextAttack != null)
            {
                currentAttack = currentAttack.nextAttack; // Changes to next attack

            }
            else
            {
                currentAttack = startingAttack;
                currentAttackIndex = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (gameObject.GetComponent<Frosted>() == null)
            {
                StatusEffects(gameObject.AddComponent<Frosted>());
            }
            else
            {
                gameObject.GetComponent<Frosted>().stack++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (gameObject.GetComponent<Poisoned>() == null)
            {
                StatusEffects(gameObject.AddComponent<Poisoned>());
            }
            else
            {
                gameObject.GetComponent<Poisoned>().stack++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            stunMeter = 100;

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

    void PlayerMovement(float move)
    {
        if (myRigidbody.velocity.y < 0) //if the player is falling set the animation to landing
        {
            animator.SetBool("land", true);
            jump = false;
        }

        Turn(move);

        myRigidbody.velocity = new Vector2(move * speed, myRigidbody.velocity.y);

        animator.SetFloat("speed", Mathf.Abs(move));


    }

    void StatusEffects(IStatusEffects newStatus)
    {
        foreach (Transform child in statusParent)
        {
            Destroy(child.gameObject);
        }

        status.Add(newStatus);

        foreach (IStatusEffects stat in status)
        {
            if (stat != null)
            {
                stat.Begin(this);
            }
        }
    }

    void Stun()
    {
        if (stunMeter >= stunMeterMax)
        {
            Debug.Log("Stunned");
            stunned = true;
            animator.SetBool("stunned", true);
        }
    }

    void LateUpdate()
    {
        ui.position = infoMarkerTarget.position;
    }

    //void Cancel()
    //{
    //    animator.GetComponent<NewCharacters>().attack = false;
    //    animator.GetComponent<NewCharacters>().attackCollider.enabled = false;
    //}

    #region Override Methods

    public override void Attack()
    {
        GameObject hBox = Instantiate(hitbox, transform);
        Hitbox hurtbox = hBox.GetComponent<Hitbox>();

        if (hurtbox != null)
        {
            hurtbox.attacker = this;
            hurtbox.damage = damage;
        }

        //attackCollider.enabled = true;
        //Vector3 tmpPos = attackCollider.transform.position;
        //attackCollider.transform.position = new Vector3(attackCollider.transform.position.x + 0, 01, attackCollider.transform.position.y);
        //attackCollider.transform.position = tmpPos;
    }



    public override IEnumerator TakeDamage(int dmg)
    {
        //If your getting hit but your not immortal
        if (!immortal)
        {
            //If you got hit but your not dead or immortal
            if (!IsDead)
            {
                //Determine how much damage the player will take
                healthStat.CurrentVal -= dmg;

                //Increase stunmeter with each hit
                stunMeter += 40;

                //Animate after the hit
                animator.SetTrigger("damage");

                //Set immortal to true to avoid getting spammed
                //Make the player sprite flash on and off for the duration of their invuln

                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;

            }
            else //Otherwise if your not immortal but you are dead
            {
                Death();
            }
        }
    }

    #endregion

    #region InfoMethods

    private bool IsGrounded()
    {
        if (myRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                //Get collider information for each transform groundpoint
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, groundMask);

                //Loop through each collider
                for (int i = 0; i < colliders.Length; i++)
                {
                    //If the collider collided with anything thats not itself return true
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }

        return false;

    }

    private void Turn(float move)
    {
        if (move > 0 && !facingRight || move < 0 && facingRight)
        {
            ChangeDirection();
        }
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal && !stunned)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    private void Respawn()
    {
        //Reset die animation
        animator.ResetTrigger("die");

        //Set idle animation
        animator.SetTrigger("idle");

        //Max out health
        healthStat.CurrentVal = healthStat.MaxVal;

        //Turn back on the colliders to continue colliding
        turnOnColliders();

    }

    void ChangeLayerWeight()
    {
        if (!OnGround)
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    private void Death()
    {
        //Activate the new animation layer
        animator.SetLayerWeight(1, 0);

        //Change the animation to the die
        animator.SetTrigger("die");

        //Turn off colliders
        turnOffColliders();
    }

    #endregion

    #region Colliders
    private void turnOffColliders()
    {
        col1.enabled = false;
        col2.enabled = false;
        attackCollider.enabled = false;

        myRigidbody.velocity = Vector2.zero;
        myRigidbody.isKinematic = true;

    }

    private void turnOnColliders()
    {
        col1.enabled = true;
        col2.enabled = true;
        myRigidbody.isKinematic = false;

    }
    #endregion
}
