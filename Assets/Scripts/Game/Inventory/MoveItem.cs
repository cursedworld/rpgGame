using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveItem : MonoBehaviour
{
    public SlotController Controller;
    public Color UnActiveColor;
    public GameObject ParentSlot;
    public GameObject EquipmentSlot;
    public bool IsEquipment = false;
    public bool TakeOne = false;
    public bool TakeHalf = false;
    public int Half;

    private void Start()
    {

    }
    void Update()
    {
        transform.position = Input.mousePosition;
        PointerEventData eventData = new(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> R = new();
        EventSystem.current.RaycastAll(eventData, R);
        if (Input.GetMouseButtonDown(0))
        {
            if (R.Count < 2)
            {
                if (TakeOne == true)
                {
                    ParentSlot.GetComponent<InventorySlot>().ItemCount++;
                }
                else if (TakeHalf == true)
                {
                    ParentSlot.GetComponent<InventorySlot>().ItemCount+= ParentSlot.GetComponent<InventorySlot>().ItemCount/2;
                }
                DestroyMoveItem();
            }
        }
    }
    public void DestroyMoveItem()
    {
        ParentSlot.transform.GetChild(0).GetComponent<Image>().color = UnActiveColor;
        Controller.ActiveSlot = null;
        Controller.ActiveID = -1;
        Controller.PreviousActiveSlot = null;
        Destroy(gameObject);
    }
}
