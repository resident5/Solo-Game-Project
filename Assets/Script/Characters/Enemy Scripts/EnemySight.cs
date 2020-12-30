using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{

    public NewEnemy enemy;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && enemy.isHorny)
        {
            enemy.target = other.gameObject;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy.target = null;
        }
    }
}
