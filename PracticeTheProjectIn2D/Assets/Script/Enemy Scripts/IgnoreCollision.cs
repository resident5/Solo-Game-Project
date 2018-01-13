using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

	[SerializeField]
	private Collider2D other, otherC,playerSwordCollider;

	private void Awake()
	{
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), other, true);
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), otherC, true);
	}
}
