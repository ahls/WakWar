using System.Collections;
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
    [SerializeField] private GameObject[] slots;
    [SerializeField] private Text[] rewardText;

    private RewardType[] _rewardTypes = new RewardType[3];
    private int _rewardAmount = 0;
    public int AdditionalReinforce = 0;
    private const int lowerConstant = 10, upperConstant = 100;
    public static int[] ChancesPerTier = { 8, 4, 2, 1 };
    private static int[] _tierChances = { 0, 0, 0, 0 };
    // Start is called before the first frame update
    void Start()
    {
        SetChances();
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

        for (int i = 0; i < 16; i++)// 전에 올라온 아이템 전부 삭제
        {
            Transform tempTransform = slots[i].transform;
            if (tempTransform.childCount != 0)
            {
                Global.ObjectManager.ReleaseObject(Items.PREFAB_NAME, slots[i].transform.GetChild(0).gameObject);
            }
        }

        int chosenItemID;

        switch (_rewardTypes[index])
        {
            case RewardType.consumable:
                chosenItemID = Items.consumableIDs[Random.Range(0, Items.consumableIDs.Count)];
                LayItems(chosenItemID);
                break;
            case RewardType.equipment:
                MakeMultiples(2, 9, 4);
                break;
            case RewardType.relic:
                chosenItemID = Items.relicIDs[Random.Range(0, Items.relicIDs.Count)];
                GameObject newItem = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
                newItem.GetComponent<Item_Data>().Setup(chosenItemID);
                newItem.transform.SetParent(slots[0].transform);
                newItem.GetComponent<RectTransform>().position = slots[0].transform.position;
                slots[0].GetComponent<Item_Slot>().CurrentNumber++;

                break;
            case RewardType.reinforcement:
                IngameManager.TwitchClient.OpenEnrolling(_rewardAmount + AdditionalReinforce);
                break;
            default:
                break;
        }

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
            int genItemID = baseNumber * 100000;
            do//아이템 값이 너무 높으면 재생성
            {
                genItemID += Random.Range(0, typeRange) * 100;

                int tierRoller = Random.Range(0, randomPool);
                if (tierRoller >= _tierChances[3]) genItemID += 3;
                else if (tierRoller >= _tierChances[2]) genItemID += 2;
                else if (tierRoller >= _tierChances[1]) genItemID += 1;

            } while (Items.DB[genItemID].value + totalValue > upperConstant + _rewardAmount);
            totalValue += Items.DB[genItemID].value;
        } while (totalValue >= _rewardAmount && listOfItems.Count == 16);
        for (int i = 0; i < listOfItems.Count; i++)
        {
            GameObject newItem = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
            newItem.GetComponent<Item_Data>().Setup(listOfItems[i], slots[i].transform);
            slots[i].GetComponent<Item_Slot>().CurrentNumber++;
        }
    }
    private void LayItems(int itemID)
    {

        int itemValue = Items.DB[itemID].value;
        int numItems = (_rewardAmount * lowerConstant / itemValue) + 1; // 지급될 아이템 갯수
        
        for (int i = 0; i < numItems; i++)
        {
            GameObject newItem = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
            newItem.GetComponent<Item_Data>().Setup(itemID, slots[i].transform);
            slots[i].GetComponent<Item_Slot>().CurrentNumber++;
        }
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
