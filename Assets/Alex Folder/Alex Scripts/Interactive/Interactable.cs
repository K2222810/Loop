using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{   //Add or remove an interactionEvent component ot this gameObject
    public bool useEvents;
    [SerializeField]
    public string promptMessage;

    public bool hasBeenInteracted = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool hasInitialised = false;

    protected virtual void Awake()
    {
        StoreOriginalTransform();
    }

    private void StoreOriginalTransform()
    {
        if (!hasInitialised)
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            hasInitialised = true;
            Debug.Log($"Stored original position for {gameObject.name}: {originalPosition}");
        }
    }

    public virtual string OnLook()
    {
        return promptMessage;
    }

    public void ResetToOriginalPosition()
    {
        if (!hasInitialised)
        {
            StoreOriginalTransform();
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        hasBeenInteracted = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        Debug.Log($"Reset {gameObject.name} to position: {originalPosition}");
    }

    public void BaseInteract()
    {
        //Becuase we using a editng script to edit the component,this value shouldn't be 
        //null
        if(useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();  
        Interact();
    }
    //This code will always be call
    protected virtual void Interact()
    {
        //We wont have any code called from the base interactabke script.
        //any custom code will go inseade of this methor on our inherited scripts.
    }



}
