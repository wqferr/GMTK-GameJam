using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovingObject {

	public EnemySpawner spawner;

	// Update is called once per frame
	public override void FixedUpdate ()
	{
		if (spawner.gameController.CurrentState != GameState.RUNNING)
			return;
		base.FixedUpdate ();
		if (transform.position.x < spawner.despawnX)
			Destroy (gameObject);
	}
}
