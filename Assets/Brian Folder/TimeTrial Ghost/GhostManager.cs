using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject player;
    public GameObject ghostPrefab;
    private GameObject currentGhost = null;

    private GhostRecorder recorder;
    private bool isRecording = false;

    void Start()
    {
        recorder = player.GetComponent<GhostRecorder>();
    }

    public void OnStartTrigger()
    {
        recorder.StartRecording();
    }
    public void OnFinishTrigger()
    {
        recorder.StopRecording();
    }

    void Update()
    {
        // Toggle recording with F
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isRecording)
            {
                recorder.StartRecording();
                isRecording = true;
                Debug.Log("Recording started");
            }
            else
            {
                recorder.StopRecording();
                isRecording = false;
                Debug.Log("Recording stopped");
            }
        }

        // Spawn ghost with R after recording is finished
        if (!isRecording && recorder.recordedFrames.Count > 0 && Input.GetKeyDown(KeyCode.R))
        {
            if (currentGhost != null)
            {
                Destroy(currentGhost);
                currentGhost = null;
                Debug.Log("Existing ghost destroyed");
            }

            currentGhost = Instantiate(ghostPrefab, recorder.recordedFrames[0].position, Quaternion.identity);
            GhostReplayer replayer = currentGhost.GetComponent<GhostReplayer>();
            replayer.replayData = new List<GhostFrame>(recorder.recordedFrames);
            Debug.Log("Ghost spawned");


            // Reset interactable objects
            int interactableLayer = LayerMask.NameToLayer("Interactive");
            InteractableLayerReset[] interactables = FindObjectsOfType<InteractableLayerReset>(); 
            foreach (var obj in interactables)
            {
                if (obj.gameObject.layer == interactableLayer)
                {
                    obj.ResetState();
                }
            }
            Debug.Log("Interactables reset");
        }
    }
}