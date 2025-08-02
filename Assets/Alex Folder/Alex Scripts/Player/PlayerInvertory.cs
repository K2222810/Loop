using UnityEngine;

public class PlayerInvertory : MonoBehaviour
{   
    private Inventory inventory;
    [SerializeField] private UI_Inventory UIinventory;

    private void Awake()
    {
        inventory = new Inventory();
        UIinventory.SetInventory(inventory);   
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
