using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
	DASHING,
	FASTFALLING,
	RISING,
	FALLING,
	GROUNDED
}

public class PlayerController : MonoBehaviour
{
	#region REFERENCES
	public Rigidbody2D rb;
	new public SpriteRenderer renderer;
	public ScoreManager score;

	public Sprite normalSprite;
	public Sprite dashSprite;
	public Sprite fastFallSprite;
	#endregion

	#region ATTRIBUTES
	private PlayerState currentState = PlayerState.GROUNDED;
	private PlayerState previousState = PlayerState.GROUNDED;

	private bool hasJump;
	private bool hasDash;

	private float fallingSpeed;
	private float timeJumping;
	private float groundHeight;
	//private bool fastFalling;

	// jumping interpolation
	private float initialHeight;
	private float targetHeight;

	#region TWEAKABLES
	public float groundJumpPeak;
	public float groundJumpDuration;
	public float airJumpHeight;
	public float airJumpDuration;
	public float bounceHeight;
	public float bounceDuration;

	private float timeDashing;
	public float dashDuration;

	public float jumpDuration;

	public float fallGravity;
	public float maxFallSpeed;
	#endregion
	#endregion

	#region PROPERTIES
	public PlayerState CurrentState
	{
		get { return this.currentState; }
	}
	#endregion

	void SwitchState(PlayerState nextState)
	{
		switch(nextState)
		{
		case PlayerState.GROUNDED:
			
			RefreshAbilities ();
			transform.position = new Vector3 (transform.position.x, groundHeight, transform.position.z);
			renderer.sprite = normalSprite;
			score.EndCombo ();

			break;
		case PlayerState.RISING: //jump
			
			if (currentState == PlayerState.GROUNDED || currentState == PlayerState.DASHING && previousState == PlayerState.GROUNDED)
				JumpTo (groundJumpPeak, groundJumpDuration);
			else
			{
				//we might need other verifications for currentState before jumping
				JumpTo (transform.position.y + airJumpHeight, airJumpDuration);
			}

			hasJump = false;

			break;
		case PlayerState.FALLING: //fall
			StartFalling ();
			break;
		case PlayerState.FASTFALLING: //fastFall
			StartFastFalling ();
			break;
		case PlayerState.DASHING:
			Dash ();
			if(currentState != PlayerState.GROUNDED)
				hasDash = false;
			break;
		default:
			break;
		}

		previousState = currentState;
		currentState = nextState;
	}

	// Use this for initialization
	void Start ()
	{
		groundHeight = transform.position.y;
		RefreshAbilities ();
		renderer.sprite = normalSprite;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Fire1"))
		{
			if(hasJump)
				SwitchState (PlayerState.RISING);
			else
				SwitchState (PlayerState.FASTFALLING);
		}

		if (Input.GetButtonDown("Fire2"))
		{
			if(hasDash)
				SwitchState (PlayerState.DASHING);
		}
	}

	void FixedUpdate()
	{
		switch(currentState)
		{
		case PlayerState.RISING: //jumping

			timeJumping += Time.fixedDeltaTime;
			if (Mathf.Abs (transform.position.y - targetHeight) < 0.1f)
			{
				transform.position = new Vector3 (transform.position.x, targetHeight, transform.position.z);
				SwitchState (PlayerState.FALLING);
			}
			else
			{
				float t = timeJumping / jumpDuration - 1;
				float dh = targetHeight - initialHeight;
				transform.position = new Vector3 (transform.position.x, dh * (t*t*t*t*t + 1) + initialHeight, transform.position.z);
			}

			hasJump = false;

			break;
		case PlayerState.FALLING: //falling

			if (transform.position.y <= groundHeight) // Hit ground
			{
				SwitchState (PlayerState.GROUNDED);
			} 
			else // Still falling
			{
				fallingSpeed -= fallGravity * fallGravity * Time.fixedDeltaTime;
				if (fallingSpeed < maxFallSpeed)
					fallingSpeed = maxFallSpeed;
				transform.Translate (new Vector3 (0.0f, fallingSpeed, 0.0f) * Time.fixedDeltaTime);
			}

			break;
		case PlayerState.FASTFALLING:
			
			if (transform.position.y <= groundHeight) // Hit ground
			{
				SwitchState (PlayerState.GROUNDED);
			} 
			else // Still falling
			{
				fallingSpeed -= fallGravity * fallGravity * Time.fixedDeltaTime;
				if (fallingSpeed < maxFallSpeed)
					fallingSpeed = maxFallSpeed;
				transform.Translate (new Vector3 (0.0f, fallingSpeed, 0.0f) * Time.fixedDeltaTime);
			}

			break;
		case PlayerState.DASHING:
			// TODO update x coordinate
			timeDashing += Time.fixedDeltaTime;
			if (timeDashing > dashDuration)
			{
				SwitchState (PlayerState.FALLING);
			}
			break;
		}
	}

	void JumpTo(float peak, float duration)
	{
		targetHeight = peak;
		initialHeight = transform.position.y;

		timeJumping = 0;
		jumpDuration = duration;

		renderer.sprite = normalSprite;
	}

	void StartFalling()
	{
		fallingSpeed = 0;
		renderer.sprite = normalSprite;
	}

	void StartFastFalling()
	{
		StartFalling ();
		fallingSpeed = maxFallSpeed;
		renderer.sprite = fastFallSprite;
	}

	void RefreshAbilities()
	{
		hasJump = true;
		hasDash = true;
	}

	void Dash()
	{
		fallingSpeed = 0;
		timeDashing = 0;

		renderer.sprite = dashSprite;
		// TODO enable hitbox
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (currentState == PlayerState.DASHING)
		{
			RefreshAbilities();
			Destroy (other.gameObject);
			score.Hit ();
			// TODO implement canceling (CONSIDER MOMENTUM)
		}
		else if (currentState == PlayerState.FASTFALLING)
		{
			Destroy (other.gameObject);
			JumpTo(transform.position.y + bounceHeight, bounceDuration);
			hasDash = true;
			score.Hit ();
		}
	}
}
