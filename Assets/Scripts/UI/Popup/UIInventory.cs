using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : UIPopup
{
    [SerializeField] private Item_Slot[] _itemSlots;
    static public UIInventory instance;
    public override PopupID GetPopupID() { return PopupID.UIInventory; }

    public override void SetInfo()
    {
    }

    private void Start()
    {
        instance = this;
    }
    public Transform[] getEmptySlot(int numSlots)
    {
        List<Transform> returningList = new List<Transform>();
        int numSlotEmpty = 0;
        foreach (var itemSlot in _itemSlots)
        {
            if (itemSlot.currentNumber == 0)
            {
                numSlotEmpty++;
                returningList.Add(itemSlot.transform);   
            }
        }
        return returningList.ToArray();
    }
    
}
