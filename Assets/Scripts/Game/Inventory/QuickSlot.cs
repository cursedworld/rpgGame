using System;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : Slot
{
    public Inventory Inv;
    public QuickSlotManager QS;
    public void Awake()
    {
        base.Awake();
        Controller = GameObject.Find("Hud/QuickSlots").GetComponent<QuickSlotManager>();
        Inv = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
        QS = Controller as QuickSlotManager;
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
        if (ActiveSlot != null && Vector3.Distance(transform.position, ActiveSlot.transform.position) < 15 && Input.GetMouseButtonUp(0))
        {
            QuickSlot PrevQuickSlot = ActiveSlot.ParentSlot.GetComponent<QuickSlot>();
            InventorySlot PrevInventorySlot = ActiveSlot.ParentSlot.GetComponent<InventorySlot>();
            int SlotNumber = Convert.ToInt32(ActiveSlot.ParentSlot.gameObject.name.Replace("Slot", "")) - 1;
            if (ActiveSlot.ParentSlot == gameObject)
            {
                return;
            }
            if (ShopItemsPool.IsPotion(ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID))
            {
                int OldItemCount = ItemCount;
                int OldID = ItemID;
                ItemID = ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID;
                ItemCount = ActiveSlot.ParentSlot.GetComponent<Slot>().ItemCount;
                transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(ItemID).name);
                transform.GetChild(1).gameObject.SetActive(true);
                if (OldID != -1)
                {
                    if (PrevInventorySlot != null)
                    {
                        PrevInventorySlot.ItemID = -1;
                        ActiveSlot.ParentSlot.transform.GetChild(1).gameObject.SetActive(false);
                        if (OldID == ItemID)
                        {
                            ItemCount += OldItemCount;
                        }
                        else
                        {
                            ActiveSlot.ParentSlot.transform.GetChild(2).GetComponent<Text>().enabled = false;
                            for (int i = 0; i < Inv.InventorySlots.Count; i++)
                            {
                                if (Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemID == -1)
                                {
                                    Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemCount = OldItemCount;
                                    Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemID = OldID;
                                    Inv.InventorySlots[i].transform.GetChild(1).gameObject.SetActive(true);
                                    Inv.InventorySlots[i].transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(OldID).name);
                                    break;
                                }
                            }
                        }

                    }
                    else
                    {
                        if (OldID == ItemID)
                        {
                            ItemCount += OldItemCount;
                            QS.slots[SlotNumber].GetComponent<Slot>().ItemCount = 0;
                            QS.slots[SlotNumber].GetComponent<Slot>().ItemID = -1;
                            ActiveSlot.ParentSlot.transform.GetChild(1).gameObject.SetActive(false);
                            QS.slots[SlotNumber].transform.GetChild(2).gameObject.GetComponent<Text>().enabled = false;
                        }
                        else
                        {
                            PrevQuickSlot.ItemID = OldID;
                            PrevQuickSlot.ItemCount = OldItemCount;
                            ActiveSlot.ParentSlot.transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(OldID).name);
                        }
                    }
                }
                else
                {
                    if (PrevQuickSlot != null)
                    {
                        QS.slots[SlotNumber].GetComponent<Slot>().ItemCount -= ItemCount;
                        if (QS.slots[SlotNumber].GetComponent<Slot>().ItemCount <= 1)
                        {
                            QS.slots[SlotNumber].transform.GetChild(2).gameObject.GetComponent<Text>().enabled = false;
                            if (QS.slots[SlotNumber].GetComponent<Slot>().ItemCount <= 0)
                            {
                                QS.slots[SlotNumber].GetComponent<Slot>().ItemCount = 0;
                                QS.slots[SlotNumber].transform.GetChild(1).gameObject.SetActive(false);
                                QS.slots[SlotNumber].GetComponent<Slot>().ItemID = -1;
                                QS.slots[SlotNumber].GetComponent<Slot>().ItemCount = -1;
                            }
                        }
                    }
                    else
                    {
                        SlotNumber = Convert.ToInt32(ActiveSlot.ParentSlot.gameObject.name.Replace("Slot", "")) - 1;
                        Inv.InventorySlots[SlotNumber].GetComponent<InventorySlot>().ItemCount -= ItemCount;
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
                    }
                }
            }
        }
    }
    public void DeletItem()
    {
        itemCount--;
        if (itemCount <= 0)
        {
            ItemID = -1;
            transform.GetChild(1).GetComponent<Image>().sprite = null;
        }
    }
}
