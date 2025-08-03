using UnityEngine;

public class KeyPickup : Interactable
{
    protected override void Interact()
    {
        PlayerInventoryData playerData = FindObjectOfType<PlayerInventoryData>();
        if (playerData != null)
        {
            playerData.hasKey = true;
            Debug.Log("Key picked up!");
            Destroy(gameObject); // remove key from the scene
        }
    }
}
