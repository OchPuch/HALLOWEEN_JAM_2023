using UnityEngine;

public abstract class InteractableThing : MonoBehaviour
{
    public bool holdable = false;
    public bool isHeld = false;
    public bool isInteractable = true;
    public bool isActivated = false;
    public abstract void Interact();
}