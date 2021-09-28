using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class Item_Drag : UIDraggable,IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _numberDisplay;

    public Transform ParentToReturn;
    public bool Equipped = false;
    private bool _canDrag = true;
    public bool SetToPool { get; set; }
    public bool SellingItem = false; //상점에서 파는 아이템일경우 true
    private RectTransform _rectTransform;
    private ItemType _itemType;
    private int _numItems;

    public int NumberOfItems 
    {
        get => _numItems;
        set
        {
            _numItems = value;
            if(_numberDisplay != null)
            {
                if(value > 1)
                {
                    _numberDisplay.text = value.ToString();
                }
                else
                {
                    _numberDisplay.text = "";
                }
            }
        }
    }
    
    

    // Start is called before the first frame update
    void Start()
    {

        _rectTransform = GetComponent<RectTransform>();

    }
    public void Init()
    {
        _rectTransform.parent.GetComponent<Item_Slot>().OccupyingItem = this;
        ParentToReturn = _rectTransform.parent;
    }
    #region 헬퍼 함수

    public void placeItem(Transform parentToBe)
    {
        Item_Slot lastSlot = ParentToReturn.GetComponent<Item_Slot>();
        if(Equipped)
        {
            lastSlot.assgiendUnit.UnEquipWeapon();
            Equipped = false;
        }
        lastSlot.OccupyingItem = null;
        if(lastSlot.SpotPurpose == 4)
        {//마법부여 칸이면 마법부여 버튼 해제
            IngameManager.UIShop.DisableEnchantButton();
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
        if (_canDrag &&eventData.button == PointerEventData.InputButton.Left)
        {
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
            ParentToReturn.GetComponent<Item_Slot>().OccupyingItem = this;
            IngameManager.UnitManager.ControlOn = true;


            //setting the raycast option
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
            transform.SetAsFirstSibling();
            //상점 판매칸 닫는 스크립트
            if (Global.UIPopupManager.FindPopup(PopupID.UIShop) != null)
            {
                IngameManager.UIShop.ToggleSellingWindow(false);
            }
            if(SetToPool)
            {
                Global.ObjectManager.ReleaseObject(Items.PREFAB_NAME, gameObject);
            }
        }
    }
    public void SetDraggable(bool state)
    {
        _canDrag = state;
    }
    public void Enchanted()
    {
        _numberDisplay.text = "+";
    }
}
