using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : SlotController
{
    public GameObject ActionWin;
    public List<GameObject> InventorySlots;
    public ShopItemsPool SP;
    public GameObject ActionWinParent;

    void Start()
    {
        ActionWinParent = GameObject.Find("Hud/Inventory/ActionWindow");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            PointerEventData eventData = new(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> R = new();
            EventSystem.current.RaycastAll(eventData, R);
            bool PressButton = false;
            for ( int i = 0; i < R.Count; i++ )
            {
                if (R[i].gameObject.transform.parent.name.Contains("Action"))
                {
                    PressButton = true;
                    break;
                }
            }
            if (!Input.GetMouseButtonDown(0) || (Input.GetMouseButtonDown(0)) && PressButton == false)
            {
                if (R.Count == 0)
                {
                    for (int i = 0; i < ActionWinParent.transform.childCount; i++)
                    {
                        Destroy(ActionWinParent.transform.GetChild(i).gameObject);
                    }
                }
                else
                {
                    for (int r = 0; r < R.Count; r++)
                    {
                        if (!R[r].gameObject.transform.parent.name.Contains("Slot"))
                        {
                            R.RemoveAt(r);
                        }
                    }
                    for (int i = 0; i < ActionWinParent.transform.childCount; i++)
                    {
                        bool hasParent = false;
                        for (int r = 0; r < R.Count; r++)
                        {
                            if (ActionWinParent.transform.GetChild(i).GetComponent<SlotAction>().ParentSlot.name == R[r].gameObject.transform.parent.name)
                            {
                                hasParent = true;
                                break;
                            }
                        }
                        if (hasParent == false)
                        {
                            Destroy(ActionWinParent.transform.GetChild(i).gameObject);
                        }
                    }
                }
            }
        }
    }

    public void ItemToInventory(ShopController shop, int Count)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if (InventorySlots[i].GetComponent<InventorySlot>().ItemID == shop.ActiveID)
            {
                InventorySlots[i].GetComponent<InventorySlot>().ItemCount += Count;
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
                InventorySlots[i].GetComponent<InventorySlot>().ItemCount = Count;
                return;
            }
        }
    }
}
