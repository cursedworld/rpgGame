using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : Slot
{
    public void Awake()
    {
        base.Awake();
        Controller = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
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
            Slot PrevSlot = null;
            if (PrevEquipSlot != null)
            {
                PrevSlot = PrevEquipSlot;
            }
            else
            {
                PrevSlot = PrevInventorySlot;
            }
            if (ItemID == ActiveSlot.ItemID || ItemID == -1)
            {
                if (ItemID == -1)
                {
                    ItemCount = PrevSlot.ItemCount;
                    ItemID = ActiveSlot.ItemID;
                    transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(ItemID).name);
                    transform.GetChild(1).gameObject.SetActive(true);
                    if (PrevEquipSlot == null)
                    {
                        PrevSlot.ItemCount = 0;
                    }
                }
                else if (ActiveSlot.ItemID == ItemID)
                {
                    ItemCount++;
                    transform.GetChild(2).gameObject.GetComponent<Text>().enabled = true;
                }
                ActiveSlot.ParentSlot.transform.GetChild(1).gameObject.SetActive(false);
                PrevSlot.ItemID = -1;
            }
            else
            {
                Sprite S = ActiveSlot.ParentSlot.transform.GetChild(1).GetComponent<Image>().sprite;
                ActiveSlot.ParentSlot.transform.GetChild(1).GetComponent<Image>().sprite = transform.GetChild(1).GetComponent<Image>().sprite;
                transform.GetChild(1).GetComponent<Image>().sprite = S;
                int id = ItemID;
                ItemID = ActiveSlot.ItemID;
                PrevSlot.ItemID = id;
                string N = ActiveSlot.ParentSlot.transform.GetChild(2).GetComponent<Text>().text; 
                ActiveSlot.ParentSlot.transform.GetChild(2).GetComponent<Text>().text = transform.GetChild(2).GetComponent<Text>().text;
                transform.GetChild(2).GetComponent<Text>().text = N;
                int C = PrevSlot.ItemCount;
                PrevSlot.ItemCount = ItemCount;
                ItemCount = C;
            }
        }
    }
}
