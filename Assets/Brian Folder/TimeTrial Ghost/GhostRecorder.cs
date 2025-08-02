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

    private void Start()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    public void StartRecording()
    {
        recordedFrames.Clear();
        isRecording = true;
        lastRecordedPosition = transform.position;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    void Update()
    {
        if (!isRecording) return;

        float sqrDist = (transform.position - lastRecordedPosition).sqrMagnitude;
        if (sqrDist >= recordDistanceThreshold * recordDistanceThreshold)
        {
            List<int> grabbedIDs = new List<int>();
            GrabObject[] allGrabbables = FindObjectsByType<GrabObject>(FindObjectsSortMode.None);
            foreach (var obj in allGrabbables)
            {

                var isGrabbedField = obj.GetType().GetField("isHeld", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (isGrabbedField == null)
                {
                    Debug.LogWarning($"No 'isGrabbed' field found on {obj.name}");
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

            bool hasMoved = sqrDist >= recordDistanceThreshold * recordDistanceThreshold;
            bool heldChanged = !grabbedIDs.SequenceEqual(lastHeldIDs);

            if (hasMoved || heldChanged)
            {
                recordedFrames.Add(new GhostFrame(transform.position, transform.rotation, grabbedIDs));
                lastRecordedPosition = transform.position;
                lastHeldIDs = new List<int>(grabbedIDs); // Clone to avoid reference issues
            }
        }
    }
}
