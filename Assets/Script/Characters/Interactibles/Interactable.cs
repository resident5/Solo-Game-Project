using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    bool buttonPressed = false;

    NewPlayer player { get => NewPlayer.Instance; }

    public float radius = 3f;

    public abstract void Interact();

    public void ButtonPressed()
    {
        buttonPressed = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
