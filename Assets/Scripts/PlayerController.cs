using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Rigidbody2D rb;

	public bool goingUp;
	public bool goingDown;
	public bool airborne;

	public float timeJumping;
	public float jumpDuration;

	public float timeFalling;
	public float fallDuration;

	public float fallingSpeed;
	public float fallGravity;
	public float maxFallSpeed;

	public float groundHeight;

	// jumping
	public float initialHeight;
	public float targetHeight;

	public float jumpPeak;
	public float airJumpHeight;

	// Use this for initialization
	void Start () {
		groundHeight = transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1") && !airborne) {
			JumpTo(jumpPeak, 0.8f);
		}
	}

	void FixedUpdate() {
		if (airborne) {
			if (goingUp) {
				timeJumping += Time.fixedDeltaTime;
				if (Mathf.Abs (transform.position.y - targetHeight) < 0.1f) {
					transform.position = new Vector3 (
						transform.position.x,
						targetHeight,
						transform.position.z
					);
					StartFalling ();
				} else {
					float t = timeJumping / jumpDuration - 1;
					float dh = targetHeight - initialHeight;
					transform.position = new Vector3 (
						transform.position.x,
						dh * (t*t*t*t*t + 1) + initialHeight,
						transform.position.z
					);
				}
			} else if (goingDown) {
				if (Mathf.Abs (transform.position.y - groundHeight) < 0.1f) {
					airborne = false;
					goingDown = false;
				} else {
					fallingSpeed -= fallGravity * fallGravity * Time.fixedDeltaTime;
					transform.Translate (new Vector3 (0.0f, fallingSpeed, 0.0f) * Time.fixedDeltaTime);
				}
			}
		}
	}

	void JumpTo(float peak, float duration) {
		targetHeight = peak;
		initialHeight = transform.position.y;
		timeJumping = 0;
		jumpDuration = duration;
		airborne = true;
		goingUp = true;
	}

	void StartFalling() {
		goingUp = false;
		goingDown = true;
		timeFalling = 0;
		fallingSpeed = 0;
	}
}
