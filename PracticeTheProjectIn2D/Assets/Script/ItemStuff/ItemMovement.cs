using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D myRigid;

	public float move = 20f;
	public float spd = 15f;
	public static int foodammo = 1;
	public static int lifehp = 10;
	public bool moveUp = true;

	// Use this for initialization
	void Start ()
	{
		myRigid = GetComponent<Rigidbody2D> ();

		spd = spd / 100;
	}
	
	// Update is called once per frame
	/*void FixedUpdate ()
	{
		Vector3 theItemVect = transform.localPosition;

		if (theItemVect.y > -1.5)
		{
			//x = 1 * Time.deltaTime;
			//transform.localPosition = x;
			transform.Translate (new Vector2 (0f, -1 * Time.deltaTime));
		} else
		{
			if (theItemVect.y < -1.5)
			{
				//transform.localPosition = -1 * Time.deltaTime;
				//transform.localPosition = x;
				transform.Translate (new Vector2 (0f, 1 * Time.deltaTime));
			}
		}

		/*if (x > -3) 
		{
			moveUp = false;
		}
		if (x < -4.15) {
			moveUp = true;
		}
					
	}

	void OnCollisionEnter(Collision2D other)
	{
		Vector3 theItemVect = transform.localPosition;

		if (other.gameObject.tag == "Floor")
		{
			if (theItemVect.y > -1.5)
			{
				transform.Translate (new Vector2 (0f, -1 * Time.deltaTime));
			} else
			{
				if (theItemVect.y < -1.5)
				{
					transform.Translate (new Vector2 (0f, 1 * Time.deltaTime));
				}
			}
		}

	}*/

	void OnCollisionEnter2D (Collision2D other)
	{
		Vector3 theItemVect = transform.localPosition;
		if (other.gameObject.tag == "Floor")
		{
			myRigid.velocity = Vector2.zero;
			myRigid.rotation = 0;
		}
	}

}
