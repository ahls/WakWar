using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Item_Draggable : UIDraggable,IPointerDownHandler, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    #region 변수
    [SerializeField] CanvasGroup _canvasGroup;
    public Transform parentToReturn;
    
    RectTransform _rectTransform;

    #endregion

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.parent.GetComponent<Item_Slot>().currentNumber++;
        parentToReturn = _rectTransform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturn.GetComponent<Item_Slot>().currentNumber--; //현재 자리를 빈자리로 표시
        _rectTransform.SetParent(_canvas.transform);
        SetSecondToLast();

        //raycast ignore to allow item_slot to be accessible.
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //setting parents
        _rectTransform.SetParent(parentToReturn);
        _rectTransform.position = parentToReturn.position;
        parentToReturn.GetComponent<Item_Slot>().currentNumber++;


        //setting the raycast option
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On pointer down called");
    }

    public void placeItem(Transform parentToBe)
    {
        parentToReturn = parentToBe;
    }
}
