using UnityEngine;

public abstract class InteractableThing : MonoBehaviour
{
    public bool holdable = false;
    public bool isHeld = false;
    public bool isInteractable = true;
    public bool isActivated = false;
    public bool isSwitch = false;
    public bool onlySoul = false;
    public abstract void Interact();
}