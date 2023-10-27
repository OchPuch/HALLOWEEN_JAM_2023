using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public Rigidbody2D rb;
    [Header("Speed")]
    public float moveMaxSpeed = 5f;
    public float moveAcceleration = 5f;
    public float maxYSpeed = 5f;
    [Header("Jump")]
    public float jumpForce = 700f;
    public float jumpDelay = 0.5f;
    [SerializeField]
    private float _jumpTimer;
    [Header("Collisions")]
    public LayerMask groundLayer;  // Set this to the layer your ground is on in the inspector
    public CircleCollider2D playerCollider;
    public float groundCheckDistance = 0.1f;
    private float _playerExtend;
    private bool _touchingSomething;
    
    
    void Start()
    {
        _playerExtend = playerCollider.radius * transform.localScale.y;
    }
    
    void Update()
    {
        bool isGrounded = CheckGround();
        float moveInput = Input.GetAxis("Horizontal");
        _jumpTimer -= Time.deltaTime;
        
        if (!isGrounded)
        {
            rb.AddForce(Vector2.right * (moveAcceleration * Time.deltaTime * moveInput));
            if (Mathf.Abs(rb.velocityX) > moveMaxSpeed) rb.velocity = new Vector2(Mathf.Sign(rb.velocityX) * moveMaxSpeed , rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveMaxSpeed * moveInput, rb.velocity.y);
        }
        
        if (Input.GetButton("Jump"))
        { 
            if (isGrounded) Jump();
        }
        
        if (rb.velocityY > Mathf.Abs(maxYSpeed)) rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocityY) * maxYSpeed);
        
    }
    
    private bool CheckGround()
    {
        // Send a short raycast down from the player to check for ground
        Vector3 raycastOrigin = transform.position + new Vector3(0f, -_playerExtend, 0f);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundCheckDistance, groundLayer);
        
        // If the raycast hit something (i.e., the ground), allow the player to jump
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    public void Jump()
    {
        if (_jumpTimer > 0f) return;
        if (!_touchingSomething) return;
        
        rb.AddForce(new Vector2(0f, jumpForce));
        _jumpTimer = jumpDelay;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _touchingSomething = true;
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        _touchingSomething = false;
    }
}