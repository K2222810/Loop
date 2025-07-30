using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject player;
    public GameObject ghostPrefab;

    private GhostRecorder recorder;
    private bool hasStarted = false;
    private bool hasFinished = false;

    void Start()
    {
        recorder = player.GetComponent<GhostRecorder>();
    }

    public void OnStartTrigger()
    {
        hasStarted = true;
        hasFinished = false;
        recorder.StartRecording();
    }

    public void OnFinishTrigger()
    {
        if (hasStarted)
        {
            hasFinished = true;
            recorder.StopRecording();
        }
    }

    void Update()
    {
        if (hasStarted && hasFinished && Input.GetKeyDown(KeyCode.R))
        {
            GameObject ghost = Instantiate(ghostPrefab, recorder.recordedFrames[0].position, Quaternion.identity);
            GhostReplayer replayer = ghost.GetComponent<GhostReplayer>();
            replayer.replayData = new List<GhostFrame>(recorder.recordedFrames);
        }
    }
}
