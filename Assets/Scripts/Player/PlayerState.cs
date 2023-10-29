using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public abstract class PlayerState 
    { 
        protected PlayerController PlayerController;
    
        public virtual void Enter()
        {
            PlayerController = PlayerController.Instance;
            Debug.Log("Enter " + GetType().Name);
        }

        public abstract void Update();

        public abstract void Exit();
    
        public virtual void OnCollisionEnter2D(Collision2D other)
        {
        
        }
    
        public virtual void OnCollisionExit2D(Collision2D other)
        {
        
        }
    
        public virtual void OnTriggerEnter2D(Collider2D other)
        {
        
        }
    
        public virtual void OnTriggerExit2D(Collider2D other)
        {
        
        }

        public virtual void Kill()
        {
            
        }


    }
}
