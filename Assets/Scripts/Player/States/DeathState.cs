using System;
using Player;
using UnityEngine;

namespace States
{
    [Serializable]
    public class DeathState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            PlayerController.SwitchMaterialWithDelay(PlayerController.deathMaterial);
            PlayerController.rb.gravityScale = PlayerController.deathGravityScale;
            PlayerController.rb.velocity = Vector2.zero;
        }
        
        public override void Update()
        {
        }

        public override void Exit()
        {
        }
    }
}