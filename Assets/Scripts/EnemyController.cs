using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public EnemySpawner spawner;
	public PlayerController player;

	// Update is called once per frame
	void FixedUpdate () {
		float spd = player.dashing ? spawner.dashSpeed : spawner.enemySpeed;
		transform.Translate (Vector3.left * spd * Time.fixedDeltaTime);
		if (transform.position.x < spawner.despawnX)
			Destroy (gameObject);
	}
}
