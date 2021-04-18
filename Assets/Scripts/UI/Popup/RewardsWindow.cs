using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RewardType
{
    Equipment,
    Reinforcement,
    Consumable,
    Relic
}

public class RewardsWindow : MonoBehaviour
{

    [SerializeField] private GameObject categoryPanel;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Text[] rewardText;

    private RewardType[] _rewardTypes = new RewardType[3];
    public int TestingLevelValue;
    public int AdditionalReinforce = 0;
    private const int LOWER_ITEM_COUNT_VALUE = 10, UPPER_ITEM_COUNT_VALUE = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
                _rewardTypes[i] = RewardType.Equipment;
                rewardText[i].text = "장비";
            }
            else if (randomPicker < 6)
            {

                _rewardTypes[i] = RewardType.Reinforcement;
                rewardText[i].text = "증원";
            }
            else if (randomPicker < 9)
            {

                _rewardTypes[i] = RewardType.Consumable;
                rewardText[i].text = "소모품";
            }
            else
            {

                _rewardTypes[i] = RewardType.Relic;
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
                Global.ObjectPoolManager.ObjectPooling(itemPrefab.name, slots[i].transform.GetChild(0).gameObject);
            }
        }


        int chosenItemID;
        switch (_rewardTypes[index])
        {
            case RewardType.Consumable:
                chosenItemID = Items.consumableIDs[Random.Range(0, Items.consumableIDs.Count)];
                LayItems(chosenItemID);
                break;
            case RewardType.Equipment:
                chosenItemID = Items.weaponIDs[Random.Range(0, Items.weaponIDs.Count)];
                LayItems(chosenItemID);
                break;
            case RewardType.Relic:
                chosenItemID = Items.relicIDs[Random.Range(0, Items.relicIDs.Count)];
                Debug.Log(chosenItemID);
                GameObject newItem = Global.ResourceManager.LoadPrefab(itemPrefab.name);
                newItem.GetComponent<Item_Data>().Setup(chosenItemID);
                newItem.transform.SetParent(slots[0].transform);
                newItem.GetComponent<RectTransform>().position = slots[0].transform.position;
                slots[0].GetComponent<Item_Slot>().CurrentNumber++;

                break;
            case RewardType.Reinforcement:
                IngameManager.TwitchClient.OpenEnrolling(TestingLevelValue + AdditionalReinforce);
                break;
            default:
                break;
        }

    }

    private void LayItems(int itemID)
    {
        Debug.Log(itemID);
        int itemValue = Items.DB[itemID].value;
        int numItems = (TestingLevelValue * LOWER_ITEM_COUNT_VALUE / itemValue) + 1; // 지급될 아이템 갯수
        Debug.Log($"{numItems}개의 {itemID}");

        for (int i = 0; i < numItems; i++)
        {
            GameObject newItem = Global.ResourceManager.LoadPrefab(itemPrefab.name);
            newItem.GetComponent<Item_Data>().Setup(itemID, slots[i].transform);
            slots[i].GetComponent<Item_Slot>().CurrentNumber++;
        }
    }
}
