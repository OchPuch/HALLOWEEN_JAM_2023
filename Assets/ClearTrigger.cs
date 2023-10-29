using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PistolController.Instance.isEquipped = false;
            PistolController.Instance.isLoaded = false;
        }
    }
}
