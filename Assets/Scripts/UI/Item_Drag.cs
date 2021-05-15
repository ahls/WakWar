using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Item_Drag : UIDraggable,IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] CanvasGroup _canvasGroup;

    public Transform ParentToReturn;
    public bool Equipped = false;
    public bool SellingItem = false; //상점에서 파는 아이템일경우 true
    private RectTransform _rectTransform;
    private ItemType _itemType;
    

    // Start is called before the first frame update
    void Start()
    {

        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.parent.GetComponent<Item_Slot>().CurrentNumber++;
        ParentToReturn = _rectTransform.parent;
    }

    #region 헬퍼 함수

    public void placeItem(Transform parentToBe)
    {
        if(Equipped)
        {
            ParentToReturn.GetComponent<Item_Slot>().assgiendUnit.UnEquipWeapon();
            Equipped = false;
        }
        ParentToReturn = parentToBe;
        playSound();
    }


    private void playSound()
    {
        switch (_itemType)
        {
            case ItemType.Potion:
                Global.AudioManager.PlayOnce("MovePotion");
                break;
            case ItemType.Weapon:
                Global.AudioManager.PlayOnce("MoveItem");
                break;
            case ItemType.Relic:
                Global.AudioManager.PlayOnce("MoveRelic");
                break;
            case ItemType.Money:
                Global.AudioManager.PlayOnce("MoveCoin");
                break;
            case ItemType.Any:
                break;
            default:
                break;
        }
    }
    public bool compareType(ItemType slottype)
    {
        return (slottype == _itemType || slottype == ItemType.Any);
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
            ParentToReturn.GetComponent<Item_Slot>().CurrentNumber--; //현재 자리를 빈자리로 표시
            _rectTransform.SetParent(_canvas.transform);
            _rectTransform.SetAsLastSibling();
            IngameManager.UnitManager.ControlOn = false;


            //raycast ignore to allow item_slot to be accessible.
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.7f;
            if(!SellingItem && Global.UIPopupManager.FindPopup(PopupID.UIShop)!=null)
            {
                IngameManager.UIShop.ToggleSellingWindow(true);
            }

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
            _rectTransform.SetParent(ParentToReturn);
            _rectTransform.position = ParentToReturn.position;
            ParentToReturn.GetComponent<Item_Slot>().CurrentNumber++;
            IngameManager.UnitManager.ControlOn = true;


            //setting the raycast option
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;

            //상점 판매칸 닫는 스크립트
            if (Global.UIPopupManager.FindPopup(PopupID.UIShop) != null)
            {
                IngameManager.UIShop.ToggleSellingWindow(false);
            }
        }
    }
}
