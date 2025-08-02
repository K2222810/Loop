using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> itemList;
    public Inventory()
    {
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.Key, amount = 1 });
 
    }
    public void AddItem(Item item) { 
    
        itemList.Add(item); 
    }
}

