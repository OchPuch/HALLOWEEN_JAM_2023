using System;
using UnityEngine;

namespace States
{
    [Serializable]
    public class DefaultState : PlayerState
    {
        
        public override void Enter()
        {
            base.Enter();
            PlayerController.rb.sharedMaterial = PlayerController.defaultMaterial;
        }
        
        public override void Update()
        {
            bool isGrounded = PlayerController.CheckGround();
            float moveInput = Input.GetAxis("Horizontal");
        
            if (!isGrounded)
            {
                PlayerController.rb.AddForce(Vector2.right * (PlayerController.moveAcceleration * Time.deltaTime * moveInput));
                if (Mathf.Abs(PlayerController.rb.velocityX) > PlayerController.moveMaxSpeed) PlayerController.rb.velocity = new Vector2(Mathf.Sign(PlayerController.rb.velocityX) * PlayerController.moveMaxSpeed ,PlayerController.rb.velocity.y);
            }
            else
            {
                PlayerController.rb.velocity = new Vector2(PlayerController.moveMaxSpeed * moveInput, PlayerController.rb.velocity.y);
            }
        
            if (Input.GetButton("Jump"))
            { 
                if (isGrounded) Jump();
            }
        
            if (PlayerController.rb.velocityY > Mathf.Abs(PlayerController.maxYSpeed)) PlayerController.rb.velocity = new Vector2(PlayerController.rb.velocity.x, Mathf.Sign(PlayerController.rb.velocityY) * PlayerController.maxYSpeed);
        }

        public override void Exit()
        {
            
        }

        private void Jump()
        {
            if (PlayerController.jumpTimer > 0f) return;
            if (!PlayerController.touchingSomething) return;
            PlayerController.rb.AddForce(new Vector2(0f, PlayerController.jumpForce));
            PlayerController.jumpTimer = PlayerController.jumpDelay;
        }

        public override void Hit()
        {
            PlayerController.Die();
        }
    }
}