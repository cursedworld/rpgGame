using UnityEngine;
using UnityEngine.UI;

public class SlotAction : MonoBehaviour
{
    public GameObject ParentSlot;
    public InventorySlot InvS;
    public Player player;
    public Inventory Inv;
    public GameObject Trash;
    public GameObject CountWindow;
    public Transform DivideWindow;
    public DropManager DropM;
    public float Step = 35f;
    void Start()
    {
        InvS = ParentSlot.GetComponent<InventorySlot>();
        player = GameObject.Find("Player").GetComponent<Player>();
        Inv = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
        DivideWindow = GameObject.Find("Hud/Inventory/DivideWindow").transform;
        DropM = GameObject.Find("Hud").GetComponent<UIManager>().DropM;
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
        if (InvS.ItemCount == 1)
        {
            TakeOne();
            return;
        }
        GameObject Item = Instantiate(Inv.DragItem, Input.mousePosition, transform.rotation, GameObject.Find("MoveItem").transform);
        Item.GetComponent<MoveItem>().ParentSlot = ParentSlot;
        Item.GetComponent<MoveItem>().Controller = Inv;
        Item.GetComponent<MoveItem>().TakeHalf = true;
        Item.GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(InvS.ItemID).name);
        Item.transform.GetChild(0).GetComponent<Text>().enabled = false;
        float half = ParentSlot.GetComponent<InventorySlot>().ItemCount / 2f;
        Item.GetComponent<MoveItem>().Half = (int)half;
        ParentSlot.GetComponent<InventorySlot>().ItemCount -= (int)Mathf.Floor(half);
        ParentSlot.transform.GetChild(2).gameObject.GetComponent<Text>().text = ParentSlot.GetComponent<InventorySlot>().ItemCount.ToString();
        Destroy(gameObject);
    }
    public void Drop()
    {
        InvS.transform.GetChild(1).gameObject.SetActive(false);
        GameObject box = player.FindLootBox();
        if (box != null)
        {
            box.GetComponent<ItemBox>().CurrentItems.Add(InvS.ItemID, InvS.ItemCount);
        }
        else
        {
            box = Instantiate(Trash, GameObject.Find("Player").transform.position, Quaternion.identity);
            box.GetComponent<ItemBox>().I = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
            box.GetComponent<ItemBox>().CurrentItems.Add(InvS.ItemID, InvS.ItemCount);
        }
        if (box.GetComponent<ItemBox>().CurrentItems.Count > 9)
        {
            DropM.gameObject.GetComponent<ScrollRect>().enabled = true;
            DropM.Scroll.SetActive(true);
            DropM.Slots.transform.parent = DropM.ScrollSpawn;
            DropM.Slots.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -55, 0);
            GameObject S = Instantiate(DropM.DropSlot, DropM.Slots.transform);
            DropM.DropSlots.Add(S);
            S.GetComponent<RectTransform>().anchoredPosition = new Vector3(15 + Step * DropM.SlotX, -15 + -Step * DropM.SlotY, 0);
            DropM.SlotX++;
            if(DropM.SlotX > 2)
            {
                DropM.SlotX = 0;
                DropM.SlotY++;
            }
        }
        InvS.ItemCount = 0;
        InvS.ItemID = -1;
    }
    public void Divide()
    {
        if (DivideWindow.transform.childCount != 0)
        {
            return;
        }
        GameObject CW = Instantiate(CountWindow, DivideWindow.position, Quaternion.identity, DivideWindow);
        CW.transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(InvS.ItemID).name);
        CW.transform.GetChild(4).GetComponent<Slider>().maxValue = InvS.ItemCount;
        CW.transform.GetChild(4).GetComponent<Slider>().value = InvS.ItemCount / 2;
        CW.transform.GetChild(5).GetComponent<Text>().text = CW.transform.GetChild(4).GetComponent<Slider>().value + "/" + InvS.ItemCount.ToString();
        CW.GetComponent<DivideWindow>().ItemCount = InvS.ItemCount;
        CW.GetComponent<DivideWindow>().ItemID = InvS.ItemID;
        CW.GetComponent<DivideWindow>().ParentSlot = ParentSlot;
    }
}
