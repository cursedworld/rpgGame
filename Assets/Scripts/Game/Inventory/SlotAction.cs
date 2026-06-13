using UnityEngine;
using UnityEngine.UI;

public class SlotAction : MonoBehaviour
{
    public GameObject ParentSlot;
    public InventorySlot InvS;
    void Start()
    {
        InvS = ParentSlot.GetComponent<InventorySlot>();
    }

    void Update()
    {
        
    }

    public void Use()
    {
        bool isPotion = ShopItemsPool.IsPotion(InvS.ItemID);
        if(isPotion == true)
        {
            InvS.ItemCount--;
            if (InvS.ItemCount == 1)
            {
                ParentSlot.transform.GetChild(2).gameObject.GetComponent<Text>().enabled = false;
            }
            else if (InvS.ItemCount == 0)
            {
                ParentSlot.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
    public void TakeOne()
    {

    }
    public void TakeHalf()
    {

    }
    public void Drop()
    {

    }
    public void Divide()
    {

    }
}
