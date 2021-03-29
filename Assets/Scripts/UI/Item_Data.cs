using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemType {potion, weapon,relic, money,any}
public class Item_Data : MonoBehaviour, IPointerDownHandler
{
    #region 변수
    public Item _item;
    public int itemID; //테스트로 넣어둔 값입니다.
    private bool loadTesting = false;
    #endregion

    private void Start()
    {
    }
    public void setup()
    {
        GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(_item.imgSrc);
        GetComponent<Item_Drag>().setType(_item.type);
    }


    private void Update()
    {
        if (loadTesting) return;
        if(Time.time > 2)
        {
            loadTesting = true;
            Debug.Log("items are setup");

            _item = Items.DB[itemID];//테스트용 코드

            GetComponent<Item_Drag>().setType(_item.type);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)  //아이템 정보 디스플레이
        {
            itemInfoDisplay.instance.loadInfo(_item);
            itemInfoDisplay.instance.setLocation(eventData.position);
        }
    }



}
