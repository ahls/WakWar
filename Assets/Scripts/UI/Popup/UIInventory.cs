using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : UIPopup
{
    [SerializeField] private Item_Slot[] _itemSlots;
    public override PopupID GetPopupID() { return PopupID.UIInventory; }
    public int MoneyLocation = -1; //돈의 위치 인덱스. 돈이 없을경우 -1 로 설정
    public override void SetInfo()
    {
    }

    private void Start()
    {
        IngameManager.instance.SetInventory(this);
    }
    public List<Transform> getEmptySlots(int numSlots)
    {
        List<Transform> returningList = new List<Transform>();
        int numSlotEmpty = 0;
        foreach (var itemSlot in _itemSlots)
        {
            if (itemSlot.CurrentNumber == 0)
            {
                numSlotEmpty++;
                returningList.Add(itemSlot.transform);
            }
        }
        return returningList;
    }
    public int getEmptySlot(int numSlots)
    {
        for (int i = 0; i < 16; i++)
        {
            if(_itemSlots[i].CurrentNumber == 0)
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// 돈을 받는경우 +, 뺴는경우 -로 가능.
    /// 돈을 빼는경우, 0보다 적을경우 false 를 리턴,
    /// 혹은 0이 될경우, 돈의 위치를 -1로 설정.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool AddMoney(int amount)
    {
        if(MoneyLocation == -1)
        {//돈의 위치가 없는경우
            getEmptySlot(1);
        }
        if(amount > 0)
        {
            
            return true;
        }
    }
}
