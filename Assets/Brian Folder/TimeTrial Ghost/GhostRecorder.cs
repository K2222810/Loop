using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    public float recordDistanceThreshold = 2f; // Adjust for frequency
    public List<GhostFrame> recordedFrames = new();

    private Vector3 lastRecordedPosition;
    public bool isRecording = false;

    private PlayerInteraction playerInteraction;
    private List<int> lastHeldIDs = new List<int>();

    [Header("Button Recording Logic")]
    private float recordingStartTime;
    private bool isPressingButton = false;

    private void Start()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    public void StartRecording()
    {
        recordedFrames.Clear();
        isRecording = true;
        lastRecordedPosition = transform.position;
        //recordingStarTime = Time.time;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    void Update()
    {
        if (!isRecording) return;

        List<int> grabbedIDs = new List<int>();
        GrabObject[] allGrabbables = FindObjectsByType<GrabObject>(FindObjectsSortMode.None);

        foreach (var obj in allGrabbables)
        {
            var isGrabbedField = obj.GetType().GetField("isHeld", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (isGrabbedField == null)
            {
                Debug.LogWarning($"No 'isHeld' field found on {obj.name}");
                continue;
            }

            if (isGrabbedField != null)
            {
                bool isGrabbed = (bool)isGrabbedField.GetValue(obj);
                if (isGrabbed)
                {
                    grabbedIDs.Add(obj.gameObject.GetInstanceID());
                }
            }
        }

        // Check for button interaction
        isPressingButton = false;
        if (playerInteraction.currentInteractable != null &&
            playerInteraction.currentInteractable is ButtonInteractable button)
        {
            isPressingButton = button.isPressed;
        }

        // Record frame if either movement threshold is met or state has changed
        float sqrDist = (transform.position - lastRecordedPosition).sqrMagnitude;
        bool hasMoved = sqrDist >= recordDistanceThreshold * recordDistanceThreshold;
        bool heldChanged = !grabbedIDs.SequenceEqual(lastHeldIDs);

        if (hasMoved || heldChanged || isPressingButton)
        {
            GhostFrame frame = new GhostFrame(
                transform.position,
                transform.rotation,
                grabbedIDs.Count > 0,
                isPressingButton,
                Time.time - recordingStartTime,
                grabbedIDs
            );

            recordedFrames.Add(frame);
            lastRecordedPosition = transform.position;
            lastHeldIDs = new List<int>(grabbedIDs);
        }
    }
}
