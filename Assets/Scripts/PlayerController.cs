using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rayLength = 0.1f;
    private Rigidbody2D rb;
    private Collider2D cl;
    private bool airborne;
    private bool jumping;
    private float xVel;
    private float yVel;
    private bool facingRight;
    private float jumpForce = 5f;

    void Start()
    {
        // MY GAME RULE: By default, every sprite should be facing right, if you want it to look left then flip x
        facingRight = transform.localScale.x >= 0;

        rb = GetComponent<Rigidbody2D>();
        cl = GetComponentInChildren<Collider2D>();
    }

    void Update()
    {
        GroundCheck();
        ProcessInputs();
        HandleTurningSprite();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void GroundCheck()
    {
        Vector2 bottom = new Vector2(transform.position.x, transform.position.y - cl.bounds.extents.y);
        Debug.DrawRay(bottom, -transform.up * rayLength, Color.red, 1f);
        airborne = !Physics2D.Raycast(bottom, -transform.up, rayLength);
    }

    void ProcessInputs()
    {
        xVel = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !airborne)
        {
            jumping = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("d");
            Debug.DrawRay(transform.position, GetForward() * 3f, Color.green, 1f);
        }
    }

    void ApplyMovement()
    {
        yVel = rb.velocity.y;

        if (jumping)
        {
            jumping = false;
            yVel += jumpForce;
        }

        rb.velocity = new Vector3(xVel * speed, yVel);
    }

    void HandleTurningSprite()
    {
        if (xVel < 0)
        {
            facingRight = false;
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            return;
        }

        if (xVel > 0)
        {
            facingRight = true;
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            return;
        }
    }

    Vector3 GetForward() {
        return facingRight ? transform.right : -transform.right;
    }
}
