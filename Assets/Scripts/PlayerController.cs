using System;
using Sirenix.OdinInspector;
using States;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    [Header("General")]
    public Rigidbody2D rb;
    public SoulController soulController;
    [Header("Speed")]
    public float moveMaxSpeed = 5f;
    public float moveAcceleration = 5f;
    public float maxYSpeed = 5f;
    [Header("Jump")]
    public float jumpForce = 700f;
    public float jumpDelay = 0.5f;
    [SerializeField]
    public float jumpTimer;
    [Header("Collisions")]
    public LayerMask groundLayer;  // Set this to the layer your ground is on in the inspector
    public CircleCollider2D playerCollider;
    public float groundCheckDistance = 0.1f;
    private float _playerExtend;
    public bool touchingSomething;
    [Header("State Materials")]
    public PhysicsMaterial2D defaultMaterial;
    public PhysicsMaterial2D ragdollMaterial;
    public PhysicsMaterial2D deathMaterial;
    [Header("State")]
    public PlayerState currentState;
    public PlayerState defaultState;
    public PlayerState ragdollState;
    public PlayerState deathState;
    public Action<PlayerState> OnStateChange;

    

    private void Awake()
    {
        Instance = this;
        defaultState = new DefaultState();
        ragdollState = new RagdollState();
        deathState = new DeathState();

    }

    public void SetState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
        OnStateChange?.Invoke(currentState);
    }
    
    void Start()
    {
        currentState = defaultState;
        currentState.Enter();
        
        _playerExtend = playerCollider.radius * transform.localScale.y;
    }
    
    void Update()
    {
        jumpTimer -= Time.deltaTime;
        currentState.Update();
    }
    
    public bool CheckGround()
    {
        // Send a short raycast down from the player to check for ground
        Vector3 raycastOrigin = transform.position + new Vector3(0f, -_playerExtend, 0f);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundCheckDistance, groundLayer);

        // If the raycast hit something (i.e., the ground), allow the player to jump
        return hit.collider != null;
    }

    [Button]
    public void Die()
    {
        SetState(deathState);
    }
    
    public void Revive()
    {
        SetState(ragdollState);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        touchingSomething = true;
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        touchingSomething = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Danger"))
        {
            currentState.Hit();
        }
    }
}