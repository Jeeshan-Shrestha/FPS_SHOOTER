using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents; 
    public String promptMessage;

    public void BaseInteract()
    {
        if (useEvents)
            gameObject.GetComponent<InteractionEvent>().onInteract.Invoke();
        Interact();
    }

    protected virtual void Interact()
    {
        
    }
}
