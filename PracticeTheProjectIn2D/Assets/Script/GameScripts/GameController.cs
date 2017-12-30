using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject enemy, food, health;

	public int enemySpawnPercent = 5;

	public float time = 0;

	public static GameController Instance
	{
		get
		{
			if (instance == null) {
				instance = FindObjectOfType<GameController> ();
			}
			return instance;
		}
	}



	public GameObject ItemPrefab
	{
		get {
			return itemPrefab;
		}
	}

	[SerializeField]
	private GameObject[] locations;

	private static GameController instance;

	[SerializeField]
	private Enemy enemyprefabs;

//	[SerializeField]
//	private GameObject edge1;

	[SerializeField]
	private GameObject itemPrefab;

	void Start(){
		
	}

	void Update()
	{
//		if (Enemy.enemyCounter < 3)
//		{
//			Instantiate (enemyprefabs, edge1);
//		}
	}

	void FixedUpdate(){
		GameObject chosenSpot = locations [Random.Range (0, locations.Length)];

		if (Enemy.enemyCounter < 6)
		{
			Instantiate (enemyprefabs, new Vector3(chosenSpot.transform.position.x,chosenSpot.transform.position.y + 5), Quaternion.identity);
			Debug.Log ("Spawning Enemy number: " + Enemy.enemyCounter);

		}
	}

	void Spawn (){
	}

}
