using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlayerAnimator : MonoBehaviour
{
    public static CustomPlayerAnimator Instance;
    [Header("General")] public GameObject spriteObject;
    public Animator animator;
    public PlayerController playerController;
    private Vector3 _spriteObjectScale;
    [Header("Thresholds")] public float runThreshold = 0.1f;
    public float fallingOrFlyingThreshold = 0.1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerController = PlayerController.Instance;
        _spriteObjectScale = spriteObject.transform.localScale;
        animator.SetBool("IsRunning", false);
        animator.SetBool("Falling", false);
        animator.SetBool("Flying", false);
    }

    private void Update()
    {
        if (playerController.currentState == playerController.defaultState)
        {
            float horizontalMovement = Mathf.Abs(playerController.rb.velocity.x);
            float verticalMovement = playerController.rb.velocity.y;
            if (horizontalMovement > runThreshold)
                spriteObject.transform.localScale =
                    new Vector3(Mathf.Sign(playerController.rb.velocity.x) * _spriteObjectScale.x, _spriteObjectScale.y,
                        _spriteObjectScale.z);

            // Check if player is grounded and running
            if (playerController.CheckGround() && horizontalMovement > runThreshold)
            {
                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }
            
            if (verticalMovement > fallingOrFlyingThreshold)
            {
                animator.SetBool("Flying", true);
            }
            else if (verticalMovement < -fallingOrFlyingThreshold)
            {
                animator.SetBool("Falling", true);
            }
            else
            {
                animator.SetBool("Flying", false);
                animator.SetBool("Falling", false);
            }
        }
    }

    public void Jump()
    {
        animator.SetTrigger("StartJump");
    }
}