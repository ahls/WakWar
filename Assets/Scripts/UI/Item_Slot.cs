﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Item_Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private ItemType _slotType;
    public UnitCombat assgiendUnit { get; set; }
    
    public int CurrentNumber { get; set; } = 0;

    //0: 아무것도 아님
    //1: 상점창
    //2: 유물창

    [SerializeField] private short _spotPurpose = 0;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return; //좌클릭이 아니면 리턴

        Item_Drag draggedItem = eventData.pointerDrag.GetComponent<Item_Drag>();

        if (draggedItem != null)
        {
            if (CurrentNumber == 0)
            {
                //상점 판매 로직
                if(_spotPurpose == 1)
                {
                    if(draggedItem.SellingItem)
                    {//상점에 진열된 아이템이면 리턴
                        return;
                    }
                    //상점에 진열된 아이템이 아닐경우, 돈을 주고 아이템 삭제
                    int itemPrice = Items.DB[draggedItem.GetComponent<Item_Data>().ItemID].value/2;//반값에 팔림
                    IngameManager.UIInventory.AddMoney(itemPrice);
                    Global.ObjectManager.ReleaseObject(draggedItem.gameObject.name, draggedItem.gameObject);
                    Global.AudioManager.PlayOnce("SellItem");
                    //상점 판매칸 닫는 스크립트
                    if (Global.UIPopupManager.FindPopup(PopupID.UIShop) != null)
                    {
                        IngameManager.UIShop.ToggleSellingWindow(false);
                    }
                }
                else if (draggedItem.compareType(_slotType))
                {//아이템창 타입 비교


                    if(_spotPurpose == 2)
                    {//유물 장착했을때
                        IngameManager.RelicManager.EquipRelic(draggedItem.GetComponent<Item_Data>().ItemID);
                    }
                    //상점 구매 로직
                    else if (draggedItem.SellingItem)
                    {
                        int itemPrice = -Items.DB[draggedItem.GetComponent<Item_Data>().ItemID].value;
                        if(!IngameManager.UIInventory.AddMoney(itemPrice))
                        {
                            Global.UIManager.PushNotiMsg("소지금이 부족합니다.", 1f);
                            return;
                        }
                        draggedItem.SellingItem = false;
                    }


                    if (assgiendUnit != null)
                    {//유닛과 연결되어있을경우, 
                        int tempWeaponIndex = Items.DB[draggedItem.GetComponent<Item_Data>().ItemID].weaponID;
                        if (Weapons.DB[tempWeaponIndex].Class == assgiendUnit.UnitClassType || assgiendUnit.UnitClassType == ClassType.Wak)
                        {
                            assgiendUnit.EquipWeapon(tempWeaponIndex);
                            draggedItem.placeItem(transform);
                            draggedItem.Equipped = true;
                            
                        }
                        else
                        {
                            Global.UIManager.PushNotiMsg("다른 직업의 장비입니다.", 1f);
                        }
                    }
                    else
                    {
                        draggedItem.placeItem(transform);
                    }
                }
                else
                {

                    Global.UIManager.PushNotiMsg("그곳에 둘 수 없는 아이템 입니다.", 1f);
                }

            }
        }
    }

    public void Setup(ItemType newType)
    {
        _slotType = newType;
    }


}
