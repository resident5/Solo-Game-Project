using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public NewCharacters attacker;

    public int numberOfHits;

    public int damage;

    BoxCollider2D Collider { get; set; }

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        StartCoroutine(DestroyAfter());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            var victim = other.GetComponent<NewCharacters>();

            if (victim != null)
            {
                if (victim.damageSourcesTags.Contains(other.gameObject.tag))
                {
                    victim.TakeDamage(attacker.damage);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            var victim = other.gameObject.GetComponent<NewCharacters>();

            if (victim != null)
            {
                if (victim.damageSourcesTags.Contains(other.gameObject.tag))
                {
                    victim.TakeDamage(attacker.damage);
                }
            }
        }
    }

    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(this.gameObject);
    }
}
