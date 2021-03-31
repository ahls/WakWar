using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Item_Drag : UIDraggable,IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] CanvasGroup _canvasGroup;

    public Transform parentToReturn;
    private ItemType _itemType;

    RectTransform _rectTransform;
    // Start is called before the first frame update
    void Start()
    {

        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.parent.GetComponent<Item_Slot>().currentNumber++;
        parentToReturn = _rectTransform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region 헬퍼 함수

    public void placeItem(Transform parentToBe)
    {
        parentToReturn = parentToBe;
    }

    public bool compareType(ItemType slottype)
    {
        return (slottype == _itemType || slottype == ItemType.any);
    }
    public void setType(ItemType _type)
    {
        _itemType = _type;
    }
    #endregion
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            parentToReturn.GetComponent<Item_Slot>().currentNumber--; //현재 자리를 빈자리로 표시
            _rectTransform.SetParent(_canvas.transform);
            _rectTransform.SetAsLastSibling();
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
}
