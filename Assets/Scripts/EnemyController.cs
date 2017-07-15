using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public EnemySpawner spawner;
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		transform.Translate (Vector3.left * spawner.enemySpeed * Time.fixedDeltaTime);
		if (transform.position.x < spawner.despawnX)
			Destroy (gameObject);
	}
}
