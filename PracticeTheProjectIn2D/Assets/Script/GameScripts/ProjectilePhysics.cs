using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class ProjectilePhysics : MonoBehaviour {

	[SerializeField]
	private float speed;

	private Rigidbody2D myRigidbody;

	private Vector2 direction;


	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate(){
		myRigidbody.velocity = direction * speed;

		if (gameObject.tag == "Projectile")
		{
			Debug.Log ("MY FOOD NO!!!");
			Destroy (gameObject, 2f);
		}
	}

	public void Initialize(Vector2 direction){
		this.direction = direction;
	}

	void OnBecameInvisible()
	{
		Destroy (gameObject);
	}



	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Floor")
		{
			myRigidbody.velocity = Vector2.zero;
		}

		if (gameObject.tag == "Projectile" && other.gameObject.name == "InvisWall")
		{
			Destroy (gameObject);
		}
	}

	void Destroy()
	{
		Destroy (gameObject, 3f);
	}
}