using UnityEngine;

public class GhostCameraManager : MonoBehaviour
{
    public Camera ghostCamera;
    public Camera mainCamera;
    public Transform player;
    public Transform ghost;

    [Header("Camera Settings")]
    public float maxDistance = 15f;
    public float fullEffectDistance = 25f;
    public float smoothSpeed = 5f;
    public Vector3 ghostCameraOffset = new Vector3(0, 5f, -8f);

    [Header("Screen Settings")]
    [Range(0.1f, 0.5f)]
    public float targetScreenPercentage = 0.4f;

    private float currentLerpValue = 0f;
    private Rect originalRect;
    private bool isActive = false;

    private void Start()
    {
        if (ghostCamera == null || mainCamera == null)
        {
            Debug.LogError("Cameras not assigned to GhostCameraManager");
            enabled = false;
            return;
        }
        // Init camera
        originalRect = ghostCamera.rect;
        ghostCamera.enabled = false;
        mainCamera.rect = new Rect(0, 0, 1, 1);
    }

    private void LateUpdate()
    {
        if (ghost == null || player == null || ghostCamera == null) return;

        float distance = Vector3.Distance(player.position, ghost.position);
        float targetLerp = Mathf.InverseLerp(maxDistance, fullEffectDistance, distance);

        currentLerpValue = Mathf.Lerp(currentLerpValue, targetLerp, Time.deltaTime * smoothSpeed);

        if (currentLerpValue > 0.01f && !isActive)
        {
            ghostCamera.enabled = true;
            isActive = true;
        }
        else if (currentLerpValue <= 0.01f && isActive)
        {
            ghostCamera.enabled = false;
            isActive = false;
        }
        if (isActive)
        {
            UpdateCameraRects();
            UpdateGhostCameraPosition();
        }
    }

    public void ResetCameraState()
    {
        if (ghost == null || player == null) return;

        // Immediately calculate the correct lerp value based on current distance
        float distance = Vector3.Distance(player.position, ghost.position);
        currentLerpValue = Mathf.InverseLerp(maxDistance, fullEffectDistance, distance);

        // Enable camera immediately if needed
        if (currentLerpValue > 0.01f)
        {
            ghostCamera.enabled = true;
            isActive = true;
            UpdateCameraRects();
            UpdateGhostCameraPosition();
        }
    }

    void UpdateCameraRects()
    {
        float ghostWidth = targetScreenPercentage * currentLerpValue;
        float mainWidth = 1f;

        // Update main camera rect (shifts left)
        mainCamera.rect = new Rect(0, 0, mainWidth, 1);

        // Update ghost camera rect (right side)
        ghostCamera.rect = new Rect(1 - ghostWidth, 0, ghostWidth, 1);
    }

    void UpdateGhostCameraPosition()
    {
        if (ghost == null || ghostCamera == null) return;

        // Calculate desired position
        Vector3 targetPosition = ghost.position + ghost.TransformDirection(ghostCameraOffset);

        // Smoothly move camera
        ghostCamera.transform.position = Vector3.Lerp(
            ghostCamera.transform.position,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );

        // Make camera look at ghost
        Vector3 directionToGhost = (ghost.position - ghostCamera.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToGhost);
        ghostCamera.transform.rotation = Quaternion.Slerp(
            ghostCamera.transform.rotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );
    }

    void OnDisable()
    {
        // Reset camera rects when disabled
        if (mainCamera != null) mainCamera.rect = new Rect(0, 0, 1, 1);
        if (ghostCamera != null)
        {
            ghostCamera.rect = originalRect;
            ghostCamera.enabled = false;
        }
    }
}
