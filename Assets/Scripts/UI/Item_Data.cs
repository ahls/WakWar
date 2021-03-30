using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemType {potion, weapon,relic, money,any}
public class Item_Data : MonoBehaviour, IPointerDownHandler
{
    #region 변수
    private int itemID;
    private int enchantID;
    #endregion

    private void Start()
    {
    }
    public void setup(int ID)
    {
        itemID = ID;
        GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(Items.DB[itemID].imgSrc);
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
