using System;
using Player;
using UnityEngine;

namespace States
{
    [Serializable]
    public class RagdollState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            PlayerController.SwitchMaterialWithDelay(PlayerController.ragdollMaterial);
            PlayerController.rb.velocity = Vector2.zero;
            PlayerController.rb.gravityScale = PlayerController.ragdollGravityScale;
            PlayerController.rb.AddForce(
                PlayerController.soulController.deactivateDirection.normalized *
                (PlayerController.soulController.deactivateDistance * PlayerController.soulController.soulPower),
                ForceMode2D.Impulse);
            PlayerController.rb.freezeRotation = false;
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
        }

        public override void OnCollisionExit2D(Collision2D other)
        {
            //if ground layer

            if (other.gameObject.layer == PlayerController.groundLayer)
            {
                PlayerController.SetState(PlayerController.defaultState);
            }
        }

        public override void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                IKillable enemyScript = other.gameObject.GetComponent<IKillable>();
                if (enemyScript != null)
                {
                    enemyScript.Kill();
                }
            }
        }
    }
}