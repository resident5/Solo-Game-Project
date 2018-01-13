using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState {

	void Start(NewEnemy enemy);
	void StateUpdate ();
	void End();
	void OnTriggerEnter (Collider2D other);

}
