using System;
using System.Collections;
using Player;
using Sirenix.OdinInspector;
using States;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IKillable
{
    public static PlayerController Instance;

    [Header("General")] 
    public int lives = 1;
    public int maxLives = 1;
    public Rigidbody2D rb;
    public SoulController soulController;
    [Header("Speed")]
    public float moveMaxSpeed = 5f;
    public float moveAcceleration = 5f;
    public float maxYSpeed = 5f;
    [Header("Jump")]
    public float jumpForce = 700f;
    public float jumpDelay = 0.5f;
    public float jumpTimer;
    [Header("Rotation")]
    public float rotateBackTime = 0.2f;
    [Header("Gravity")]
    public float ragdollGravityScale = 0.5f;
    public float deathGravityScale = 0f;
    [HideInInspector]
    public float baseGravityScale;
    [Header("Collisions")] 
    public int groundLayer; // Set this to the layer your ground is on in the inspector
    public LayerMask groundLayerMask;
    public CapsuleCollider2D playerCollider;
    public float groundCheckDistance = 0.1f;
    private float _playerExtend;
    public bool touchingSomething;
    public bool isGrounded => CheckGround() && touchingSomething;
    [Header("State Materials")]
    public float delay = 0.1f;
    public PhysicsMaterial2D defaultMaterial;
    public PhysicsMaterial2D ragdollMaterial;
    public PhysicsMaterial2D deathMaterial;
    [Header("State")]
    public PlayerState currentState;
    public PlayerState defaultState;
    public PlayerState ragdollState;
    public PlayerState deathState;
    public Action<PlayerState> OnStateChange;
    [Header("Particles")]
    public GameObject deathEffect;
    public GameObject trueDeathEffect;
    public GameObject reviveEffect;
    private void Awake()
    {
        Instance = this;
        defaultState = new DefaultState();
        ragdollState = new RagdollState();
        deathState = new DeathState();
        baseGravityScale = rb.gravityScale;
        rb.gravityScale = 0.0f;
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        GameManager.Instance.playerVelocityOnLeave = Vector2.zero;
        currentState = defaultState;
        currentState.Enter();
        _playerExtend = playerCollider.size.y / 2f * transform.localScale.y;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.Instance.loadedFromCheckpoint)
        {
            rb.velocity = Vector2.zero;
            lives = GameManager.Instance.livesOnCheckpoint;
            return;
        }
        rb.velocity = GameManager.Instance.playerVelocityOnLeave;
        lives = GameManager.Instance.livesOnLeave;
    }
    
    void Update()
    {
        if (Time.timeScale == 0) return;
        jumpTimer -= Time.deltaTime;
        currentState.Update();
    }
    
    public bool CheckGround()
    {
        // Send a short raycast down from the player to check for ground
        Vector3 raycastOrigin = transform.position + new Vector3(0f, -_playerExtend, 0f);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundCheckDistance, groundLayerMask);

        // If the raycast hit something (i.e., the ground), allow the player to jump
        return hit.collider != null;
    }
    
    public void Revive()
    {
        PlayerAudio.Instance.PlayReviveSound();
        Instantiate(reviveEffect, transform.position, Quaternion.identity);
        SetState(ragdollState);
    }

    public void Heal()
    {
        lives = maxLives;
    }
    
    public void Kill()
    {
        currentState.Kill();
    }
    
    public void RotateBack()
    {
        StartCoroutine(RotationBackToNormal());
    }
    
    public void SwitchMaterialWithDelay(PhysicsMaterial2D material)
    {
        StartCoroutine(SwitchMaterialWithDelay(material, delay));
    }
    
    private IEnumerator RotationBackToNormal()
    {
        float estimatedTime = 0f;
        Quaternion startRotation = transform.rotation;
        while (estimatedTime < rotateBackTime)
        {
            estimatedTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.identity, estimatedTime / rotateBackTime);
            yield return null;
        }
    }

    private IEnumerator SwitchMaterialWithDelay(PhysicsMaterial2D material, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.sharedMaterial = material;
    }
 
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        touchingSomething = true;
        currentState.OnCollisionEnter2D(other);
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        touchingSomething = false;
        currentState.OnCollisionExit2D(other);
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        touchingSomething = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentState.OnTriggerEnter2D(other);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        currentState.OnTriggerExit2D(other);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}