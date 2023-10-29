using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite fullSprite;
    public Sprite collectedSprite;
    public bool isUsed;
    public GameObject substance;
    public GameObject collectEffect;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isUsed && PlayerController.Instance.lives != PlayerController.Instance.maxLives)
            {
                PlayerController.Instance.Heal();
                sr.sprite = collectedSprite;
                substance.SetActive(false);
                isUsed = true;
            }
        }
    }
}
