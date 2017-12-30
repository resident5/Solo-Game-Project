using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

	[SerializeField]
	private Enemy enemy;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			enemy.Target = other.gameObject;
			Debug.Log ("Target Acquired: " + other.gameObject.name);
		}
		
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			enemy.Target = null;
			Debug.Log ("Lost Target!!!");
		}
	}
}
