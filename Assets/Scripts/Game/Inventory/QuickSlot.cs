using UnityEngine.UI;

public class QuickSlot : Slot
{
    //void Update()
    //{
    //    base.Update();
    //}
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
