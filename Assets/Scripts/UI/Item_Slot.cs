﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Item_Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private ItemType _slotType;
    public int currentNumber { get; set; } = 0;
    public void OnDrop(PointerEventData eventData)
    {
        Item_Draggable draggedItem = eventData.pointerDrag.GetComponent<Item_Draggable>();
        
        if(draggedItem != null)
        {
            if (currentNumber == 0)
            {
                if (draggedItem.compareType(_slotType))
                {
                    draggedItem.placeItem(transform);
                }
                else
                {
                    Global.UIManager.PushNotiMsg("잘못된 타입 입니다.", 1f);
                }
            }
        }
    }

    public void setup(ItemType newType)
    {
        _slotType = newType;
    }

    
}
