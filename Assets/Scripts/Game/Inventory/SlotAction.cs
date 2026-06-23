using UnityEngine;
using UnityEngine.UI;

public class SlotAction : MonoBehaviour
{
    public GameObject ParentSlot;
    public InventorySlot InvS;
    public Player player;
    void Start()
    {
        InvS = ParentSlot.GetComponent<InventorySlot>();
        player = GameObject.Find("Player").GetComponent<Player>();
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
            Armor A = (Armor)ShopItemsPool.ItemByID(InvS.ItemID); //
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
