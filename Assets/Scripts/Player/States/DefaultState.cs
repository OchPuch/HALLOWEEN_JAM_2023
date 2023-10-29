using System;
using System.Collections;
using Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace States
{
    [Serializable]
    public class DefaultState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            PlayerController.SwitchMaterialWithDelay(PlayerController.defaultMaterial);
            PlayerController.rb.gravityScale = PlayerController.baseGravityScale;
            PlayerController.rb.freezeRotation = true;
            PlayerController.RotateBack();
        }
        
        public override void Update()
        {
            float moveInput = Input.GetAxis("Horizontal");
        
            if (!PlayerController.isGrounded)
            {
                if ((Mathf.Abs(PlayerController.rb.velocityX) < PlayerController.moveMaxSpeed))
                {
                    PlayerController.rb.AddForce(Vector2.right * (PlayerController.moveAcceleration * Time.deltaTime * moveInput));
                }
            }
            else
            {
                PlayerController.rb.velocity = new Vector2(PlayerController.moveMaxSpeed * moveInput, PlayerController.rb.velocity.y);
            }
        
            if (Input.GetButton("Jump"))
            { 
                if (PlayerController.isGrounded) Jump();
            }
        
            if (PlayerController.rb.velocityY > Mathf.Abs(PlayerController.maxYSpeed)) PlayerController.rb.velocity = new Vector2(PlayerController.rb.velocity.x, Mathf.Sign(PlayerController.rb.velocityY) * PlayerController.maxYSpeed);
        }

        public override void Exit()
        {
            
        }

        private void Jump()
        {
            if (PlayerController.jumpTimer > 0f) return;
            PlayerAudio.Instance.PlayJumpSound();
            PlayerController.rb.velocityY = PlayerController.jumpForce;
            PlayerController.jumpTimer = PlayerController.jumpDelay;
            CustomPlayerAnimator.Instance.Jump();
        }

        public override void Kill()
        {
            PlayerAudio.Instance.PlayDeathSound();
            PlayerController.lives--;
            if (PlayerController.lives < 0)
            {
                Object.Instantiate(PlayerController.trueDeathEffect, PlayerController.transform.position, Quaternion.identity);
                PlayerController.SetState(PlayerController.deathState);
                GameManager.Instance.GameOver();
                return;
            }
            Object.Instantiate(PlayerController.deathEffect, PlayerController.transform.position, Quaternion.identity);
            PlayerController.SetState(PlayerController.deathState);
        }


        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Killzone"))
            {
                PlayerController.Kill();
                
            }
        }
        
        
    }
}