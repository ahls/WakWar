using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInfoDisplay : UIPopup, IPointerExitHandler
{
    public override PopupID GetPopupID() { return PopupID.UIItemToolTip; }

    [SerializeField] private Text _name, _type, _value, _desc,_enchant;
    RectTransform rectTransform;

    public class Param
    {
        public EnchantBase enchant;
        public Item item;
        public Vector2 position;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void SetInfo()
    {
    }

    public void SetLocation(Vector2 mouseLocation)
    {
        rectTransform.position = mouseLocation + new Vector2(0,-10);
        rectTransform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Pop();
    }

    public override void PostInitialize()
    {
        Param param = GetParam() as Param;
        if (param.enchant == null)
        {
            _name.text = param.item.name;
            _enchant.text = string.Empty;
        }
        else
        {
            _name.text = param.enchant.Name + param.item.name;
            _enchant.text = param.enchant.Desc;
        }
        switch (param.item.type)
        {
            case ItemType.Potion:
                _type.text = "물약";
                break;
            case ItemType.Weapon:
                _type.text = "장비";
                break;
            case ItemType.Relic:
                _type.text = "유물";
                break;
            case ItemType.Money:
                _type.text = "화폐";
                break;
            case ItemType.Any:
                _type.text = "잡동사니";
                break;
            default:
                break;
        }
        _value.text = param.item.value.ToString();
        if(param.item.type == ItemType.Weapon)
        {//무기일경우 아이템 타입, 공격력, 공속, 방어, 사정거리 표시 후 플레이버 텍스트 추가
            string descriptionText = "";
            Weapon weapon= Weapons.DB[param.item.weaponID];
            switch (weapon.weaponType)
            {
                case WeaponType.Sword:
                    descriptionText += "<b>검</b> (전사)\n";
                    break;
                case WeaponType.Axe:
                    descriptionText += "<b>도끼</b> (전사)\n";
                    break;
                case WeaponType.Shield:
                    descriptionText += "<b>거대 방패</b> (전사)\n";
                    break;
                case WeaponType.Gun:
                    descriptionText += "<b>총</b> (사수)\n";
                    break;
                case WeaponType.Bow:
                    descriptionText += "<b>활</b> (사수)\n";
                    break;
                case WeaponType.Throw:
                    descriptionText += "<b>투척무기</b> (사수)\n";
                    break;
                case WeaponType.Blunt:
                    descriptionText += "<b>둔기</b> (지원가)\n";
                    break;
                case WeaponType.Wand:
                    descriptionText += "<b>지팡이</b> (지원가)\n";
                    break;
                case WeaponType.Instrument:
                    descriptionText += "<b>악기</b> (지원가)\n";
                    break;
                default:
                    descriptionText += "line71 확인:" + ((param.item.weaponID / 10000) + (param.item.weaponID % 1000) / 100).ToString() + "\n";
                    break;
            }
            descriptionText += "공격력 :  " + weapon.damage.ToString();
            descriptionText += "       공격속도 :  " + weapon.AttackSpeed.ToString()+'\n';
            descriptionText += "방어력 :  " + weapon.Armor.ToString();
            descriptionText += "       사정거리 :  " + weapon.AttackRange.ToString() + "\n\n<i>" + param.item.desc + "</i>";
            _desc.text = descriptionText;

        }
        else
        {
            _desc.text = param.item.desc;
        }
        gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
        rectTransform.sizeDelta = new Vector2(400, 93 + _desc.cachedTextGenerator.lineCount * 22.5f);

        SetLocation(param.position);
    }
}
