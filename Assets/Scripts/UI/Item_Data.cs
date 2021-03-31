using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemType {potion, weapon,relic, money,any}
public class Item_Data : MonoBehaviour, IPointerDownHandler
{
    #region 변수
    public int testID;
    private int itemID;
    private int enchantID;
    #endregion

    private void Start()
    {
//        setup(testID);//테스팅용 라인
    }
    public void setup(int ID)
    {
        itemID = ID;
        Debug.Log($"아이템 데이터 스크립트:{Items.DB[itemID].imgSrc}");
        Debug.Log(Items.DB[itemID].name);
        string tempsource = "weapons/" + Items.DB[itemID].imgSrc;
//        GetComponent<Image>().sprite = Resources.Load<Sprite>("weapons/" + Items.DB[itemID].imgSrc);
        GetComponent<Image>().sprite = Resources.Load<Sprite>(tempsource);//리소스 매니져로 불러오는 방법 물어보기
        GetComponent<Item_Drag>().setType(Items.DB[itemID].type);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)  //아이템 정보 디스플레이
        {
            itemInfoDisplay.instance.loadInfo(Items.DB[itemID]);
            itemInfoDisplay.instance.setLocation(eventData.position);
        }
    }



}
