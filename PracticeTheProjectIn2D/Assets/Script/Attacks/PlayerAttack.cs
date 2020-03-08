using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private NewPlayer player { get => NewPlayer.Instance; }

    private float timeBtwAttack;
    [SerializeField] private float startTimeBtwAttack;

    [SerializeField] private Transform attackPos;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private Animator camAnim;

    [SerializeField] private AudioClip[] sounds;

    float comboStartTime;
    [SerializeField] float comboResetTime = 0.8f;

    private AudioSource audioSource { get => GetComponent<AudioSource>(); }

    void Update()
    {
        if (player.inputActive)
        {
            InitiateAttack();
        }
    }

    void InitiateAttack()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKeyDown(KeyboardInputs.Instance.keybinder["ATTACK"]))
            {
                comboStartTime = Time.time;
                var currentAttack = player.currentAttack;
                player.animator.Play(currentAttack.name);
                //0camAnim.Play("shake");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, currentAttack.attackRange, whatIsEnemy);

                if (enemiesToDamage.Length > 0)
                {
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        var enemy = enemiesToDamage[i].GetComponent<NewEnemy>();
                        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
                        GetComponent<HitStop>().Stop(GameController.Instance.hitStopAmount);
                        float force = player.facingRight ? currentAttack.knockbackForce : -currentAttack.knockbackForce;
                        enemy.myRigidBody.velocity = Knockback(currentAttack.knockback, force);

                        StartCoroutine(enemy.TakeDamage(currentAttack.damage));
                    }
                }
                CycleAttack();

                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }


        if (Time.time - comboStartTime > comboResetTime)
        {
            comboStartTime = 0;
            player.currentAttack = player.startingAttack;
        }
    }

    void CycleAttack()
    {
        if (player.currentAttack.nextAttack != null)
        {
            player.currentAttack = player.currentAttack.nextAttack; // Changes to next attack
        }
        else
        {
            player.currentAttack = player.startingAttack;
            player.currentAttackIndex = 0;
        }
    }

    Vector2 Knockback(Attack.KnockbackDirection knockback, float force)
    {
        switch (knockback)
        {
            case Attack.KnockbackDirection.KnockBack:
                Debug.Log("KNOCKBACK");
                return new Vector2(force, 0f);
            case Attack.KnockbackDirection.KnockUp:
                Debug.Log("KNOCKUP");
                return new Vector2(force/2, force);
            case Attack.KnockbackDirection.Pull:
                Debug.Log("PULL");
                return new Vector2(-force, 0f);
        }

        return new Vector2(0f, 0f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, player.currentAttack.attackRange);
    }
}