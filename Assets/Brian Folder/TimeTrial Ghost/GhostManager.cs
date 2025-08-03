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

    private void Awake()
    {
        playerStartPosition = player.transform.position;
        playerStartRotation = player.transform.rotation;
        Debug.Log($"Stored player start position: {playerStartPosition}");
    }

    void Start()
    {
        recorder = player.GetComponent<GhostRecorder>();

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
            CharacterController characterController = player.GetComponent<CharacterController>();
            Rigidbody playerRb = player.GetComponent<Rigidbody>();

            if (characterController != null) {
                // Disable Character Controller temporarily to set position
                characterController.enabled = false;
                player.transform.SetPositionAndRotation(playerStartPosition, playerStartRotation);
                characterController.enabled = true;
            }
            if (playerRb != null)
            {
                // If using Rigidbody
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
                playerRb.position = playerStartPosition;
                playerRb.rotation = playerStartRotation;
                player.transform.SetPositionAndRotation(playerStartPosition, playerStartRotation);
            }

            Debug.Log($"Current position is {player.transform.position} Reset player to position: {playerStartPosition}");

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
            InteractableLayerReset[] interactables = FindObjectsByType<InteractableLayerReset>(FindObjectsSortMode.None); 
            foreach (var obj in interactables)
            {
                if (obj.gameObject.layer == interactableLayer)
                {
                    obj.ResetState();
                }
            }

            ButtonInteractable[] buttons = FindObjectsByType<ButtonInteractable>(FindObjectsSortMode.None);
            foreach (var button in buttons)
            {
                button.ResetToOriginalPosition();
            }

            Debug.Log("Grabbables and buttons reset");
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