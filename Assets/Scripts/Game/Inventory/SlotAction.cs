using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SlotAction : MonoBehaviour
{
    public GameObject ParentSlot;
    public InventorySlot InvS;
    public Player player;
    public Inventory Inv;
    void Start()
    {
        InvS = ParentSlot.GetComponent<InventorySlot>();
        player = GameObject.Find("Player").GetComponent<Player>();
        Inv = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
    }

    void Update()
    {

    }

    public void Use()
    {
        bool isPotion = ShopItemsPool.IsPotion(InvS.ItemID);
        if (isPotion == true)
        {
            Potion P = (Potion)ShopItemsPool.ItemByID(InvS.ItemID);
            {
                player.Hp += P.Amount;
                if (player.Hp > 100)
                {
                    player.Hp = 100;
                }
            }
            InvS.ItemCount--;
            if (InvS.ItemCount == 1)
            {
                ParentSlot.transform.GetChild(2).gameObject.GetComponent<Text>().enabled = false;
            }
            else if (InvS.ItemCount == 0)
            {
                InvS.ItemID = -1;
                ParentSlot.transform.GetChild(1).gameObject.SetActive(false);
            }
            Destroy(gameObject);
        }
        else
        {
            Armor A = (Armor)ShopItemsPool.ItemByID(InvS.ItemID);
            for (int i = 0; i < Inv.Equipment.EquipmentSlotNames.Count; i++)
            {
                if (Inv.Equipment.EquipmentSlotNames[i] == A.SlotName)
                {
                    if (Inv.Equipment.EquipmentSlots[i].GetComponent<EquipSlot>().ItemID != -1)
                    {
                        return;
                    }
                    Inv.Equipment.EquipmentSlots[i].GetComponent<EquipSlot>().ItemID = InvS.ItemID;
                    Inv.Equipment.EquipmentSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(InvS.ItemID).name);
                    Inv.Equipment.EquipmentSlots[i].transform.GetChild(1).gameObject.SetActive(true);
                    InvS.ItemCount--;
                    if (InvS.ItemCount == 1)
                    {
                        ParentSlot.transform.GetChild(2).gameObject.GetComponent<Text>().enabled = false;
                    }
                    else if (InvS.ItemCount == 0)
                    {
                        InvS.ItemID = -1;
                        ParentSlot.transform.GetChild(1).gameObject.SetActive(false);
                    }
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
    public void TakeOne()
    {
        GameObject Item = Instantiate(Inv.DragItem, Input.mousePosition, transform.rotation, GameObject.Find("MoveItem").transform);
        Item.GetComponent<MoveItem>().ParentSlot = ParentSlot;
        Item.GetComponent<MoveItem>().Controller = Inv;
        Item.GetComponent<MoveItem>().TakeOne = true;
        Item.GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(InvS.ItemID).name);
        Item.transform.GetChild(0).GetComponent<Text>().enabled = false;
        ParentSlot.GetComponent<InventorySlot>().ItemCount--;
        ParentSlot.transform.GetChild(2).gameObject.GetComponent<Text>().text = ParentSlot.GetComponent<InventorySlot>().ItemCount.ToString();
        Destroy(gameObject);
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
