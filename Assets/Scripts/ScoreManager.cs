using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

	public uint combo = 0;
	public uint maxCombo = 0;
	
	public void Hit() {
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
