using UnityEngine;

public class ShopSlot : Slot
{
    public void Awake()
    {
        base.Awake();
        Controller = GameObject.Find("Hud/Shop").GetComponent<ShopController>();
    }
}
