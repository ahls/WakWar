using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Item_Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private ItemType _slotType;
    public UnitCombat assgiendUnit { get; set; }

    public int CurrentNumber { get; set; } = 0;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        Item_Drag draggedItem = eventData.pointerDrag.GetComponent<Item_Drag>();

        if (draggedItem != null)
        {
            if (CurrentNumber == 0)
            {
                if (draggedItem.compareType(_slotType))
                {
                    if (assgiendUnit != null)
                    {//유닛과 연결되어있을경우, 이미 무기로 설정되어있음.
                        int tempWeaponIndex = Items.DB[draggedItem.GetComponent<Item_Data>().ItemID].weaponID;
                        if (Weapons.DB[tempWeaponIndex].weaponType == assgiendUnit.weaponType || assgiendUnit.weaponType == WeaponType.Wak)
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

                    Global.UIManager.PushNotiMsg("잘못된 타입 입니다.", 1f);
                }

            }
        }
    }

    public void Setup(ItemType newType)
    {
        _slotType = newType;
    }


}
