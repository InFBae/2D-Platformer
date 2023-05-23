using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float movePower;
    [SerializeField] private float jumpPower;

    [SerializeField] LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer render;
    private Vector2 inputDir;
    private bool isGround;

    private void Awake()
    {
       rb = GetComponent<Rigidbody2D>(); 
       animator = GetComponent<Animator>();
       render = rb.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        if(inputDir.x < 0 && rb.velocity.x > -maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
        else if(inputDir.x > 0 && rb.velocity.x < maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
        
    }
    private void OnMove(InputValue inputValue)
    {
        inputDir = inputValue.Get<Vector2>();
        animator.SetFloat("MoveSpeed", Mathf.Abs(inputDir.x));
        if (inputDir.x > 0)
            render.flipX = false;
        else if (inputDir.x < 0)
            render.flipX = true;
    }

    private void OnJump(InputValue inputValue)
    {
        if(isGround)
        {
            Jump();
        }       
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGround = true;
        animator.SetBool("IsGround", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
        animator.SetBool("IsGround", false);
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        
        if(hit.collider != null)
        {
            Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, 0) - transform.position, Color.red);
            Debug.Log(hit.collider.gameObject.name);
            isGround = true;
            animator.SetBool("IsGround", true);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.green);
            isGround = false;
            animator.SetBool("IsGround", false);
        }
    }

    

}
