using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    public Dictionary<int, int> CurrentItems = new();
    public Inventory I;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void OpenBox()
    {
        // убрать все картинки со слотов
        GameObject.Find("Hud").GetComponent<UIManager>().TrashWindow.SetActive(true);
        DropManager DropM = GameObject.Find("Hud/TrashWindow").GetComponent<DropManager>();
        for (int i = 0; i < CurrentItems.Count; i++)
        {
            InventorySlot S = DropM.DropSlots[i].GetComponent<InventorySlot>();
            S.ItemID = CurrentItems.ElementAt(i).Key;
            S.ItemCount = CurrentItems.ElementAt(i).Value;
            DropM.DropSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsPool.LoadImage(ShopItemsPool.ItemByID(S.ItemID).name);
            DropM.DropSlots[i].transform.GetChild(1).gameObject.SetActive(true); 
        }
    }
}
