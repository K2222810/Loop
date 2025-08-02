using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { 
        Key,
        health,
        coin
    }

    public ItemType itemType;
    public int amount;

}
