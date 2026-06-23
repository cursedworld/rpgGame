using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    public List<GameObject> EquipmentSlots;
    public GameObject Description;
    public Dictionary<int, string> EquipmentSlotNames;
    void Start()
    {
        EquipmentSlotNames = new()
        {
            {0, "Helmet" },
            {1, "BodyArmor" },
            {2, "Gloves" },
            {3, "Pants" },
            {4, "Boots" },
        };
        for (int i = 0; i < EquipmentSlots.Count; i++)
        {
            if (EquipmentSlots[i].GetComponent<EquipSlot>().ItemID == -1)
            {
                EquipmentSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
    void Update()
    {
        
    }
}
