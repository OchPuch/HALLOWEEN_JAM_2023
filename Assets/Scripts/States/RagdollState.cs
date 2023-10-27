using System;
using UnityEngine;

namespace States
{
    [Serializable]
    public class RagdollState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            PlayerController.rb.sharedMaterial = PlayerController.ragdollMaterial;
            PlayerController.rb.velocity = Vector2.zero;
            PlayerController.rb.AddForce(PlayerController.soulController.deactivateDistance * PlayerController.soulController.deactivateDirection.normalized, ForceMode2D.Impulse);
            
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}