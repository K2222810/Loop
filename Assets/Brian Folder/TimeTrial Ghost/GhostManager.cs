using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject player;
    public GameObject ghostPrefab;
    private GameObject currentGhost = null;

    [Header("Camera Field")]
    public Camera mainCamera;
    public GameObject ghostCameraPrefab;
    private GameObject currentGhostCameraObject = null;
    private GhostCameraManager cameraManager;

    private GhostRecorder recorder;
    private bool isRecording = false;

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    void Start()
    {
        recorder = player.GetComponent<GhostRecorder>();

        playerStartPosition = player.transform.position;
        playerStartPosition = player.transform.position;

        // Setup camera manager once
        if (cameraManager == null)
        {
            GameObject cameraManagerObj = new GameObject("GhostCameraManager");
            cameraManager = cameraManagerObj.AddComponent<GhostCameraManager>();
            cameraManager.mainCamera = mainCamera;
            cameraManager.player = player.transform;
            cameraManager.enabled = false;
        }
        
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
            // Reset player position
            player.transform.position = playerStartPosition;
            player.transform.rotation = playerStartRotation;

            // Reset player velocity if it has a Rigidbody
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
            }

            if (currentGhost != null)
            {
                Destroy(currentGhost);
                currentGhost = null;
                Debug.Log("Existing ghost destroyed");
            }

            if (currentGhostCameraObject != null)
            {
                Destroy(currentGhostCameraObject.gameObject);
                currentGhostCameraObject = null;
                cameraManager.enabled = false;
                Debug.Log("Existing ghost camera destroyed");
            }

            //Setup ghost prefab and replayer
            currentGhost = Instantiate(ghostPrefab, recorder.recordedFrames[0].position, Quaternion.identity);
            GhostReplayer replayer = currentGhost.GetComponent<GhostReplayer>();
            replayer.replayData = new List<GhostFrame>(recorder.recordedFrames);
            Debug.Log("Ghost spawned");

            // Create and setup new ghost camera
            currentGhostCameraObject = Instantiate(ghostCameraPrefab, currentGhost.transform);
            Camera newGhostCamera = currentGhostCameraObject.GetComponent<Camera>();

            // Setup camera manager
            cameraManager.ghostCamera = newGhostCamera;
            cameraManager.ghost = currentGhost.transform;
            cameraManager.enabled = true;
            cameraManager.ResetCameraState();

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

    private void OnDisable()
    {
        if (cameraManager != null)
        {
            cameraManager.enabled = false;
        }
        if (currentGhostCameraObject != null)
        {
            Destroy(currentGhostCameraObject);
        }
    }
}