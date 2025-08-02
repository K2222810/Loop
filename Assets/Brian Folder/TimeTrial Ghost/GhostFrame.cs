using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GhostFrame{
    public Vector3 position;
    public Quaternion rotation;

    public List<int> grabbedObjectIDs;
    public GhostFrame(Vector3 pos, Quaternion rot, List<int> grabbedIDs)
    {
        position = pos;
        rotation = rot;
        grabbedObjectIDs = new List<int>(grabbedIDs);
    }
}
