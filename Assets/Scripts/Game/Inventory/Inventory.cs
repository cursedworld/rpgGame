using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SlotController
{
    public List<GameObject> InventorySlots;
    public ShopItemsPool SP;
    public GameObject DragItem;
    void Update()
    {

    }

    public void ItemToInventory(ShopController shop)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if (InventorySlots[i].GetComponent<InventorySlot>().ItemID == shop.ActiveID)
            {
                InventorySlots[i].GetComponent<InventorySlot>().ItemCount++;
                return;
            }
        }
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if (InventorySlots[i].GetComponent<InventorySlot>().ItemID == -1)
            {
                InventorySlots[i].GetComponent<InventorySlot>().ItemID = shop.ActiveID;
                InventorySlots[i].transform.GetChild(1).gameObject.SetActive(true);
                InventorySlots[i].transform.GetChild(1).gameObject.GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(InventorySlots[i].GetComponent<InventorySlot>().ItemID).name);
                InventorySlots[i].GetComponent<InventorySlot>().ItemCount = 1;
                return;
            }
        }
    }
}
