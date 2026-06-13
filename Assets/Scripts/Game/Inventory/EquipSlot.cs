using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : Slot
{
    public EquipmentInventory Inventory;
    public int TypeID;
    public Inventory Inv;

    public void Awake()
    {
        base.Awake();
        Controller = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
        Inv = Controller as Inventory;
        Inventory = GameObject.Find("Hud/PlayerStats").GetComponent<EquipmentInventory>();
    }
    void Update()
    {
        MoveItem ActiveSlot;
        try
        {
            ActiveSlot = GameObject.Find("Hud/Inventory/MoveItem").transform.GetChild(0).gameObject.GetComponent<MoveItem>();
        }
        catch
        {
            ActiveSlot = null;
        }
        if (ActiveSlot != null && Vector3.Distance(transform.position, ActiveSlot.transform.position) < 15 &&
            Input.GetMouseButtonUp(0) && !ActiveSlot.ParentSlot.CompareTag("EquipSlot") &&
            ItemID != ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID)    
        {
            int SlotNumber = Convert.ToInt32(ActiveSlot.ParentSlot.gameObject.name.Replace("Slot", "")) - 1;
            if (ShopItemsPool.ItemByID(ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID).type == "Belt")
            {
                int OldID = ItemID;
                ItemID = ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID;
                Armor a = (Armor)ShopItemsPool.ItemByID(ItemID);
                player.Armor += a.armor;
                transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(ItemID).name);
                transform.GetChild(1).gameObject.SetActive(true);
                Inv.InventorySlots[SlotNumber].GetComponent<InventorySlot>().ItemCount--;
                if (Inv.InventorySlots[SlotNumber].GetComponent<InventorySlot>().ItemCount <= 1)
                {
                    Inv.InventorySlots[SlotNumber].transform.GetChild(2).gameObject.GetComponent<Text>().enabled = false;
                    if (Inv.InventorySlots[SlotNumber].GetComponent<InventorySlot>().ItemCount <= 0)
                    {
                        Inv.InventorySlots[SlotNumber].GetComponent<InventorySlot>().ItemCount = 0;
                        Inv.InventorySlots[SlotNumber].transform.GetChild(1).gameObject.SetActive(false);
                        Inv.InventorySlots[SlotNumber].GetComponent<InventorySlot>().ItemID = -1;
                        Inv.InventorySlots[SlotNumber].GetComponent<InventorySlot>().ItemCount = -1;
                    }
                }
                if (OldID != -1)
                {
                    Armor olda = (Armor)ShopItemsPool.ItemByID(OldID);
                    player.Armor -= olda.armor;
                    bool inInventory = false;
                    for (int i = 0; i < Inv.InventorySlots.Count; i++)
                    {
                        if (Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemID == OldID)
                        {
                            Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemCount++;
                            inInventory = true;
                            break;
                        }
                    }
                    if (!inInventory)
                    {
                        for (int i = 0; i < Inv.InventorySlots.Count; i++)
                        {
                            if (Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemID == -1)
                            {
                                Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemID = OldID;
                                Inv.InventorySlots[i].transform.GetChild(1).gameObject.SetActive(true);
                                Inv.InventorySlots[i].transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(OldID).name);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
