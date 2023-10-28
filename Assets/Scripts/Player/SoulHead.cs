using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHead : MonoBehaviour
{
    
    public InteractableThing closestThing;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Things"))
        {
            var interactableThing = other.GetComponent<InteractableThing>();
            if (interactableThing != null)
            {
                if (Vector2.Distance(transform.position, interactableThing.transform.position) < Vector2.Distance(transform.position, closestThing.transform.position))
                {
                    closestThing = interactableThing;
                }
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Things"))
        {
            var interactableThing = other.GetComponent<InteractableThing>();
            if (interactableThing != null)
            {
                if (interactableThing == closestThing)
                {
                    closestThing = null;
                }
            }
        }
    }
}
