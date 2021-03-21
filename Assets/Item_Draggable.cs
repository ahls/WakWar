using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Item_Draggable : MonoBehaviour,IPointerDownHandler, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    #region 변수
    [SerializeField] Canvas _canvas;
    [SerializeField] CanvasGroup _canvasGroup;
    public Transform parentToReturn;
    RectTransform _rectTransform;
    int _childIndex = 0;

    #endregion

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturn = _rectTransform.parent;
        _childIndex = transform.GetSiblingIndex();
        _rectTransform.SetParent(parentToReturn.parent.parent);
        _rectTransform.SetSiblingIndex(_rectTransform.parent.childCount-1);
        parentToReturn.GetChild(_childIndex).gameObject.SetActive(true); 

        //raycast ignore to allow item_slot to be accessible.
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //setting parents
        parentToReturn.GetChild(_childIndex).gameObject.SetActive(false);
        _rectTransform.SetParent(parentToReturn);
        _rectTransform.SetSiblingIndex(_childIndex); 


        //setting the raycast option
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On pointer down called");
    }

    public void dropItem(Transform parentToBe, int childIndex)
    {
        parentToReturn = parentToBe;
        _childIndex = childIndex;
    }
}
