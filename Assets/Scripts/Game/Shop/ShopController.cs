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
    public GameObject purchaseWin;
    public Color defaultColor = Color.white;
    private void Awake()
    {
        purchaseWin = GameObject.Find("Hud/Shop/CountWindow");
        inv = GameObject.Find("Hud/Inventory").GetComponent<Inventory>();
        Money = GameObject.Find("Shop/Balance").GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
        for (int i = 0; i < GameObject.Find("Hud/Shop/ShopSlots").transform.childCount; i++)
        {
            ShopSlots.Add(GameObject.Find("Hud/Shop/ShopSlots").transform.GetChild(i).gameObject);
        }
    }
    void Start()
    {
        BuyButton = GameObject.Find("Hud/Shop/Buy");
        UI = GameObject.Find("Hud").GetComponent<UIManager>();
        Shop = gameObject;
        Shop.SetActive(false);
        purchaseWin.SetActive(false);
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
            for (int i = 0; i < ShopSlots.Count; i++)
            {
                if (CurrentShop.CurrentShop.Count > i && CurrentShop.CurrentShop[i].ItemCount > 0)
                {
                    ShopSlots[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                    ShopSlots[i].transform.GetComponent<Button>().enabled = true;
                    ShopSlots[i].transform.GetChild(2).GetComponent<Text>().enabled = true;
                    ShopSlots[i].transform.GetChild(3).GetComponent<Text>().enabled = true;
                    ShopSlots[i].transform.GetChild(1).GetComponent<Image>().color = defaultColor;
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
        purchaseWin.SetActive(true);
        purchaseWin.transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(ActiveID).name);
        purchaseWin.transform.GetChild(4).GetComponent<Slider>().maxValue = CurrentShop.CurrentShop[SlotNumber].ItemCount;
        purchaseWin.transform.GetChild(4).GetComponent<Slider>().value = CurrentShop.CurrentShop[SlotNumber].ItemCount /2;
        purchaseWin.transform.GetChild(5).GetComponent<Text>().text = purchaseWin.transform.GetChild(4).GetComponent<Slider>().value + "/" + CurrentShop.CurrentShop[SlotNumber].ItemCount.ToString();
    }
    public void Buy()
    {
        UI.WhenBuyText.GetComponent<FadeText>().enabled = true;
        if (player.Coin < ShopItemsPool.ItemByID(ActiveID).price * purchaseWin.transform.GetChild(4).GetComponent<Slider>().value)
        {
            UI.WhenBuyText.GetComponent<Text>().text = "You have not enought money! You need" + (ShopItemsPool.ItemByID(ActiveID).price - player.Coin).ToString() + " coins";
        }
        else
        {
            UI.WhenBuyText.GetComponent<Text>().text = "You succesfully bought an item";
            player.Coin -= ShopItemsPool.ItemByID(ActiveID).price * (int)purchaseWin.transform.GetChild(4).GetComponent<Slider>().value;
            Money.text = "Money: " + player.Coin;
            inv.ItemToInventory(this, (int)purchaseWin.transform.GetChild(4).GetComponent<Slider>().value);
            for (int i = 0; i < ShopSlots.Count; i++)
            {
                if (ShopSlots[i].GetComponent<ShopSlot>().ItemID == ActiveID)
                {
                    CurrentShop.CurrentShop[i].ItemCount -= (int)purchaseWin.transform.GetChild(4).GetComponent<Slider>().value;
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
            purchaseWin.SetActive(false);
        }
        UI.WhenBuyText.GetComponent<FadeText>().Show = true;
    }
    public void ClosePurchaseWin()
    {
        purchaseWin.SetActive(false);
    }
    public void ChangeCount()
    {        
        if (purchaseWin != null)
        {
            purchaseWin.transform.GetChild(5).GetComponent<Text>().text = purchaseWin.transform.GetChild(4).GetComponent<Slider>().value + "/" + CurrentShop.CurrentShop[SlotNumber].ItemCount.ToString();
        }
    }
}
