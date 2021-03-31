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

public class RewardsWindow : MonoBehaviour
{

    [SerializeField] GameObject categoryPanel;
    [SerializeField] GameObject[] slots;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Text[] rewardText;

    private rewardType[] rewardTypes = new rewardType[3];
    public int testingLevelValue;
    public int additionalReinforce = 0;
    private const int lowerConstant = 10, upperConstant = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRewards()
    {

        categoryPanel.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            int randomPicker = Random.Range(0,10);
            if (randomPicker < 3)
            {
                rewardTypes[i] = rewardType.equipment;
                rewardText[i].text = "장비";
            }
            else if (randomPicker < 6)
            {

                rewardTypes[i] = rewardType.reinforcement;
                rewardText[i].text = "증원";
            }
            else if (randomPicker < 9)
            {

                rewardTypes[i] = rewardType.consumable;
                rewardText[i].text = "소모품";
            }
            else
            {

                rewardTypes[i] = rewardType.relic;
                rewardText[i].text = "유물";
            }
        }
    }

    public void onButtonClicked(int index)
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
        switch (rewardTypes[index])
        {
            case rewardType.consumable:
                chosenItemID = Items.consumableIDs[Random.Range(0, Items.consumableIDs.Count)];
                layItems(chosenItemID);
                break;
            case rewardType.equipment:
                chosenItemID = Items.weaponIDs[Random.Range(0, Items.weaponIDs.Count)];
                layItems(chosenItemID);
                break;
            case rewardType.relic:
                chosenItemID = Items.relicIDs[Random.Range(0, Items.relicIDs.Count)];
                Debug.Log(chosenItemID);
                GameObject newItem = Global.ResourceManager.LoadPrefab(itemPrefab.name);
                newItem.GetComponent<Item_Data>().setup(chosenItemID);
                newItem.transform.SetParent(slots[0].transform);
                newItem.GetComponent<RectTransform>().position = slots[0].transform.position;
                slots[0].GetComponent<Item_Slot>().currentNumber++;

                break;
            case rewardType.reinforcement:
                IngameManager.TwitchClient.openEnrolling(testingLevelValue+additionalReinforce);
                break;
            default:
                break;
        }

    }
    private void layItems(int itemID)
    {
        Debug.Log(itemID);
        int itemValue = Items.DB[itemID].value;
        int numItems = (testingLevelValue * lowerConstant / itemValue)+1; // 지급될 아이템 갯수
        Debug.Log($"{numItems}개의 {itemID}");
        
        for (int i = 0; i < numItems; i++)
        {
            GameObject newItem = Global.ResourceManager.LoadPrefab(itemPrefab.name);
            newItem.GetComponent<Item_Data>().setup(itemID);
            newItem.transform.SetParent(slots[i].transform,false);
            newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(116, 116);
            newItem.transform.localScale = Vector3.one;
            newItem.GetComponent<RectTransform>().position = slots[i].transform.position;
            slots[i].GetComponent<Item_Slot>().currentNumber++;
        }
    }
}
