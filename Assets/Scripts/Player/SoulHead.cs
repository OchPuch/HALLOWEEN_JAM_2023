using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class SoulHead : MonoBehaviour
    {
        public InteractableThing closestThing;
        public List<InteractableThing> interactableThings = new List<InteractableThing>();
        private SoulController _soulController;

        private void Start()
        {
            _soulController = SoulController.Instance;
            _soulController.soulHead = this;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Things"))
            {
                var interactableThing = other.GetComponent<InteractableThing>();
                if (interactableThing == null) return;
                interactableThings.Add(interactableThing);
                if (closestThing != null) return;
                closestThing = interactableThing;
                _soulController.canInteract = true;
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Things"))
            {
                var interactableThing = other.GetComponent<InteractableThing>();
                if (interactableThing != null)
                {
                    interactableThings.Remove(interactableThing);
                    if (closestThing != interactableThing) return;
                    closestThing = null;
                        
                    if (interactableThings.Count > 0)
                    {
                        closestThing = GetClosestThing();
                        _soulController.canInteract = true;
                    }
                    else
                    {
                        _soulController.canInteract = false;
                    }
                }
            }
        }

        private void Update()
        {
            if (interactableThings.Count <= 1) return;
            closestThing = GetClosestThing();
            _soulController.canInteract = true;

        }
        
        private InteractableThing GetClosestThing()
        {
            if (interactableThings.Count <= 0) return null;
            if (interactableThings.Count == 1) return interactableThings[0];
            var closestDistance = Mathf.Infinity;
            InteractableThing ct = null;
            foreach (var interactableThing in interactableThings)
            {
                var distance = Vector2.Distance(interactableThing.transform.position, transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    ct = interactableThing;
                }
            }
            return ct;
        }
    
    }
}
