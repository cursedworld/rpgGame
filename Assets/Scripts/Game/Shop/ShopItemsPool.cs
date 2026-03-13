using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class Item 
{
    public string name;
    public string type;
    public int id;
    public int price;
    public Item(string name, string type, int id, int price)
    {
        this.name = name;
        this.type = type;
        this.id = id;
        this.price = price;
    }
}
public class Armor: Item
{
    public int armor;
    public string Enchant;
    public float Weight;
    public Armor(int armor, string Enchant, string name, string type, int id, int price, float weight) : base(name, type, id, price)
    {
        this.armor = armor;
        this.Enchant = Enchant;
        Weight = weight;
    }
}
public class Potion : Item
{
    public int duration;
    public int Amount;
    public Potion(int duration, int Amount, string name, string type, int id, int price) : base(name, type, id, price)
    {
        this.duration = duration;
        this.Amount = Amount;
    }
}
public class ShopItemsPool
{
    private static List<string> ItemTypes = new()
    {
        "Belt"
    };
    private static List<string> PotionTypes = new()
    {
        "Heal"
    };
    public static List<Item> items = new()
    {
        new Armor(10, "", "Belt_1", "Belt", 0, 12, 2),
        new Armor(11, "", "Belt_2", "Belt", 1, 18, 2),
        new Armor(12, "", "Belt_3", "Belt", 2, 11, 1.5f),
        new Armor(13, "", "Belt_4", "Belt", 3, 16, 2),
        new Armor(14, "", "Belt_5", "Belt", 4, 19, 2),
        new Armor(15, "", "Belt_6", "Belt", 5, 10, 1.3f),
        new Armor(16, "", "Belt_7", "Belt", 6, 13, 2),
        new Armor(17, "", "Belt_8", "Belt", 7, 14, 2),
        new Armor(18, "", "Belt_9", "Belt", 8, 15, 1.8f),
        new Armor(19, "", "Belt_10", "Belt",9, 17, 2),
        new Potion(0, 5, "Potion_1", "Heal", 10, 12),
        new Potion(0, 6, "Potion_2", "Heal", 11, 18),
    };
    public static List<Item> CreatePool(string PoolType)
    {
        switch(PoolType)
        {
            case "Items":
                {
                    return SelectPool(ItemTypes);
                }
            case "Potions":
                {
                    return SelectPool(PotionTypes);
                }
            default:
                {
                    return null;
                }
        }
    }
    public static Sprite LoadImage(string name)
    {
        return Resources.Load<Sprite>($"ShopSprites/Icons/{name.Substring(0, name.IndexOf('_'))}/{name}");
    }
    public static List<Item> SelectPool(List<string> Pool)
    {
        List<Item> CurrentItems = new();
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = 0; j < Pool.Count; j++)
            {
                if (items[i].type == Pool[j])
                {
                    CurrentItems.Add(items[i]);
                }
            }
        }
        return CurrentItems;
    }
    public static Item ItemByID(int ID)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].id == ID)
            {
                return items[i];
            }
        }
        return null;
    }
    public static bool IsEquipment(int id)
    {
        Item i = ItemByID(id);
        return ItemTypes.Any(x => x == i.type);
    }
}
