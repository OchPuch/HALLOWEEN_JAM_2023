using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class PlayerState 
{ 
    protected PlayerController PlayerController;
    
    public virtual void Enter()
    {
        PlayerController = PlayerController.Instance;
    }

    public abstract void Update();

    public abstract void Exit();

    public virtual void Hit()
    {
        
    }


}
