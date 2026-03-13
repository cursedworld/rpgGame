using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool Active = false;
    public int ItemID = -1;
    public int itemCount = 0;
    public bool FirstClick = true;
    public SlotController Controller;
    public Player player;
    public Color ActiveColor = new Color32(156, 136, 57, 255);
    public Color UnActiveColor = new Color32(31, 31, 31, 255);
    public int ItemCount
    {
        get
        {
            return itemCount;
        }
        set
        {
            itemCount = value;
            transform.GetChild(2).GetComponent<Text>().text = itemCount.ToString();
            if (itemCount > 1)
            {
                transform.GetChild(2).GetComponent<Text>().enabled = true;
            }
            else
            {
                transform.GetChild(2).GetComponent<Text>().enabled = false;
            }
        }
    }
    protected void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData data)
    {
        Active = true;
        if (Controller is not ShopController)
        {
            StartCoroutine(MoveItem());
        }
    }
    public void OnPointerExit(PointerEventData data)
    {
        Active = false;
        if (Controller is not ShopController)
        {
            StopCoroutine(MoveItem());
        }
    }
    public IEnumerator MoveItem()
    {
        while (Active && Controller.ActiveSlot == null)
        {
            if (Input.GetMouseButton(0) && ItemID != -1)
            {
                transform.GetChild(0).GetComponent<Image>().color = ActiveColor;
                Controller.ActiveSlot = gameObject;
                Inventory Inv = Controller as Inventory;
                Image Item = Instantiate(Inv.DragItem, Input.mousePosition, transform.rotation, Controller.gameObject.transform.Find("MoveItem").transform).GetComponent<Image>();
                Item.sprite = transform.GetChild(1).GetComponent<Image>().sprite;
                Item.GetComponent<MoveItem>().ParentSlot = gameObject;
                Item.GetComponent<MoveItem>().ItemID = ItemID;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void Click()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            if (FirstClick && ItemID != -1)
            {
                transform.GetChild(0).GetComponent<Image>().color = ActiveColor;
                FirstClick = false;
                if (Controller.PreviousActiveSlot != null)
                {
                    Controller.PreviousActiveSlot.GetComponent<Slot>().DeactivateSlot();
                }
                if (Controller.ActiveSlot == null)
                {
                    Controller.PreviousActiveSlot = gameObject;
                }
                Controller.PreviousActiveSlot = gameObject;
                Controller.ActiveSlot = gameObject;
                Controller.ActiveID = ItemID;
            }
            else if (!FirstClick)
            {
                DeactivateSlot();
            }
        }
    }
    public void DeactivateSlot()
    {
        transform.GetChild(0).GetComponent<Image>().color = UnActiveColor;
        FirstClick = true;
        Controller.ActiveSlot = null;
        Controller.PreviousActiveSlot = null;
        Controller.ActiveID = -1;
    }
}
