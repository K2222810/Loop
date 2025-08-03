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
    private EnhancedPlayerUi enhancedplayerUI;
    private InputManager inputManager;

    [Header("Yolo")]
    public Interactable currentInteractable;
    private bool isUIInitialised = false;
    private IEnumerator InitialiseUI()
    {
        // Wait for a frame to ensure all components are initialized
        yield return new WaitForEndOfFrame();

        enhancedplayerUI = GetComponent<EnhancedPlayerUi>();
        if (enhancedplayerUI != null)
        {
            isUIInitialised = true;
        }
        else
        {
            Debug.LogWarning("EnhancedPlayerUI component not found!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent <PlayerLook>().cam;
        //enhancedplayerUI = GetComponent<EnhancedPlayerUi>(); 
        inputManager = GetComponent<InputManager>();
        // Wait for UI to be initialized
        StartCoroutine(InitialiseUI());
    }
    // Update is called once per frame
    void Update()
    {
        if (isUIInitialised)
        {
            enhancedplayerUI.UpdateText(string.Empty);
        }
        //create a ray at the center of the camera,shooting upwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //variable store our collision information

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            currentInteractable = hitInfo.collider.GetComponent<Interactable>();
            if (currentInteractable != null)
            {
                enhancedplayerUI.UpdateText(currentInteractable.promptMessage);

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
