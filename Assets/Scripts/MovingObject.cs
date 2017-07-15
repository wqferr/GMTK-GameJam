﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

	public PlayerController player;

	public void FixedUpdate () {
		transform.Translate (Vector3.left * player.hspeed * Time.fixedDeltaTime);
	}
}
