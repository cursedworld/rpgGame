using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Description;
    public EquipSlot EquipSlot;
    void Start()
    {
        EquipSlot = transform.parent.GetComponent<EquipSlot>();
    }

    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData data)
    {
        if (EquipSlot.ItemID != -1)
        {
            Description = Instantiate(EquipSlot.Inventory.Description, transform.position - new Vector3(100, 0, 0), transform.rotation, transform);
            Description.transform.GetChild(0).GetComponent<Text>().text = ShopItemsPool.ItemByID(EquipSlot.ItemID).name;
            Armor a = (Armor)ShopItemsPool.ItemByID(EquipSlot.ItemID);
            Description.transform.GetChild(1).GetComponent<Text>().text = "Armor: " + a.armor;
            Description.transform.GetChild(2).GetComponent<Text>().text = "Weight: " + a.Weight + "kg";
        }
    }
    public void OnPointerExit(PointerEventData data)
    {
        if (EquipSlot.ItemID != -1)
        {
            Destroy(Description);
        }
    }
    public void OnDisable()
    {
        if (Description != null)
        {
            Destroy(Description);
        }
    }
}
