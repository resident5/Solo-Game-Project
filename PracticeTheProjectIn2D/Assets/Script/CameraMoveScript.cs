using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour {

	[SerializeField]
	private float xMin;

	[SerializeField]
	private float xMax;

	[SerializeField]
	private float yMin;

	[SerializeField]
	private float yMax;

	public Transform player;

	// Use this for initialization
	void Start () {
		//player = GameObject.Find ("Player").transform;
	}

	void LateUpdate () {
		transform.position = new Vector3(Mathf.Clamp(player.position.x,xMin,xMax), 
			Mathf.Clamp(player.position.y, yMin,yMax), transform.position.z);

	}
}
