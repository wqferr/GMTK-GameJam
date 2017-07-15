using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	
	public GameObject enemyPrefab;
	public GameObject enemyDeathFX;
	public PlayerController player;

	public float spawnRadius;
	public float despawnX;
	public float baseSpawnInterval;

	private float timeToSpawn;

	// Use this for initialization
	void Start () {
		timeToSpawn = 1;
	}

	// Update is called once per frame
	void Update () {
		timeToSpawn -= Time.deltaTime;
		if (timeToSpawn < 0) {
			timeToSpawn = baseSpawnInterval * (Random.value/4 + 1);
			Spawn ();
		}
	}

	void Spawn() {
		float r = (2 * Random.value - 1) * spawnRadius;
		GameObject obj = Instantiate (enemyPrefab);
		obj.transform.parent = transform;
		obj.transform.position = transform.position;
		obj.transform.Translate(Vector3.up * r);
		var controller = obj.GetComponent<EnemyController>();
		controller.spawner = this;
		controller.player = player;
		controller.deathFX = enemyDeathFX;
	}
}
