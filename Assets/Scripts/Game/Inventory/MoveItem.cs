using UnityEngine;
using UnityEngine.UI;

public class MoveItem : MonoBehaviour
{
    public SlotController Controller;
    public Color UnActiveColor;
    public GameObject ParentSlot;
    public GameObject EquipmentSlot;
    public bool IsEquipment = false;

    private void Start()
    {

    }
    void Update()
    {
        transform.position = Input.mousePosition;
        if (!Input.GetMouseButton(0))
        {
            ParentSlot.transform.GetChild(0).GetComponent<Image>().color = UnActiveColor;
            Controller.ActiveSlot = null;
            Controller.ActiveID = -1;
            Controller.PreviousActiveSlot = null;
            Destroy(gameObject);
        }
    }
}
