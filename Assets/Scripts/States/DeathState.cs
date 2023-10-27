using System;

namespace States
{
    [Serializable]
    public class DeathState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            PlayerController.rb.sharedMaterial = PlayerController.deathMaterial;
        }
        
        public override void Update()
        {
        }

        public override void Exit()
        {
        }
    }
}