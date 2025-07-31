using UnityEngine;

public class GrabbableObject : Interactable
{
    [Header("Grab Settings")]
    public float grabDistance = 2f;
    public float smoothFactor = 15f;
    public LayerMask collisionMask;

    private Rigidbody rb;
    private Transform playerCamera;
    private bool isGrabbed;
    private float currentDistance;
    private int originalLayer;
    private Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main.transform;
        originalLayer = gameObject.layer;
    }

    void Update()
    {
        if (!isGrabbed) return;

        // Calculate target position
        CalculateTargetPosition();
    }

    void FixedUpdate()
    {
        if (!isGrabbed) return;

        // Apply smooth movement in physics update
        ApplySmoothMovement();
    }

    protected override void Interact()
    {
        if (!isGrabbed)
        {
            GrabObject();
        }
        else
        {
            ReleaseObject();
        }
    }

    private void GrabObject()
    {
        isGrabbed = true;
        currentDistance = grabDistance;

        // Prevent player collisions
        gameObject.layer = LayerMask.NameToLayer("GrabbedObject");

        if (rb)
        {
            rb.useGravity = false;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        promptMessage = "Drop";
    }

    private void ReleaseObject()
    {
        isGrabbed = false;

        // Restore collision layer
        gameObject.layer = originalLayer;

        if (rb)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        promptMessage = "Grab";
    }

    private void CalculateTargetPosition()
    {
        targetPosition = playerCamera.position + playerCamera.forward * currentDistance;

        // Wall collision prevention
        RaycastHit hit;
        if (Physics.Linecast(playerCamera.position, targetPosition, out hit, collisionMask))
        {
            currentDistance = Mathf.Clamp(hit.distance * 0.9f, 0.1f, grabDistance);
            targetPosition = playerCamera.position + playerCamera.forward * currentDistance;
        }
    }

    private void ApplySmoothMovement()
    {
        if (rb)
        {
            // Smooth physics-based movement
            rb.MovePosition(Vector3.Lerp(
                rb.position,
                targetPosition,
                smoothFactor * Time.fixedDeltaTime
            ));
        }
        else
        {
            // Smooth transform-based movement
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                smoothFactor * Time.deltaTime
            );
        }
    }
}