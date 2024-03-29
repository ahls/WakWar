﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum RewardType
{
    equipment,
    reinforcement,
    consumable,
    relic
}

public class RewardsWindow : UIPopup
{

    [SerializeField] private GameObject categoryPanel;
    [SerializeField] private GameObject MontyPanel;
    [SerializeField] private GameObject _exitWarning;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private Text[] rewardText;

    private RewardType _lastReward;
    private RewardType[] _rewardTypes = new RewardType[3];
    private int _rewardAmount = 200;
    public int AdditionalReinforce = 0;
    public static int[] ChancesPerTier = { 8, 4, 2, 1 };
    private static int[] _tierChances = { 0, 0, 0, 0 };
    private const int lowerConstant = 10, upperConstant = 100;
    private KeyCode _transferAllKey = KeyCode.Space;
    private List<int> _spawnedRelics = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        SetChances();
    }
    private void OnDisable()
    {
        if(IngameManager.ProgressManager!= null)
        IngameManager.ProgressManager.NextSequence();
    }
    private void Update()
    {
     if(Input.GetKeyDown(_transferAllKey))
        {
            int emptySlotIndex = 0;
            List<Transform> emptySlots = IngameManager.UIInventory.GetEmptySlots();
            List<Transform> itemSlots = new List<Transform>();
            foreach (GameObject slot in slots)
            {
                if (emptySlots.Count > emptySlotIndex)
                {
                    if (slot.GetComponent<Item_Slot>().Occupied)
                    {
                        Item_Drag itemDrag = slot.transform.GetChild(0).GetComponent<Item_Drag>();
                        itemDrag.SetupForItemPlacement(emptySlots[emptySlotIndex]);
                        itemDrag.PlaceItem();
                        emptySlotIndex++;
                    }
                }
            }
        }
    }
    public void SetChances()
    {
        _tierChances[0] = ChancesPerTier[0];
        _tierChances[1] = _tierChances[0] + ChancesPerTier[1];
        _tierChances[2] = _tierChances[1] + ChancesPerTier[2];
        _tierChances[3] = _tierChances[2] + ChancesPerTier[3];
    }

    public void SetRewards()
    {
        categoryPanel.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            int randomPicker = Random.Range(0, 10);
            if (randomPicker < 3)
            {
                _rewardTypes[i] = RewardType.equipment;
                rewardText[i].text = "장비";
            }
            else if (randomPicker < 6)
            {
                _rewardTypes[i] = RewardType.reinforcement;
                rewardText[i].text = "증원";
            }
            else if (randomPicker < 9)
            {
                _rewardTypes[i] = RewardType.consumable;
                rewardText[i].text = "소모품";
            }
            else
            {
                _rewardTypes[i] = RewardType.relic;
                rewardText[i].text = "유물";
            }
        }
    }

    public void OnButtonClicked(int index)
    {
        categoryPanel.SetActive(false);// 선택창 숨김

        ClearRewards();

        _lastReward = _rewardTypes[index];
        switch (_rewardTypes[index])
        {
            case RewardType.consumable:
                MakeMultiples(1, 4, 2);
                if (IngameManager.RelicManager.RerollReward)
                {
                    MontySetup();
                }
                break;
            case RewardType.equipment:
                if(IngameManager.RelicManager.RerollReward)
                {
                    MontySetup();
                }
                MakeMultiples(2, 9, 4);
                break;
            case RewardType.relic:
                int chosenItemID = Items.relicIDs[Random.Range(0, Items.relicIDs.Count - 3)];// 보스 보상 제외

                MakeSingle(chosenItemID);

                break;
            case RewardType.reinforcement:
                Global.UIManager.PushNotiMsg($"팬치 {3}명을 증원 받을 수 있습니다!\n" +
                    $"(남은 자리: {IngameManager.TwitchClient.OpenEnrolling(3 + AdditionalReinforce)})", 3);
                Global.UIPopupManager.Pop(PopupID.UIReward);
                break;
            default:
                break;
        }

    }



    private void ClearRewards()
    {
        for (int i = 0; i < 16; i++)// 전에 올라온 아이템 전부 삭제
        {
            slots[i].GetComponent<Item_Slot>().OccupyingItem = null;
            Transform tempTransform = slots[i].transform;
            if (tempTransform.childCount != 0)
            {
                Global.ObjectManager.ReleaseObject(Items.PREFAB_NAME, slots[i].transform.GetChild(0).gameObject);
            }
        }
    }
    public void TryClose()
    {
        foreach (GameObject slot in slots)
        {
            if(slot.transform.childCount > 0)
            {
                _exitWarning.SetActive(true);
                return;
            }
        }
        gameObject.SetActive(false);
    }
    public void BossRelic(int itemID)
    {
        categoryPanel.SetActive(false);
        ClearRewards();
        MakeSingle(itemID);
    }
    private void MakeSingle(int itemID)
    {
        GameObject newItem = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
        newItem.GetComponent<Item_Data>().Setup(itemID,slots[0].transform);
    }
    private void MakeMultiples(int baseNumber,int typeRange, int tierRange)
    {
        List<int> listOfItems = new List<int>();
        
        int totalValue = 0;
        int randomPool = 0; //티어 뽑을때 최대 랜덤 값 지정해주는 변수
        for (int i = 0; i < tierRange; i++)
        {
            randomPool += ChancesPerTier[i];
        }
        do
        {
            int genItemID = baseNumber * 10000;
            do//아이템 값이 너무 높으면 재생성
            {
                genItemID += Random.Range(0, typeRange) * 100;

                int tierRoller = Random.Range(0, randomPool);
                if (tierRoller >= _tierChances[3]) genItemID += 3;
                else if (tierRoller >= _tierChances[2]) genItemID += 2;
                else if (tierRoller >= _tierChances[1]) genItemID += 1;

            } while (Items.DB[genItemID].value + totalValue > upperConstant + _rewardAmount);
            totalValue += Items.DB[genItemID].value;
            listOfItems.Add(genItemID);

        } while (totalValue < _rewardAmount && listOfItems.Count < 16);
        listOfItems.Sort();
        for (int i = 0; i < listOfItems.Count; i++)
        {
            GameObject newItem = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
            newItem.GetComponent<Item_Data>().Setup(listOfItems[i], slots[i].transform);
        }
        Debug.Log($"총 가격: {_rewardAmount}");
        Debug.Log($"아이템 갯수: {listOfItems.Count}");
    }

    private void MontySetup()
    {
        MontyPanel.SetActive(true);
    }
    public void MontyReroll()
    {
        ClearRewards();
        if(_lastReward == RewardType.equipment)
        {
            MakeMultiples(2, 9, 4);
        }
        else if(_lastReward == RewardType.consumable)
        {
            MakeMultiples(1, 4, 2);
        }
        MontyPanel.SetActive(false);
    }
    public void MontyReveal()
    {
        MontyPanel.SetActive(false);
    }




    public override PopupID GetPopupID()
    {
        return PopupID.UIReward;
    }

    public override void SetInfo()    {    }

    public override void PostInitialize()
    {
        _rewardAmount = (int)Param;
        SetRewards();
    }
}
