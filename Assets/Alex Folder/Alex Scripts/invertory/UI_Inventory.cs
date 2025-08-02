using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
   private Inventory inventory;
   private Transform itemSlotContainer;
   private Transform itemSlotTemplate;


    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = transform.Find("itemSlotTemplate");
    }
    public void SetInventory(Inventory inventory){
       this.inventory = inventory; 
   }

    private void RefreshInventroyItems() 
    {   
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 30f;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotContainer, itemSlotTemplate).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            x++;
            if (x > 4) 
            {
                x = 0;
                y++;
            }
                

        }
    } 




}
