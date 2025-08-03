    using UnityEngine;

public class DoorInteractable : Interactable
{
    private bool isOpen = false;

    protected override void Interact()
    {
        PlayerInventoryData playerData = FindObjectOfType<PlayerInventoryData>();

        if (playerData != null && playerData.hasKey)
        {
            if (!isOpen)
            {
                Debug.Log("Door opened!");
                OpenDoor();
                isOpen = true;
            }
        }
        else
        {
            Debug.Log("You need a key to open this door.");
        }
    }

    private void OpenDoor()
    {
        // Play animation, move the door, or disable collider
        // For example:
        transform.position += Vector3.up * 3f;
    }
}
