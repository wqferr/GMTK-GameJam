using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour {

  public Rigidbody2D rb;
  new public SpriteRenderer renderer;
  public Sprite normalSprite;
  public Sprite dashSprite;
  public Sprite fastFallSprite;

  // Control variables
  public bool goingUp;
  public bool goingDown;
  public bool airborne;
  public bool dashing;

  private bool hasAirJump;
  private bool hasAirDash;
  private bool fastFalling;


  public float groundJumpPeak;
  public float groundJumpDuration;
  public float airJumpHeight;
  public float airJumpDuration;
  public float bounceHeight;
  public float bounceDuration;

  private float timeDashing;
  public float dashDuration;

  private float timeJumping;
  public float jumpDuration;

  private float fallingSpeed;
  public float fallGravity;
  public float maxFallSpeed;

  private float groundHeight;
  // jumping interpolation
  private float initialHeight;
  private float targetHeight;
  // Use this for initialization
  void Start () {
    groundHeight = transform.position.y;
    RefreshAbilities ();
    renderer.sprite = normalSprite;
    fastFalling = false;
  }
  // Update is called once per frame
  void Update () {
    if (Input.GetButtonDown ("Fire1")) {
      dashing = false;
      if (airborne) {
        if (hasAirJump) {
          hasAirJump = false;
          JumpTo (transform.position.y + airJumpHeight, airJumpDuration);
        }
        else if (!fastFalling) {
          StartFastFalling ();
        }
      }
      else {
        JumpTo (groundJumpPeak, groundJumpDuration);
      }
    }
    if (Input.GetButtonDown("Fire2")) {
      if (!airborne || hasAirDash) {
        hasAirDash = !airborne;
        Dash ();
      }
    }
  }
  void FixedUpdate() {
    if (dashing) {
      // TODO update x coordinate
      timeDashing += Time.fixedDeltaTime;
      if (timeDashing > dashDuration) {
        dashing = false;
        StartFalling ();
      }
    }
    else if (airborne) {
      if (goingUp) {
        timeJumping += Time.fixedDeltaTime;
        if (Mathf.Abs (transform.position.y - targetHeight) < 0.1f) {
          transform.position = new Vector3 (						transform.position.x,						targetHeight,						transform.position.z					);
          StartFalling ();
        }
        else {
          float t = timeJumping / jumpDuration - 1;
          float dh = targetHeight - initialHeight;
          transform.position = new Vector3 (						transform.position.x,						dh * (t*t*t*t*t + 1) + initialHeight,						transform.position.z					);
        }
      }
      else if (goingDown) {
        if (transform.position.y <= groundHeight) {
          // Hit ground
          airborne = false;
          goingDown = false;
          fastFalling = false;
          transform.position = new Vector3 (						transform.position.x,						groundHeight,						transform.position.z					);
          RefreshAbilities ();
          renderer.sprite = normalSprite;
        }
        else {
          // Still falling
          fallingSpeed -= fallGravity * fallGravity * Time.fixedDeltaTime;
          if (fallingSpeed < maxFallSpeed)						fallingSpeed = maxFallSpeed;
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
    renderer.sprite = normalSprite;
  }
  void StartFalling() {
    goingUp = false;
    goingDown = true;
    fallingSpeed = 0;
    renderer.sprite = normalSprite;
  }
  void StartFastFalling() {
    StartFalling ();
    fastFalling = true;
    fallingSpeed = maxFallSpeed;
    renderer.sprite = fastFallSprite;
  }
  void RefreshAbilities() {
    hasAirJump = true;
    hasAirDash = true;
  }
  void Dash() {
    goingUp = false;
    goingDown = false;
    fastFalling = false;
    fallingSpeed = 0;
    renderer.sprite = dashSprite;
    timeDashing = 0;
    dashing = true;
    // TODO enable hitbox
  }
  void OnTriggerEnter2D(Collider2D other) {
    if (dashing) {
      Destroy (other);
      hasAirDash = true;
      hasAirJump = true;
      // TODO implement canceling (CONSIDER MOMENTUM)
    }
    else if (fastFalling) {
      JumpTo(transform.position.y + bounceHeight, bounceDuration);
      hasAirDash = true;
    }
  }
}
