using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : SlotController
{
    public ShopGenerator CurrentShop;
    public List<GameObject> ShopSlots;
    public GameObject BuyButton;
    public UIManager UI;
    public Player player;
    public Inventory inv;
    public GameObject Shop;
    public Color SoldColor;
    public Text Money;

    private void Awake()
    {
        inv = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
        Money = GameObject.Find("Shop/Balance").GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Start()
    {
        for (int i = 0; i < GameObject.Find("Hud/Shop/ShopSlots").transform.childCount; i++)
        {
            ShopSlots.Add(GameObject.Find("Hud/Shop/ShopSlots").transform.GetChild(i).gameObject);
        }
        BuyButton = GameObject.Find("Hud/Shop/Buy");
        UI = GameObject.Find("Hud").GetComponent<UIManager>();
        Shop = gameObject;
        Shop.SetActive(false);
    }
    
    void Update()
    {
        if(ActiveSlot == null)
        {
            BuyButton.GetComponent<Button>().interactable = false;
        }
        else if(ActiveSlot != null)
        {
            BuyButton.GetComponent<Button>().interactable = true;
        }
        if(CurrentShop != null)
        {
            for(int i = 0; i < ShopSlots.Count; i++)
            {
                if (CurrentShop.CurrentShop.Count > i && CurrentShop.CurrentShop[i].ItemCount > 0)
                {
                    ShopSlots[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                    ShopSlots[i].transform.GetComponent<Button>().enabled = true;
                    ShopSlots[i].transform.GetChild(2).GetComponent<Text>().enabled = true;
                    ShopSlots[i].transform.GetChild(3).GetComponent<Text>().enabled = true;
                }
                else
                {
                    ShopSlots[i].transform.GetChild(1).GetComponent<Image>().color = SoldColor;
                    ShopSlots[i].transform.GetComponent<Button>().enabled = false;
                    ShopSlots[i].transform.GetChild(2).GetComponent<Text>().enabled = false;
                    ShopSlots[i].transform.GetChild(3).GetComponent<Text>().text = "Sold";
                }
            }
        }
    }

    public void OnEnable()
    {
        Money.text = "Money: " + player.Coin;
    }

    public void ShowShop(ShopGenerator S)
    {
        CurrentShop = S;
        for( int i = 0; i < S.CurrentShop.Count; i++)
        {
            ShopSlots[i].GetComponent<ShopSlot>().ItemID = S.CurrentShop[i].item.id;
            ShopSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(S.CurrentShop[i].item.name);
            ShopSlots[i].transform.GetChild(2).GetComponent<Text>().text = S.CurrentShop[i].ItemCount.ToString();
            ShopSlots[i].transform.GetChild(3).GetComponent<Text>().text = S.CurrentShop[i].item.price.ToString();
        }
        for (int i = S.CurrentShop.Count; i < ShopSlots.Count; i++)
        {
            ShopSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
            ShopSlots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            ShopSlots[i].transform.GetComponent<Button>().enabled = false;
            ShopSlots[i].transform.GetChild(2).GetComponent<Text>().enabled = false;
            ShopSlots[i].transform.GetChild(3).GetComponent<Text>().enabled = false;
        }
        Shop.SetActive(true);
    }

    public void CloseShop()
    {
        for (int i = 0; i < ShopSlots.Count; i++)
        {
            if (ShopSlots[i].GetComponent<ShopSlot>().ItemID == ActiveID)
            {
                ShopSlots[i].transform.GetChild(0).GetComponent<Image>().color = ShopSlots[i].GetComponent<ShopSlot>().UnActiveColor;
                ActiveID = -1;
                ActiveSlot = null;
                break;
            }
        }
        Shop.SetActive(false);
    }
    public void WhenBuy()
    {
        UI.WhenBuyText.GetComponent<FadeText>().enabled = true;
        if (player.Coin < ShopItemsPool.ItemByID(ActiveID).price)
        {
            UI.WhenBuyText.GetComponent<Text>().text = "You have not enought money! You need" + (ShopItemsPool.ItemByID(ActiveID).price - player.Coin).ToString() + " coins";
        }
        else
        {
            UI.WhenBuyText.GetComponent<Text>().text = "You succesfully bought an item";
            player.Coin -= ShopItemsPool.ItemByID(ActiveID).price;
            Money.text = "Money: " + player.Coin;
            inv.ItemToInventory(this);
            for (int i = 0; i < ShopSlots.Count; i++)
            {
                if (ShopSlots[i].GetComponent<ShopSlot>().ItemID == ActiveID)
                {
                    CurrentShop.CurrentShop[i].ItemCount--;
                    ShopSlots[i].transform.GetChild(2).GetComponent<Text>().text = CurrentShop.CurrentShop[i].ItemCount.ToString();
                    if (CurrentShop.CurrentShop[i].ItemCount <= 0)
                    {
                        ShopSlots[i].transform.GetChild(0).GetComponent<Image>().color = ShopSlots[i].GetComponent<ShopSlot>().UnActiveColor;
                        ActiveID = -1;
                        ActiveSlot = null;
                    }
                    break;
                }
            }
        }
        UI.WhenBuyText.GetComponent<FadeText>().Show = true;
    }
}
