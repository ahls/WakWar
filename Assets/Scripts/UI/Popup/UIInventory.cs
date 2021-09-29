using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : UIPopup
{
    [SerializeField] private Item_Slot[] _itemSlots;
    public override PopupID GetPopupID() { return PopupID.UIInventory; }
    private Item_Drag moneyDrag;
    public override void SetInfo()
    {
    }
    public override void PostInitialize()
    {
    }

    private void Start()
    {
        if (IngameManager.UIInventory == null)
        {
            IngameManager.instance.SetInventory(this);
            Pop();
        }
    }
    public List<Transform> getEmptySlots(int numSlots)
    {
        List<Transform> returningList = new List<Transform>();
        int numSlotEmpty = 0;
        foreach (var itemSlot in _itemSlots)
        {
            if (itemSlot.OccupyingItem == null)
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
            if(_itemSlots[i].OccupyingItem == null)
            {
                return i;
            }
        }
        return -1;
    }
    public int GetCurrentMoney()
    {
        if (moneyDrag != null) return moneyDrag.NumberOfItems;
        return 0;
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
        if(moneyDrag == null)
        {//돈이 없는경우
            if(amount <0 )
            {//돈이 아예 없는데 차감하려 할 경우
                return false;
            }

            //돈 추가
            int emptySpot = getEmptySlot(1);
            GameObject newMoney = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
            newMoney.GetComponent<Item_Data>().Setup(1, _itemSlots[emptySpot].transform);
            moneyDrag = newMoney.GetComponent<Item_Drag>();
            _itemSlots[emptySpot].OccupyingItem.NumberOfItems = amount;
            return true;
        }
        else if(amount > 0)
        {//돈의 위치가 있고, 추가
            moneyDrag.NumberOfItems += amount;
            return true;
        }
        else
        {//차감시
            if(moneyDrag.NumberOfItems < -amount)
            {//돈이 모자르면
                return false;
            }
            //차감할 돈이 충분함
            moneyDrag.NumberOfItems += amount;
            if (moneyDrag.NumberOfItems == 0)
            {
                moneyDrag = null; //돈 전부 소진시 아이템 제거
            }
            return true;
        }
    }
}
