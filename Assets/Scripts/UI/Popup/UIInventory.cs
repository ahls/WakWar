using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : UIPopup
{
    [SerializeField] private Item_Slot[] _itemSlots;
    public override PopupID GetPopupID() { return PopupID.UIInventory; }
    private int _moneyLocation = -1; //돈의 위치 인덱스. 돈이 없을경우 -1 로 설정
    public override void SetInfo()
    {
    }
    public override void PostInitialize()
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
        if(_moneyLocation == -1)
        {//돈의 위치가 없는경우
            if(amount <0 )
            {//돈이 아예 없는데 차감하려 할 경우
                return false;
            }

            _moneyLocation = getEmptySlot(1);

            GameObject newMoney = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
            newMoney.GetComponent<Item_Data>().Setup(10000, _itemSlots[_moneyLocation].transform);
            _itemSlots[_moneyLocation].CurrentNumber = amount;
            return true;
        }
        else if(amount > 0)
        {//추가시
            _itemSlots[_moneyLocation].CurrentNumber += amount;
            return true;
        }
        else
        {//차감시
            if(_itemSlots[_moneyLocation].CurrentNumber < -amount)
            {//돈이 모자르면
                return false;
            }

            _itemSlots[_moneyLocation].CurrentNumber += amount;
            return true;
        }
    }
}
