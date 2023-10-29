using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PistolProp : MonoBehaviour
{
    public bool isLoaded = true;
    public float enemyHitForce = 10f;
    public PistolPropPhysics physics;
    public GameObject loadedIndicator;
    public bool canBePickedUp = false;
    
    

    private void Start()
    {
        StartCoroutine(WaitBeforeBeingAbleToPickUp());
    }

    private void Update()
    {
        loadedIndicator.SetActive(isLoaded);
    }
    
    private IEnumerator WaitBeforeBeingAbleToPickUp()
    {
        yield return new WaitForSeconds(0.1f);
        canBePickedUp = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (canBePickedUp)
        {
            TryToPickUp();
        }
    }
    
    public void TryToPickUp()
    {
        switch (PistolController.Instance.isEquipped)
        {
            case true when PistolController.Instance.isLoaded:
                return;
            case true when !PistolController.Instance.isLoaded:
                if (isLoaded) 
                {
                    PistolController.Instance.isLoaded = true;
                    isLoaded = false;
                }
                break;
            default:
                if (!canBePickedUp) return;
                PistolController.Instance.PickUpPistol(this);
                Destroy(physics.gameObject);
                break;
        }
    }
    
 
    
    
}