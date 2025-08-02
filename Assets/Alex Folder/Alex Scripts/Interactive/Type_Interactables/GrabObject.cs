using UnityEngine;

public class GrabObject : Interactable
{
    private bool isHeld = false;
    private Transform holdParent;
    private Rigidbody rb;
    private Camera playerCamera;

    [SerializeField] private float holdDistance = 2f;
    [SerializeField] private float moveSpeed = 10f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Get player's camera via PlayerLook (you can also set this manually if needed)
        PlayerLook look = FindObjectOfType<PlayerLook>();
        if (look != null)
        {
            playerCamera = look.cam;
            GameObject holdObject = new GameObject("HoldPoint");
            holdObject.transform.SetParent(playerCamera.transform);
            holdObject.transform.localPosition = new Vector3(0, 0, holdDistance);
            holdParent = holdObject.transform;
        }
    }

    private void Update()
    {
        if (isHeld)
        {
            // Smoothly move object to hold position
            Vector3 targetPos = holdParent.position;
            rb.linearVelocity = (targetPos - transform.position) * moveSpeed;
        }
    }

    protected override void Interact()
    {
        if (!isHeld)
            Grab();
        else
            Drop();
    }

    private void Grab()
    {
        isHeld = true;
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.linearDamping = 10f;
    }

    private void Drop()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.freezeRotation = false;
        rb.linearDamping = 0f;
        rb.linearVelocity = Vector3.zero;

        hasBeenInteracted = true;
    }
}
