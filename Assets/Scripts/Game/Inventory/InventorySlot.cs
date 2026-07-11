using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Slot, IPointerClickHandler
{
    public void Awake()
    {
        base.Awake();
        Controller = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
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
            ActiveSlot = GameObject.Find("Hud/Inventory/MoveItem").transform.GetChild(0).gameObject.GetComponent<MoveItem>();
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
                if (ItemID == -1)
                {
                    if (ActiveSlot.TakeOne == true)
                    {
                        ItemCount++;
                    }
                    else
                    {
                        ItemCount = PrevSlot.ItemCount;
                    }
                    ItemID = ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID;
                    transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(ItemID).name);
                    transform.GetChild(1).gameObject.SetActive(true);
                    if (PrevEquipSlot == null && ActiveSlot.TakeOne == false)
                    {
                        PrevSlot.ItemCount = 0;
                    }
                }
                else if (ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID == ItemID)
                {
                    ItemCount += PrevSlot.ItemCount;
                    if (ActiveSlot.TakeOne == true)
                    {
                        ItemCount++;
                    }
                    transform.GetChild(2).gameObject.GetComponent<Text>().enabled = true;
                    PrevSlot.ItemCount = 0;
                    ActiveSlot.ParentSlot.transform.GetChild(2).gameObject.GetComponent<Text>().enabled = false;
                }
                if (PrevSlot.ItemCount == 0)
                {
                    ActiveSlot.ParentSlot.transform.GetChild(1).gameObject.SetActive(false);
                    PrevSlot.ItemID = -1;
                }
            }
            else ///////////
            {
                Sprite S = ActiveSlot.ParentSlot.transform.GetChild(1).GetComponent<Image>().sprite;
                ActiveSlot.ParentSlot.transform.GetChild(1).GetComponent<Image>().sprite = transform.GetChild(1).GetComponent<Image>().sprite;
                transform.GetChild(1).GetComponent<Image>().sprite = S;
                int id = ItemID;
                ItemID = ActiveSlot.ParentSlot.GetComponent<Slot>().ItemID;
                PrevSlot.ItemID = id;
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
                }
            }
            ActiveSlot.DestroyMoveItem();
        }
    }
}
