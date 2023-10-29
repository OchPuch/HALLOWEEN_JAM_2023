using System;
using System.Collections;
using System.Collections.Generic;
using Pistol;
using UnityEngine;

public class PistolPropPhysics : MonoBehaviour
{
    public PistolProp connectedProp;
    public Rigidbody2D rb;
    public bool isLerpingBack = false;
    public float lerpTime = 1f;
    public GameObject hitEffect;
    
    public PhysicsMaterial2D bouncyMaterial;
    public PhysicsMaterial2D smoothMaterial;
    
    public bool isThrown = false;

    private void Start()
    {
        rb.sharedMaterial = isThrown ? bouncyMaterial : smoothMaterial;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isLerpingBack) isLerpingBack = false;
        
        
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            //if line between pistol and player is not blocked by anything
            //then pistol is lerping back to player
            //else pistol is falling down
            
            //check line between pistol and player
            if (Physics2D.Linecast(transform.position, PlayerController.Instance.transform.position, LayerMask.GetMask("Level")))
            {
                //if line is blocked by something
                //pistol is flying up
                rb.AddForce(Vector2.up * connectedProp.enemyHitForce, ForceMode2D.Impulse);
            }
            else
            {
                //if line is not blocked by anything
                //pistol is lerping back to player
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = Vector2.zero;
                StartCoroutine(LerpPistolBack());
            }

            if (!connectedProp.isLoaded)
            {
                connectedProp.isLoaded = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (!connectedProp.isLoaded)
            {
                var bullet = other.gameObject.GetComponent<Bullet>();
                
                connectedProp.isLoaded = true;
                rb.velocity = bullet.rb.velocity;
                Destroy(other.gameObject);
            }

            if (connectedProp.isLoaded)
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Level"))
        {
            isThrown = false;
            rb.sharedMaterial = smoothMaterial;
        }
    }

    private IEnumerator LerpPistolBack()
    {
        float timeElapsed = 0f;
        isLerpingBack = true;
        while (timeElapsed < lerpTime)
        {
            if (!isLerpingBack) yield break;
            transform.position = Vector3.Lerp(transform.position, PlayerController.Instance.transform.position, timeElapsed / lerpTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        isLerpingBack = false;
    }
}
