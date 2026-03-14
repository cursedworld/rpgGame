using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : Slot
{
    public void Awake()
    {
        base.Awake();
        Controller = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
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
