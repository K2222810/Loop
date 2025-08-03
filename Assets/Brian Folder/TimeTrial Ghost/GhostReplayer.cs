using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.UI;


public class GhostReplayer : MonoBehaviour
{
    public List<GhostFrame> replayData;
    public float playbackSpeed = 7.0f;

    [Header("Button Variables")]
    public float buttonInteractionDistance = 3.0f;
    public LayerMask interactionLayer = -1;

    private int currentIndex = 0;
    private float t = 0f;
    private Dictionary<int, GameObject> trackedObjects = new Dictionary<int, GameObject>();

    private void Start()
    {
        // Cache all interactive objects at start
        GrabObject[] allGrabbables = FindObjectsByType<GrabObject>(FindObjectsSortMode.None);
        foreach (var obj in allGrabbables)
        {
            trackedObjects[obj.gameObject.GetInstanceID()] = obj.gameObject;
        }
    }

    private void Update()
    {
        if (replayData == null || replayData.Count < 2) return;

        GhostFrame currentFrame = replayData[currentIndex];
        GhostFrame nextFrame = replayData[currentIndex + 1];

        t += (Time.deltaTime * playbackSpeed) / Vector3.Distance(currentFrame.position, nextFrame.position);

        transform.position = Vector3.Lerp(currentFrame.position, nextFrame.position, t);
        transform.rotation = Quaternion.Lerp(currentFrame.rotation, nextFrame.rotation, t);

        // Handle grabbed objects
        UpdateGrabbedObjects(currentFrame.grabbedObjectIDs);

        if (currentFrame.isPressingButton)
        {
            HandleButtonInteraction();
        }

        if (t >= 1f)
        {
            currentIndex++;
            t = 0f;

            if (currentIndex >= replayData.Count - 1)
            {
                enabled = false;
                ReleaseAllObjects();
            }    
        }

    }
    private void UpdateGrabbedObjects(List<int> objectIDs)
    {
        // Handle null objectIDs list
        if (objectIDs == null)
        {
            objectIDs = new List<int>();
        }

        foreach (var kvp in trackedObjects)
        {
            GameObject obj = kvp.Value;
            if (obj == null) continue;

            if (objectIDs.Contains(kvp.Key))
            {
                // Object should be grabbed
                AttachObjectToGhost(obj);
            }
            else
            {
                // Object should be released
                DetachObjectFromGhost(obj);
            }
        }
    }

    private void HandleButtonInteraction()
    {
        // Find all buttons and check if ghost is close enough to any of them
        ButtonInteractable[] allButtons = FindObjectsByType<ButtonInteractable>(FindObjectsSortMode.None);

        Debug.Log($"Found {allButtons.Length} buttons in scene");

        foreach (ButtonInteractable button in allButtons)
        {
            if (button != null)
            {
                float distanceToButton = Vector3.Distance(transform.position, button.transform.position);

                // Debug info
                Debug.Log($"Ghost distance to button {button.name}: {distanceToButton:F2}, hasBeenInteracted: {button.hasBeenInteracted}, buttonInteractionDistance: {buttonInteractionDistance}");

                if (!button.hasBeenInteracted)
                {
                    if (distanceToButton <= buttonInteractionDistance)
                    {
                        Debug.Log($"Ghost close enough to press button: {button.name}");
                        button.PressButton();
                        break; // Only press one button at a time
                    }
                    else
                    {
                        Debug.Log($"Ghost too far from button {button.name}: {distanceToButton:F2} > {buttonInteractionDistance}");
                    }
                }
                else
                {
                    Debug.Log($"Button {button.name} already interacted with");
                }
            }
        }
    }

    private void AttachObjectToGhost(GameObject obj)
    {
        if (obj.transform.parent != transform)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            obj.transform.SetParent(transform);
            obj.transform.localPosition = new Vector3(0, 0, 1.5f);
        }
    }

    private void DetachObjectFromGhost(GameObject obj)
    {
        if (obj.transform.parent == transform)
        {
            obj.transform.SetParent(null);
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }
    private void ReleaseAllObjects()
    {
        foreach (var kvp in trackedObjects)
        {
            if (kvp.Value != null)
            {
                DetachObjectFromGhost(kvp.Value);
            }
        }
    }
    private void OnDisable()
    {
        ReleaseAllObjects();
    }
}
