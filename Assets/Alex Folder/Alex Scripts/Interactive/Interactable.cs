using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{   //Add or remove an interactionEvent component ot this gameObject
    public bool useEvents;
    [SerializeField]
    public string promptMessage;

    public virtual string OnLook()
    {
        return promptMessage;
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
