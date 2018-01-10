﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

	[SerializeField]
	private NewEnemy enemy;

	void Start()
	{
		enemy = GetComponentInParent<NewEnemy> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			enemy.target = other.gameObject;
			Debug.Log ("Target Acquired: " + other.gameObject.name);
		}
		
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			enemy.target = null;
			Debug.Log ("Lost Target!!!");
		}
	}
}
