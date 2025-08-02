using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GhostReplayer : MonoBehaviour
{
    public List<GhostFrame> replayData;
    public float playbackSpeed = 7.0f;

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
