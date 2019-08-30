using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedJump : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump(float fall, float jump)
    {
        if (rb.velocity.y <= 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fall - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyboardInputs.Instance.keybinder["JUMP"]))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jump - 1) * Time.deltaTime;
        }
    }
}
