using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovingObject {

	public EnemySpawner spawner;

	// Update is called once per frame
	void FixedUpdate ()
	{
		base.FixedUpdate ();
		if (transform.position.x < spawner.despawnX)
			Destroy (gameObject);
	}
}
