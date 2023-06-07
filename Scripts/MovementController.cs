using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Rigidbody rb;
    public Collider objectCollider;

    public float moveSpeedMultiplier = 0.20f;
    public float jumpForceMultiplier = 5.00f;
    public float maxSpeedHorizontal = 7f;
    public float maxSpeedVertical = 4f;
    public float maxAcceleration = 4f;
    public float slowdown = 0.9f;   

    public bool isOnFloor = false;

    private float distToGround;

    void Start()
    {
        // set the distance between the center of the model to its edge,
        // to determine collisions with the ground.
        this.distToGround = objectCollider.bounds.extents.y;
    }

    void FixedUpdate()
    {
        // check if the character is on the ground - if so, flag it as such, so that it can jump.
        this.isOnFloor = IsGrounded();

        // if instruction is given to jump, and the character is on solid ground, do so.
        if ((Input.GetKey("space") || Input.GetAxis("Vertical") > 0) && this.isOnFloor) { 
            Jump(); 
        }

        // if instruction is given to move left or right, do so
        // Consider: should this apply the same in mid-air? If not, how *should* it work?
        if (Input.GetAxis("Horizontal") != 0) {
            float moveSpeedChange = Input.GetAxis("Horizontal") * moveSpeedMultiplier;
            float desiredSpeed = Mathf.Clamp(rb.velocity.x + moveSpeedChange, -maxSpeedHorizontal, maxSpeedHorizontal);
            rb.velocity = new Vector3(desiredSpeed, rb.velocity.y, 0);
            //Vector3 force = new Vector3(, 0, 0);
            //rb.AddForce(force, ForceMode.Impulse);
        }

        // Apply slowdown "drag" for better movement control,
        // and for staying put when landing on the ground
        if (!IsAccelerating() && this.isOnFloor) {
            float adjustedVelocity = rb.velocity.x * slowdown;
            Vector3 v = new Vector3( adjustedVelocity < 0.05 ? 0 : adjustedVelocity , rb.velocity.y, rb.velocity.z);
            rb.velocity = v; 
        }
    }

    public bool IsAccelerating()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput != 0 && (moveInput * rb.velocity.x) > 0) {
            return true;
        } else {
            return false;
        }
    }

    /// <summary>
    /// Determine if the character is in contact with the ground, using raycasting.
    /// </summary>
    /// <returns>true if the character is in contact with the ground</returns>
    bool IsGrounded()
    { 
        this.isOnFloor = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        return this.isOnFloor;
    }

    /// <summary>
    /// Apply force to the 
    /// </summary>
    void Jump()
    {
        // if the character is not on the floor, don't allow the jump to take place
        //if (!this.isOnFloor) { return; }

        // if allowed, add the jump velocity to the model, making them jump
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForceMultiplier, 0);

        // ...then set the flag indicating they are no longer
        // on the ground, so you can't jump while in mid-air
        this.isOnFloor = false;
    }
}
