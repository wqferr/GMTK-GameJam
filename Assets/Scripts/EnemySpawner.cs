using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public float spawnRadius;
	public GameObject enemyPrefab;
	public float enemySpeed;
	public float despawnX;

	public float spawnInterval;
	float timeToSpawn;

	// Use this for initialization
	void Start () {
		timeToSpawn = spawnInterval;
	}

	// Update is called once per frame
	void Update () {
		timeToSpawn -= Time.deltaTime;
		if (timeToSpawn < 0) {
			timeToSpawn = spawnInterval;
			Spawn ();
		}
	}

	void Spawn() {
		float r = (2 * Random.value - 1) * spawnRadius;
		GameObject obj = Instantiate (enemyPrefab);
		obj.transform.parent = transform;
		obj.transform.position = transform.position;
		obj.transform.Translate(Vector3.up * r);
		obj.GetComponent<EnemyController> ().spawner = this;
	}
}
