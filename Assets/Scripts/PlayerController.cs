using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rayLength = 0.1f;
    public float attackCooldown = 0.5f;
    public float attackRange = 0.5f;
    public float attackDamage = 1f;

    private Rigidbody2D rb;
    private Collider2D cl;
    private bool airborne;
    private bool jumping;
    private float xVel;
    private float yVel;
    private bool facingRight;
    private float jumpForce = 5f;
    private bool attacking;
    private bool inAttackCooldown;
    

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
        MakeAttack();
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

        if (Input.GetMouseButtonDown(0) && !inAttackCooldown)
        {
            attacking = true;
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

    Vector3 GetForward()
    {
        return facingRight ? transform.right : -transform.right;
    }

    void MakeAttack()
    {
        if (attacking)
        {
            attacking = false;
            inAttackCooldown = true;

            Debug.DrawRay(transform.position, GetForward() * attackRange, Color.green, 1f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetForward(), attackRange);

            if (hit)
            {
                Damageable damageable = hit.transform.GetComponent<Damageable>();
                damageable.TakeDamage(attackDamage);
            }

            Invoke(nameof(ResetAttackCooldown), attackCooldown);
        }
    }

    void ResetAttackCooldown()
    {
        inAttackCooldown = false;
    }
}
