using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Rigidbody2D rb;

	// Control variables
	public bool goingUp;
	public bool goingDown;
	public bool airborne;

	public bool hasAirJump;
	public bool hasAirDash;
	public bool fastFalling;


	public float timeJumping;
	public float jumpDuration;

	public float fallingSpeed;
	public float fallGravity;
	public float maxFallSpeed;

	public float groundHeight;


	// jumping interpolation
	public float initialHeight;
	public float targetHeight;

	public float jumpPeak;
	public float airJumpHeight;

	// Use this for initialization
	void Start () {
		groundHeight = transform.position.y;
		RefreshAbilities ();
		fastFalling = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			if (airborne)
				JumpTo(transform.position.y + airJumpHeight, 1.0f);
			else
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
				if (transform.position.y < groundHeight) {
					airborne = false;
					goingDown = false;
					fastFalling = false;
					RefreshAbilities ();
				} else {
					fallingSpeed -= fallGravity * fallGravity * Time.fixedDeltaTime;
					if (fallingSpeed < maxFallSpeed)
						fallingSpeed = maxFallSpeed;
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
		fallingSpeed = 0;
	}

	void RefreshAbilities() {
		hasAirJump = true;
		hasAirDash = true;
	}
}
