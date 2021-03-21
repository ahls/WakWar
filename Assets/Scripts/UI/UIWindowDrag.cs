using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIWindowDrag : UIDraggable,   IDragHandler, IBeginDragHandler,IEndDragHandler
{
    private RectTransform _rectTransform;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvasScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetSecondToLast();
        IngameManager.UnitManager.ControlOn = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IngameManager.UnitManager.ControlOn = true;
    }

}
