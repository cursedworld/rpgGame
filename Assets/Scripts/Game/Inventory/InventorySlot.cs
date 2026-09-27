using NUnit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Slot, IPointerClickHandler
{
    public void Awake()
    {
        base.Awake();
        if (GameObject.Find("Hud/Inventory") != null)
        {
            Controller = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
        }
        else
        {
            Controller = GameObject.FindWithTag("Trash").GetComponent<ItemBox>().I;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && ItemID != -1)
        {
            Inventory i = Controller as Inventory;
            for (int j = 0; j < i.ActionWinParent.transform.childCount; j++)
            {
                if (i.ActionWinParent.transform.GetChild(j).GetComponent<SlotAction>().ParentSlot.name == gameObject.name)
                {
                    return;
                }
            }
            GameObject a = Instantiate(i.ActionWin, eventData.position + new Vector2(65, 20), Quaternion.identity, i.ActionWinParent.transform);
            a.GetComponent<SlotAction>().ParentSlot = gameObject;
        }
    }
    protected void Update()
    {
        MoveItem ActiveSlot;
        try
        {
            GameObject A;
            if (GameObject.Find("MoveItem") != null)
            {
                A = GameObject.Find("MoveItem");
            }
            else
            {
                A = GameObject.Find("TrashMoveItem");
            }
            ActiveSlot = A.transform.GetChild(0).gameObject.GetComponent<MoveItem>();
        }
        catch
        {
            ActiveSlot = null;
        }
        if (ActiveSlot != null && Vector3.Distance(transform.position, ActiveSlot.transform.position) < 15 && Input.GetMouseButtonUp(0))
        {
            if (ActiveSlot.ParentSlot == gameObject)
            {
                return;
            }
            EquipSlot PrevEquipSlot = ActiveSlot.ParentSlot.GetComponent<EquipSlot>();
            InventorySlot PrevInventorySlot = ActiveSlot.ParentSlot.GetComponent<InventorySlot>();
            QuickSlot PrevQuickSlot = ActiveSlot.ParentSlot.GetComponent<QuickSlot>();
            Slot PrevSlot = null;
            if (PrevEquipSlot != null)
            {
                PrevSlot = PrevEquipSlot;
                Armor a = (Armor)ShopItemsPool.ItemByID(ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID);
                player.Armor -= a.armor;
            }
            else if (PrevQuickSlot != null)
            {
                PrevSlot = PrevQuickSlot;
            }
            else
            {
                PrevSlot = PrevInventorySlot;
            }
            if (ItemID == ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID || ItemID == -1)
            {
                if (ActiveSlot.TakeOne == true)
                {
                    ItemCount++;
                }
                else if (ActiveSlot.TakeHalf == true)
                {
                    ItemCount += ActiveSlot.Half;
                }
                else
                {
                    ItemCount += PrevSlot.ItemCount;
                }
                if (ItemID == -1)
                {
                    ItemID = ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID;
                    transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(ItemID).name);
                    transform.GetChild(1).gameObject.SetActive(true);
                    if (PrevEquipSlot == null && ActiveSlot.TakeOne == false && ActiveSlot.TakeHalf == false)
                    {
                        PrevSlot.ItemCount = 0;
                    }
                }
                else if (ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID == ItemID)
                {
                    transform.GetChild(2).gameObject.GetComponent<Text>().enabled = true;
                    if (ActiveSlot.TakeOne == false && ActiveSlot.TakeHalf == false)
                    {
                        PrevSlot.ItemCount = 0;
                        ActiveSlot.ParentSlot.transform.GetChild(2).gameObject.GetComponent<Text>().enabled = false;
                    }
                }
                if (PrevSlot.ItemCount == 0)
                {
                    ActiveSlot.ParentSlot.transform.GetChild(1).gameObject.SetActive(false);
                    PrevSlot.ItemID = -1;
                }
            }
            else
            {
                if (PrevSlot.ItemCount == 0)
                {
                    Sprite S = ActiveSlot.ParentSlot.transform.GetChild(1).GetComponent<Image>().sprite;
                    ActiveSlot.ParentSlot.transform.GetChild(1).GetComponent<Image>().sprite = transform.GetChild(1).GetComponent<Image>().sprite;
                    transform.GetChild(1).GetComponent<Image>().sprite = S;
                    int id = ItemID;
                    ItemID = ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID;
                    PrevSlot.ItemID = id;
                }
                if (ActiveSlot.TakeOne == true)
                {
                    if (PrevSlot.ItemCount > 0)
                    {
                        Inventory Inv = Controller as Inventory;
                        for (int i = 0; i < Inv.InventorySlots.Count; i++)
                        {
                            if (Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemID == -1)
                            {
                                Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemID = ItemID;
                                Inv.InventorySlots[i].GetComponent<InventorySlot>().ItemCount = ItemCount;
                                Inv.InventorySlots[i].transform.GetChild(1).gameObject.SetActive(true);
                                Inv.InventorySlots[i].transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(ItemID).name);
                                Inv.InventorySlots[i].transform.GetChild(2).GetComponent<Text>().text = ItemCount.ToString();
                                Sprite S = ActiveSlot.ParentSlot.transform.GetChild(1).GetComponent<Image>().sprite;
                                transform.GetChild(1).GetComponent<Image>().sprite = S;
                                ItemID = PrevSlot.ItemID;
                                ItemCount = PrevSlot.ItemCount;
                                ActiveSlot.DestroyMoveItem();
                                return;
                            }
                        }
                    }
                    else
                    {
                        PrevSlot.ItemCount++;
                    }
                }
                if (PrevEquipSlot != null)
                {
                    Armor a = (Armor)ShopItemsPool.ItemByID(PrevSlot.ItemID);
                    player.Armor += a.armor;
                }
                else
                {
                    string N = ActiveSlot.ParentSlot.transform.GetChild(2).GetComponent<Text>().text;
                    ActiveSlot.ParentSlot.transform.GetChild(2).GetComponent<Text>().text = transform.GetChild(2).GetComponent<Text>().text;
                    transform.GetChild(2).GetComponent<Text>().text = N;
                    int C = PrevSlot.ItemCount;
                    PrevSlot.ItemCount = ItemCount;
                    ItemCount = C;
                    int D = PrevSlot.ItemID;
                    PrevSlot.ItemID = ItemID;
                    ItemID = D;
                    PrevSlot.transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(PrevSlot.ItemID).name);
                    transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(ItemID).name);
                }
            }
            ActiveSlot.DestroyMoveItem();
        }
    }
}
