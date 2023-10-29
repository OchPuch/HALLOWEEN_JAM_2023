using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public bool fastMusic;
    public bool FINALTRIGGER = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (FINALTRIGGER)
            {
                MusicManager.Instance.ChangeToFinal();
                return;
            }
            
            if (fastMusic)
            {
                MusicManager.Instance.ChangeToFast();
            }
            else
            {
                MusicManager.Instance.ChangeToSlow();
            }
        }
    }
}
