using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GhostFrame{
    public Vector3 position;
    public Quaternion rotation;

    public bool isHoldingObject;
    public bool isPressingButton;
    public float timestamp;

    public List<int> grabbedObjectIDs;

    // Constructor for button pressing and object grabbing
    public GhostFrame(Vector3 pos, Quaternion rot, bool holding, bool pressing, float time, List<int> grabbedIDs = null)
    {
        position = pos;
        rotation = rot;
        isHoldingObject = holding;
        isPressingButton = pressing;
        timestamp = time;
        grabbedObjectIDs = new List<int>(grabbedIDs ?? new List<int>());
    }

    // Simplified constructor for just position, rotation and grabbed objects
    public GhostFrame(Vector3 pos, Quaternion rot, List<int> grabbedIDs)
    {
        position = pos;
        rotation = rot;
        isHoldingObject = grabbedIDs != null && grabbedIDs.Count > 0;
        isPressingButton = false;
        timestamp = Time.time;
        grabbedObjectIDs = new List<int>(grabbedIDs ?? new List<int>());
    }
}
