using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedJump : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float jumpMultiplier = 2f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.velocity.y < 0)
        {
            //Debug.Log("Falling");
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y >= 0 && !Input.GetKey(KeyboardInputs.Instance.keybinder["JUMP"]))
        {
            //Debug.Log("Jumping");
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
