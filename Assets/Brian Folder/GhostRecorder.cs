using UnityEngine;
using System.Collections.Generic;

public class GhostRecorder : MonoBehaviour
{
    public bool isRecording = false;
    public List<GhostFrame> recordedFrames = new List<GhostFrame>();

    public void StartRecording()
    {
        recordedFrames.Clear();
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    private void Update()
    {
        if (isRecording)
        {
            recordedFrames.Add(new GhostFrame(transform.position, transform.rotation));
        }
    }
}
