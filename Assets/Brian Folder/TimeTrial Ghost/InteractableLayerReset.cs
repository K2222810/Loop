using UnityEngine;

public class InteractableLayerReset : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void ResetState()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
