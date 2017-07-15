using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

	public uint combo;
	public uint maxCombo;
	public uint enemiesKilled;
	public float distance;

	public void Start() {
		combo = 0;
		maxCombo = 0;
		enemiesKilled = 0;
		distance = 0;
	}

	public void Hit() {
		enemiesKilled++;
		combo++;
	}

	public void EndCombo() {
		// TODO add score
		combo = 0;
		if (combo > maxCombo)
			SetMaxCombo (combo);
	}

	public void SetMaxCombo(uint cmb) {
		maxCombo = cmb;
		// TODO notify player maybe
	}
}
