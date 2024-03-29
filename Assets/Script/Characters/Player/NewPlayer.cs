﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    public int jumpNum = 2;

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
    private float interactRadius = 3f;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private int stunMeter;
    private int stunMeterMax = 100;

    #endregion

    [Space]
    #region Player forces
    [Header("Player Forces")]

    [SerializeField] private float jumpVelocity;

    [SerializeField] private float dashForce;

    #endregion

    [Space]
    #region Inputs
    [Header("Inputs")]

    [SerializeField] private float immortalTime;

    #endregion

    [Space]
    #region Canvas
    public Transform statusParent;
    public GameObject placeholder;

    #endregion

    #endregion

    public GameObject hitbox;

    public Attack startingAttack;

    public Attack currentAttack;
    public int currentAttackIndex;

    public Interactable focusedNPC;

    NewEnemy partner;

    public IState state;

    public bool inputActive = true;

    public bool isBeingUsed;

    public bool tapped;
    public int tapCount;
    public float lastTap;
    public float tapDelay = 0.4f;
    public int maxTaps = 2;

    public bool dashing = false;
    public float dashTime;

    public GameObject pregInfo;
    public Transform infoMarkerTarget;

    public Transform ui;
    public Canvas worldCanvas;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(this.gameObject);

    }

    public override void Start()
    {
        base.Start();

        state = new DefaultState();

        status = new List<IStatusEffects>();

        StatusEffects(null);

        isBeingUsed = false;
    }

    void Update()
    {
        FocusOnInteractable();

        if (inputActive)
        {
            PlayerInput();
        }

        if (isBeingUsed)
        {
            Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 3f);

            foreach (Collider2D o in col)
            {
                if (o.GetComponent<NewEnemy>() != null && o.GetComponent<NewEnemy>().usePlayer)
                {
                    partner = o.GetComponent<NewEnemy>();
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

    void FixedUpdate()
    {
        if (!IsDead || !isStunned || !damaged)
        {
            float move = Input.GetAxis("Horizontal");

            OnGround = IsGrounded();

            PlayerMovement(move);

            ChangeLayerWeight();

            Stun();

        }
    }

    void PlayerInput()
    {
        if(focusedNPC != null)
        Debug.Log("Player's actual focus is + " + focusedNPC.name);

        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("MY X VELOCITY IS " + myRigidbody.velocity.x);

            dashing = true;
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ui = Instantiate(pregInfo, worldCanvas.transform).transform;
        }

        if (Input.GetKeyDown(KeyboardInputs.Instance.keybinder["JUMP"]) && jumpNum > 0 && focusedNPC == null)
        {
            NewPlayer.Instance.jumpNum -= 1;
            animator.SetTrigger("jump");
            myRigidbody.velocity = Vector2.up * jumpVelocity;
        }
        else if (Input.GetKeyDown(KeyboardInputs.Instance.keybinder["JUMP"]) && focusedNPC != null)
        {
            Debug.Log("PLAYER IN INTERACT MODE WITH " + focusedNPC.name);

            if (focusedNPC.gameObject.GetComponent<NPC>())
            {
                Debug.Log("PLAYER SHOULD NOW BE INTERACTING WITH NPC");
                var npc = focusedNPC.gameObject.GetComponent<NPC>();
                npc.Interact();
            }

            //if (focusedNPC.gameObject.GetComponent<Portal>())
            //{
            //    Debug.Log("PLAYER SHOULD NOW BE INTERACTING WITH PORTAL");
            //    var npc = focusedNPC.gameObject.GetComponent<Portal>();
            //    npc.Interact();
            //}

            //if (focusedNPC.gameObject.GetComponent<NPC>())
            //{
            //    Debug.Log("PLAYER SHOULD NOW BE INTERACTING WITH NPC");
            //    var npc = focusedNPC.gameObject.GetComponent<NPC>();
            //    npc.Interact();
            //}

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed space");
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

    }
    void PlayerMovement(float move)
    {
        if (inputActive)
        {
            if (myRigidbody.velocity.y < 0) //if the player is falling set the animation to landing
            {
                animator.SetBool("land", true);
                //jump = false;
            }

            Turn(move);

            if (!dashing)
            {
                myRigidbody.velocity = new Vector2(move * speed, myRigidbody.velocity.y);
                animator.SetFloat("speed", Mathf.Abs(move));
            }
            else
            {
                myRigidbody.velocity = Vector2.right * dashForce * move * Time.deltaTime;
            }
        }

    }
    
    public void StatusEffects(IStatusEffects newStatus)
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
            isStunned = true;
            animator.SetBool("stunned", true);
        }
    }

    void LateUpdate()
    {
        ui.position = infoMarkerTarget.position;
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashTime);
        dashing = false;
    }

    #region Override Methods
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

    //Methods for info

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
                        jumpNum = 2;
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
        while (immortal && !isStunned)
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

    void FocusOnInteractable()
    {
        
        //float dist = Vector2.Distance(transform.position, gameObject.transform.position);
        //if (dist <= radius)
        //{
        //    focusedNPC = this;
        //    //Debug.Log("Player has interacted with this " + gameObject.name);
        //}
        //else
        //{
        //    focusedNPC = null;
        //}
    }

    #endregion

    #region Colliders

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

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
