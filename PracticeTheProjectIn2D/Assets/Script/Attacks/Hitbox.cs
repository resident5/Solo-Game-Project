using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public NewPlayer player;

    [SerializeField] ParticleSystem specialEffect;

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
        Debug.Log("Other tag " + other.gameObject.tag);
        if (other.gameObject.tag == "Enemy")
        {
            Instantiate(specialEffect.gameObject, player.gameObject.transform);
        }
    }

    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(this.gameObject);
    }
}
