using NUnit;
using UnityEngine;
using UnityEngine.UI;

public class DivideWindow : MonoBehaviour
{
    public int ItemCount = 0;
    public int ItemID = -1;
    public Inventory Inv;
    public GameObject ParentSlot;
    void Start()
    {
        Inv = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
    }

    void Update()
    {
        
    }

    public void Accept()
    {
        if (transform.GetChild(4).GetComponent<Slider>().value == 0)
        {
            Destroy(gameObject);
            return;
        }
        GameObject Item = Instantiate(Inv.DragItem, Input.mousePosition, transform.rotation, GameObject.Find("MoveItem").transform);
        Item.GetComponent<MoveItem>().ParentSlot = ParentSlot;
        Item.GetComponent<MoveItem>().Controller = Inv;
        Item.GetComponent<MoveItem>().TakeHalf = true;
        Item.GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(ItemID).name);
        Item.transform.GetChild(0).GetComponent<Text>().enabled = false;
        Item.GetComponent<MoveItem>().Half = (int)transform.GetChild(4).GetComponent<Slider>().value;
        ParentSlot.GetComponent<InventorySlot>().ItemCount -= (int)transform.GetChild(4).GetComponent<Slider>().value;
        ParentSlot.transform.GetChild(2).gameObject.GetComponent<Text>().text = ParentSlot.GetComponent<InventorySlot>().ItemCount.ToString();
        Destroy(gameObject);
    }

    public void Decline()
    {
        Destroy(gameObject);
    }

    public void ChangeSliderValue()
    {
        transform.GetChild(5).GetComponent<Text>().text = transform.GetChild(4).GetComponent<Slider>().value + "/" + ItemCount.ToString();
    }
}
