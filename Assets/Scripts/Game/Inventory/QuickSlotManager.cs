using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : SlotController
{
    public List<GameObject> slots;
    public Color ActiveColor;
    public Color UnActiveColor;
    public List<KeyCode> Keys;
    public Player player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Keys = new()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0,
        };
        for (int i = 0; i < transform.childCount; i++)
        {
            slots.Add(transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        if ( Input.anyKeyDown)
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                if (Input.GetKeyDown(Keys[i]) && slots[i].GetComponent<QuickSlot>().ItemID != -1)
                {
                    ActiveSlot(slots[i].GetComponent<QuickSlot>().ItemID);
                    slots[i].GetComponent<QuickSlot>().DeletItem();
                }
            }
        } 
    }
    public void ActiveSlot(int id)
    {
        Potion P = (Potion)ShopItemsPool.ItemByID(id);
        switch (P.type)
        {
            case "Heal":
                {
                    player.Hp += P.Amount;
                    if (player.Hp > 100)
                    {
                        player.Hp = 100;
                    }
                    break;
                }
        }
    }
}
