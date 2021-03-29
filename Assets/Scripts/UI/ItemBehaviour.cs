using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemType {potion, weapon,relic, money,any}
public class ItemBehaviour : UIDraggable, IBeginDragHandler, IEndDragHandler,IDragHandler, IPointerDownHandler
{
    #region 변수
    [SerializeField] CanvasGroup _canvasGroup;
    public Item _item;
    public Transform parentToReturn;
    
    RectTransform _rectTransform;

    #endregion

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.parent.GetComponent<Item_Slot>().currentNumber++;
        parentToReturn = _rectTransform.parent;
    }
    public void setup()
    {
        GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(_item.imgSrc);
    }

    #region 클릭 관련
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            parentToReturn.GetComponent<Item_Slot>().currentNumber--; //현재 자리를 빈자리로 표시
            _rectTransform.SetParent(_canvas.transform);
            SetSecondToLast();
            IngameManager.UnitManager.ControlOn = false;

            //raycast ignore to allow item_slot to be accessible.
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.7f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _rectTransform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //setting parents
            _rectTransform.SetParent(parentToReturn);
            _rectTransform.position = parentToReturn.position;
            parentToReturn.GetComponent<Item_Slot>().currentNumber++;
            IngameManager.UnitManager.ControlOn = true;


            //setting the raycast option
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
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

    #endregion

    #region 헬퍼 함수

    public void placeItem(Transform parentToBe)
    {
        parentToReturn = parentToBe;
    }

    public bool compareType(ItemType slottype)
    {
        return (slottype == _item.type || slottype ==ItemType.any);
    }

    #endregion
}
