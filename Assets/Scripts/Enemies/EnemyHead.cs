using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour, IKillable
{
   public EnemyScript enemyScript;
   public void Kill()
   {
      enemyScript.Kill();
   }
}
