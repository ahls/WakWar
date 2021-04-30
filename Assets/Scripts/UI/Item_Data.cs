﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemType { Potion, Weapon, Relic, Money, Any }
public class Item_Data : MonoBehaviour, IPointerDownHandler
{
    #region 변수
    public int ItemID { get; set; }
    private int _enchantID;
    public int Price; //0이면 비매품 
    #endregion

    private void Start()
    {
//        setup(testID);//테스팅용 라인
    }

    public void Setup(int ID, Transform parent = null)
    {
        ItemID = ID;
        string imgSrc = Items.DB[ItemID].imgSrc;
        if (imgSrc != "Null")
        {
            GetComponent<Image>().sprite = Global.ResourceManager.LoadTexture(imgSrc);
        }
        GetComponent<Item_Drag>().setType(Items.DB[ItemID].type);
        if(parent != null)
        {
            transform.SetParent(parent);
            transform.position = parent.position;
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(58, 58);
        transform.localScale = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)  //아이템 정보 디스플레이
        {/*
            var param = new ItemInfoDisplay.Param();
            param.item = Items.DB[ItemID];
            param.position = eventData.position;
            */
            // Global.UIPopupManager.Push(PopupID.UIItemToolTip, param);
            Global.UIPopupManager.Push(PopupID.UIItemToolTip);
        }
    }



}
