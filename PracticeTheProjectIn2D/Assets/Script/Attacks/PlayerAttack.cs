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
    private AudioSource audioSource { get => GetComponent<AudioSource>(); }

    void Update()
    {
        if (player.inputActive)
        {
            Attack();
        } 
    }

    void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKeyDown(KeyboardInputs.Instance.keybinder["ATTACK"]))
            {
                player.animator.Play(player.currentAttack.name);
                //0camAnim.Play("shake");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, player.currentAttack.attackRange, whatIsEnemy);

                if (enemiesToDamage.Length > 0)
                {
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        var enemy = enemiesToDamage[i].GetComponent<NewEnemy>();
                        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
                        
                        StartCoroutine(enemy.TakeDamage(player.currentAttack.damage));
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, player.currentAttack.attackRange);
    }
}
