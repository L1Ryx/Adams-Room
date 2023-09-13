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
        WalkingDown
    }

    public PlayerState currentState;  // Variable to keep track of the current player state
    private PlayerState lastDirection; // Variable to keep track of the last direction moved
    public float moveSpeed = 5.0f;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ChangeState(PlayerState.IdleForward);  // Initialize with Idle state
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(moveX, moveY);

        if (move.sqrMagnitude > 1)
        {
            move.Normalize();
        }

        // Change state based on movement
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

        rb.velocity = new Vector2(move.x * moveSpeed, move.y * moveSpeed);

        HandleAnim();
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

    // Function to change player state
    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
        // Here you can add code to update animations or other gameplay elements based on state change
    }
}
