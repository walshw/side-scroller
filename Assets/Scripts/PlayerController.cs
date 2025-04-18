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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponentInChildren<Collider2D>();
    }

    void Update()
    {
        GroundCheck();
        ProcessInputs();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }
    
    void GroundCheck() {
        Vector2 bottom = new Vector2(transform.position.x, transform.position.y - cl.bounds.extents.y);
        Debug.DrawRay(bottom, -transform.up * rayLength, Color.red, 1f);
        airborne = !Physics2D.Raycast(bottom, -transform.up, rayLength);
    }

    void ProcessInputs() {
        if (Input.GetKeyDown(KeyCode.Space) && !airborne)
        {
            jumping = true;
        }
    }

    void ApplyMovement() {

        float yVel = rb.velocity.y;

        if (jumping)
        {
            jumping = false;
            yVel += 5f;
        }

        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, yVel);
    }
}
