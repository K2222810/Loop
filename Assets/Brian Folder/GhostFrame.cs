using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GhostFrame{
    public Vector3 position;
    public Quaternion rotation;

    public GhostFrame(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}
