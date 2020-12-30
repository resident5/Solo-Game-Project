using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    NewPlayer player { get => NewPlayer.Instance; }

    public float radius = 3f;
    private void Update()
    {
        float dist = Vector2.Distance(player.transform.position, gameObject.transform.position);
        if (dist <= radius)
        {
            player.focusedNPC = GetComponent<NPC>();
        }
        else
        {
            player.focusedNPC = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
