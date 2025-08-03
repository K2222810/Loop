using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera cam;
    [SerializeField] 
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;

    [Header("Yolo")]
    public Interactable currentInteractable;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent <PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>(); 
        inputManager = GetComponent<InputManager>();
    }
    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);
        //create a ray at the center of the camera,shooting upwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //variable store our collision information

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            currentInteractable = hitInfo.collider.GetComponent<Interactable>();
            if (currentInteractable != null)
            {
                playerUI.UpdateText(currentInteractable.promptMessage);

                // Check if object hasn't been interacted with
                if (inputManager.onFoot.Interact.triggered && !currentInteractable.hasBeenInteracted)
                {
                    currentInteractable.BaseInteract();
                }

                // Reset object
                if (Input.GetKeyUp(KeyCode.C))
                {
                    currentInteractable.ResetToOriginalPosition();
                    currentInteractable.hasBeenInteracted = false;
                }
            }
        }
        else
        {
            currentInteractable = null;
        }
    }
}
