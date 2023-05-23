using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Mouse : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] LayerMask groundMask;

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        if (!IsGroundExist())
        {
            Turn();
        }
    }

    public void Move()
    {
        rb.velocity = new Vector2(transform.right.x * -moveSpeed, rb.velocity.y);
    }

    public void Turn()
    {
        transform.Rotate(Vector3.up, 180);
    }

    private bool IsGroundExist()
    {

        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 1f, groundMask);
    }
}
