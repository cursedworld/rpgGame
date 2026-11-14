using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropManager : SlotController
{
    public List<GameObject> DropSlots;
    public GameObject DropSlot;
    public int DefWidth = 110;
    public int ScrollWidth = 128;
    public GameObject Scroll;
    public RectTransform Background;
    public GameObject Slots;
    public Transform ScrollSpawn;
    public int SlotX = 0;
    public int SlotY = 3;
    void Awake()
    {
        GameObject.Find("Hud/TrashWindow").GetComponent<ScrollRect>().enabled = false;
        Scroll = GameObject.Find("Hud/TrashWindow/Scrollbar Vertical");
        Scroll.SetActive(false);
        Background = GameObject.Find("Hud/TrashWindow").GetComponent<RectTransform>();
        for (int i = 0; i < GameObject.Find("Hud/TrashWindow").transform.GetChild(3).childCount; i++)
        {
            DropSlots.Add(GameObject.Find("Hud/TrashWindow").transform.GetChild(3).GetChild(i).gameObject);
        }
        Slots = GameObject.Find("Hud/TrashWindow/Slots");
        ScrollSpawn = GameObject.Find("Hud/TrashWindow/Viewport/Content").transform;
    }
    void Update()
    {
        
    }
}
