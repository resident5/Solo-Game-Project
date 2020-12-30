using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

	[SerializeField]
	private Collider2D other = null;

	[SerializeField]
	private Collider2D otherC = null;

	private void Awake()
	{
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), other, true);
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), otherC, true);
	}
}
