using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
	DASHING,
	FASTFALLING,
	RISING,
	FALLING,
	GROUNDED,
	HIT
}

public class PlayerController : MonoBehaviour
{
	#region REFERENCES
	public GameController gameController;

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

	[HideInInspector]
	public float hspeed;
	public float hacc;
	public bool accelerating;

	// jumping interpolation
	private float initialHeight;
	private float targetHeight;

	private int health;

	#region TWEAKABLES
	public int startingHealth;

	public float normalHSpeed;
	public float dashHSpeed;
	public float jumpHSpeed;
	public float fallHSpeed;

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

	private float initialX;
	private float targetX;

	private bool recoiling;
	private bool recovering;
	private bool hitstun;

	public float hitKnockbackHeight;
	public float hitKnockbackDuration;
	public float hitHSpeed;
	public float hitHKnockback;
	public float hitRecoverSpeed;
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
			hitstun = false;
			RefreshAbilities ();
			if (!accelerating && previousState != PlayerState.FASTFALLING)
				hspeed = normalHSpeed;
			else
				accelerating = true;
			transform.position = new Vector3 (transform.position.x, groundHeight, transform.position.z);
			renderer.sprite = normalSprite;
			score.EndCombo ();

			break;
		case PlayerState.RISING: //jump
			if (currentState != PlayerState.DASHING) {
				hspeed = jumpHSpeed;
				accelerating = true;
			}
			hasJump = false;

			break;
		case PlayerState.FALLING: //fall
			StartFalling ();
			break;
		case PlayerState.FASTFALLING: //fastFall
			StartFastFalling ();
			hspeed = fallHSpeed;
			break;
		case PlayerState.DASHING:
			Dash ();
			if(currentState != PlayerState.GROUNDED)
				hasDash = false;
			break;
		case PlayerState.HIT:
			DecreaseHealth (1);
			recoiling = true;
			hitstun = true;

			targetX = initialX - hitHKnockback;

			JumpTo (transform.position.y + hitKnockbackHeight, hitKnockbackDuration);
			nextState = PlayerState.RISING;
			break;
		default:
			break;
		}

		previousState = currentState;
		currentState = nextState;
	}

	public void Die()
	{
		//PlayDeathAnimation
		gameController.PlayerDied();
	}

	public void DecreaseHealth(int amount)
	{
		health -= amount;
		if(health <= 0)
			Die ();
	}

	public void IncreaseHealth(int amount)
	{
		health += amount;
		if(health > startingHealth)
			startingHealth = health;
	}

	// Use this for initialization
	void Start ()
	{
		health = startingHealth;
		initialX = transform.position.x;
		groundHeight = transform.position.y;
		RefreshAbilities ();
		renderer.sprite = normalSprite;
		hspeed = normalHSpeed;
	}

	// Update is called once per frame
	void Update ()
	{
		if (gameController.CurrentState != GameState.RUNNING)
			return;

		if (!hitstun) {
			if (Input.GetButtonDown ("Fire1")) {
				if (hasJump) {
					if (currentState == PlayerState.GROUNDED || currentState == PlayerState.DASHING && previousState == PlayerState.GROUNDED)
						JumpTo (groundJumpPeak, groundJumpDuration);
					else
						JumpTo (transform.position.y + airJumpHeight, airJumpDuration);
				} else
					SwitchState (PlayerState.FASTFALLING);
			}

			if (Input.GetButtonDown ("Fire2")) {
				if (hasDash)
					SwitchState (PlayerState.DASHING);
			}
		}
	}

	void FixedUpdate()
	{
		if (gameController.CurrentState != GameState.RUNNING)
			return;

		if (accelerating)
		{
			hspeed += hacc * Time.fixedDeltaTime;
			if (hspeed >= normalHSpeed) {
				hspeed = normalHSpeed;
				accelerating = false;
			}
		}
		score.distance += hspeed * Time.fixedDeltaTime;

		if (recoiling) {
			if (transform.position.x > targetX) {
				transform.Translate (Vector3.left * hitHSpeed * Time.fixedDeltaTime);
			} else {
				recoiling = false;
				recovering = true;
			}
		} else if (recovering) {
			if (transform.position.x < initialX) {
				transform.Translate (Vector3.right * hitRecoverSpeed * Time.fixedDeltaTime);
			} else {
				recovering = false;
			}
		}
		
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
				transform.position = new Vector3 (
					transform.position.x,
					groundHeight,
					transform.position.z
				);
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
				transform.position = new Vector3 (
					transform.position.x,
					groundHeight,
					transform.position.z
				);
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
		SwitchState (PlayerState.RISING);
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
		if (currentState == PlayerState.DASHING) {
			accelerating = false;
			hspeed = normalHSpeed;
		}
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
		hspeed = dashHSpeed;
		accelerating = false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (currentState == PlayerState.DASHING) {
			RefreshAbilities ();
			other.gameObject.GetComponent<EnemyController> ().Kill ();
			score.Hit ();
			// TODO maybe increase dash time?
		} else if (currentState == PlayerState.FASTFALLING) {
			other.gameObject.GetComponent<EnemyController> ().Kill ();
			JumpTo (transform.position.y + bounceHeight, bounceDuration);
			hasDash = true;
			score.Hit ();
		}
		else if (!hitstun)
		{
			SwitchState (PlayerState.HIT);
		}
	}
}
