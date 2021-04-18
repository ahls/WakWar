﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemType { Potion, Weapon, Relic, Money, Any }
public class Item_Data : MonoBehaviour, IPointerDownHandler
{
    #region 변수
    public int TestID;
    public int ItemID { get; set; }
    private int _enchantID;
    #endregion

    private void Start()
    {
//        setup(testID);//테스팅용 라인
    }

    public void Setup(int ID, Transform parent = null)
    {
        ItemID = ID;
        Debug.Log($"아이템 데이터 스크립트:{Items.DB[ItemID].imgSrc}");
        Debug.Log(Items.DB[ItemID].name);
        string tempsource = "weapons/" + Items.DB[ItemID].imgSrc;
        Debug.Log(tempsource);
//        GetComponent<Image>().sprite = Resources.Load<Sprite>("weapons/" + Items.DB[itemID].imgSrc);
        GetComponent<Image>().sprite = Resources.Load<Sprite>(tempsource);//리소스 매니져로 불러오는 방법 물어보기
        GetComponent<Item_Drag>().setType(Items.DB[ItemID].type);
        if(parent != null)
        {
            transform.SetParent(parent);
            transform.position = parent.position;
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(116, 116);
        transform.localScale = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)  //아이템 정보 디스플레이
        {
            var param = new itemInfoDisplay.Param();
            param.item = Items.DB[ItemID];
            param.position = eventData.position;

            Global.UIPopupManager.Push(PopupID.UIItemToolTip, param);
        }
    }



}
