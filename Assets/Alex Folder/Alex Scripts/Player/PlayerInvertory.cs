using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UI_Inventory uiInventory;
    private Inventory inventory;
 

    private void Awake()
    {
        inventory = new Inventory(UseItem);


        if (uiInventory != null)
        {
            uiInventory.SetInventory(inventory);
        }
        else
        {
            Debug.LogWarning("UI_Inventory not assigned in PlayerInventory.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;

            default:
                Debug.Log("Used item: " + item.itemType);
                break;
        }
    }

    public Inventory GetInventory() => inventory;
}
