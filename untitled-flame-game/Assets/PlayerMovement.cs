using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Enum to store player states
    public enum PlayerState
    {
        IdleForward,
        IdleBack,
        IdleLeft,
        IdleRight,
        WalkingLeft,
        WalkingRight,
        WalkingUp,
        WalkingDown,
        Stomping
    }

    [Header("Shockwave Spawn Points")]
    [SerializeField] private Transform shockwaveSpawnPointUp;
    [SerializeField] private Transform shockwaveSpawnPointDown;
    [SerializeField] private Transform shockwaveSpawnPointLeft;
    [SerializeField] private Transform shockwaveSpawnPointRight;

    [Header("Stomp Attack")]
    [SerializeField] private GameObject shockwavePrefabUp;
    [SerializeField] private GameObject shockwavePrefabDown;
    [SerializeField] private GameObject shockwavePrefabLeft;
    [SerializeField] private GameObject shockwavePrefabRight;
    [SerializeField] private float stompCooldown = 1f; // Cooldown time for stomp attack
    private float stompCooldownTimer = 0f;

    public PlayerState currentState;  // Variable to keep track of the current player state
    private PlayerState lastDirection; // Variable to keep track of the last direction moved
    public float moveSpeed = 5.0f;
    private Rigidbody2D rb;
    private Animator anim;

    public float windForce = 20f;
    public bool isUnderWindEffect = false;
    public Vector2 windDirection;

    [Header("StepSFX")]
    [SerializeField] private float stepTime = 0.3f;
    private float nextStepTime;

    [Header("StompSFX")]
    [SerializeField] private float stompVolume = 0.5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ChangeState(PlayerState.IdleForward);  // Initialize with Idle state
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentState != PlayerState.Stomping)
        {
            StompAttack();
        }
    }


    void FixedUpdate()
    {
        if (currentState != PlayerState.Stomping)
        {
            HandleMovement();
            HandleWindEffect();
        }
        else
        {
            rb.velocity = Vector2.zero; // Ensure the player stops moving while stomping
        }

        HandleAnim(); // Update animations based on the current state
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(moveX, moveY);
        if (move.sqrMagnitude > 1)
        {
            move.Normalize();
        }

        ChangeStateBasedOnMovement(moveX, moveY);
        rb.velocity = move * moveSpeed;

        if (IsWalking() && Time.time >= nextStepTime)
        {
            SFXManager.Instance.PlayFootstepSound(transform.position);
            nextStepTime = Time.time + stepTime;
        }
    }

    private void HandleWindEffect()
    {
        if (isUnderWindEffect)
        {
            rb.AddForce(windDirection * windForce);
        }
    }



    private void ChangeStateBasedOnMovement(float moveX, float moveY)
    {
        if (moveX > 0)
        {
            ChangeState(PlayerState.WalkingRight);
            lastDirection = PlayerState.IdleRight;
        }
        else if (moveX < 0)
        {
            ChangeState(PlayerState.WalkingLeft);
            lastDirection = PlayerState.IdleLeft;
        }
        else if (moveY > 0)
        {
            ChangeState(PlayerState.WalkingUp);
            lastDirection = PlayerState.IdleBack;
        }
        else if (moveY < 0)
        {
            ChangeState(PlayerState.WalkingDown);
            lastDirection = PlayerState.IdleForward;
        }
        else
        {
            ChangeState(lastDirection);
        }
    }

    // Check if the player is in a walking state
    private bool IsWalking()
    {
        return currentState == PlayerState.WalkingLeft ||
               currentState == PlayerState.WalkingRight ||
               currentState == PlayerState.WalkingUp ||
               currentState == PlayerState.WalkingDown;
    }

    public void ApplyWindForce(Vector2 direction)
    {
        windDirection = direction;
        isUnderWindEffect = true;
        StartCoroutine(StopWindEffectAfterDuration());
    }

    public void StompAttack()
    {
        ChangeState(PlayerState.Stomping);
        anim.SetBool("isStomping", true);
        anim.SetTrigger("stomp" + GetDirectionSuffix(lastDirection)); // Triggers the correct directional stomp animation
    }

    public void CreateShockwaveFromAnim()
    {
        CreateShockwave();
    }

    public void EndStomp()
    {
        anim.SetBool("isStomping", false);
        ChangeState(lastDirection); // Return to the last idle or movement state
    }

    public void CreateShockwave()
    {
        SFXManager.Instance.PlaySFX(9, stompVolume);
        GameObject shockwavePrefab = GetShockwavePrefab(lastDirection);
        Transform spawnPoint = GetShockwaveSpawnPoint(lastDirection);
        Instantiate(shockwavePrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private Transform GetShockwaveSpawnPoint(PlayerState direction)
    {
        switch (direction)
        {
            case PlayerState.IdleBack:
            case PlayerState.WalkingUp:
                return shockwaveSpawnPointUp;
            case PlayerState.IdleForward:
            case PlayerState.WalkingDown:
                return shockwaveSpawnPointDown;
            case PlayerState.IdleLeft:
            case PlayerState.WalkingLeft:
                return shockwaveSpawnPointLeft;
            case PlayerState.IdleRight:
            case PlayerState.WalkingRight:
                return shockwaveSpawnPointRight;
            default:
                return shockwaveSpawnPointDown; // Default to down if unsure
        }
    }

    private GameObject GetShockwavePrefab(PlayerState direction)
    {
        switch (direction)
        {
            case PlayerState.IdleBack:
            case PlayerState.WalkingUp:
                return shockwavePrefabUp;
            case PlayerState.IdleForward:
            case PlayerState.WalkingDown:
                return shockwavePrefabDown;
            case PlayerState.IdleLeft:
            case PlayerState.WalkingLeft:
                return shockwavePrefabLeft;
            case PlayerState.IdleRight:
            case PlayerState.WalkingRight:
                return shockwavePrefabRight;
            default:
                return shockwavePrefabDown; // Default to down if unsure
        }
    }

    private string GetDirectionSuffix(PlayerState direction)
    {
        switch (direction)
        {
            case PlayerState.IdleBack:
            case PlayerState.WalkingUp:
                return "Up";
            case PlayerState.IdleForward:
            case PlayerState.WalkingDown:
                return "Down";
            case PlayerState.IdleLeft:
            case PlayerState.WalkingLeft:
                return "Left";
            case PlayerState.IdleRight:
            case PlayerState.WalkingRight:
                return "Right";
            default:
                return "Down"; // Default to down if unsure
        }
    }

    IEnumerator StopWindEffectAfterDuration()
    {
        yield return new WaitForSeconds(EventManager.Instance.eventDuration); // 10 seconds duration
        isUnderWindEffect = false;
    }

    public void IncreaseSpeed(float multiplier)
    {
        moveSpeed *= multiplier; // Increase the speed by the given multiplier
    }

    private void HandleAnim()
    {
        switch (currentState)
        {
            case PlayerState.IdleForward:
                anim.SetBool("isWalking", false);
                anim.SetInteger("direction", 0);
                break;
            case PlayerState.WalkingDown:
                anim.SetBool("isWalking", true);
                anim.SetInteger("direction", 0);
                break;
            case PlayerState.IdleBack:
                anim.SetBool("isWalking", false);
                anim.SetInteger("direction", 1);
                break;
            case PlayerState.WalkingUp:
                anim.SetBool("isWalking", true);
                anim.SetInteger("direction", 1);
                break;
            case PlayerState.IdleLeft:
                anim.SetBool("isWalking", false);
                anim.SetInteger("direction", 2);
                break;
            case PlayerState.WalkingLeft:
                anim.SetBool("isWalking", true);
                anim.SetInteger("direction", 2);
                break;
            case PlayerState.IdleRight:
                anim.SetBool("isWalking", false);
                anim.SetInteger("direction", 3);
                break;
            case PlayerState.WalkingRight:
                anim.SetBool("isWalking", true);
                anim.SetInteger("direction", 3);
                break;

        }
    }

    public void ChangeWindForce(float multiplier)
    {
        windForce *= multiplier;
    }

    // Function to change player state
    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
        // Here you can add code to update animations or other gameplay elements based on state change
    }
}
