using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum rewardType
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

    private rewardType[] _rewardTypes = new rewardType[3];
    private int _rewardAmount = 0;
    public int AdditionalReinforce = 0;
    private const int lowerConstant = 10, upperConstant = 100;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void SetRewards()
    {
        categoryPanel.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            int randomPicker = Random.Range(0, 10);
            if (randomPicker < 3)
            {
                _rewardTypes[i] = rewardType.equipment;
                rewardText[i].text = "장비";
            }
            else if (randomPicker < 6)
            {
                _rewardTypes[i] = rewardType.reinforcement;
                rewardText[i].text = "증원";
            }
            else if (randomPicker < 9)
            {
                _rewardTypes[i] = rewardType.consumable;
                rewardText[i].text = "소모품";
            }
            else
            {
                _rewardTypes[i] = rewardType.relic;
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
            case rewardType.consumable:
                chosenItemID = Items.consumableIDs[Random.Range(0, Items.consumableIDs.Count)];
                LayItems(chosenItemID);
                break;
            case rewardType.equipment:
                chosenItemID = Items.weaponIDs[Random.Range(0, Items.weaponIDs.Count)];
                LayItems(chosenItemID);
                break;
            case rewardType.relic:
                chosenItemID = Items.relicIDs[Random.Range(0, Items.relicIDs.Count)];
                Debug.Log(chosenItemID);
                GameObject newItem = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
                newItem.GetComponent<Item_Data>().Setup(chosenItemID);
                newItem.transform.SetParent(slots[0].transform);
                newItem.GetComponent<RectTransform>().position = slots[0].transform.position;
                slots[0].GetComponent<Item_Slot>().CurrentNumber++;

                break;
            case rewardType.reinforcement:
                IngameManager.TwitchClient.OpenEnrolling(_rewardAmount + AdditionalReinforce);
                break;
            default:
                break;
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
