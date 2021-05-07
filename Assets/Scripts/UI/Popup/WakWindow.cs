using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using TMPro;
public class WakWindow : UIPopup
{
    [SerializeField] private TextMeshProUGUI _currentHealth;
    [SerializeField] private TextMeshProUGUI _maxHealth;
    [SerializeField] private TextMeshProUGUI _warrierCount;
    [SerializeField] private TextMeshProUGUI _shooterCount;
    [SerializeField] private TextMeshProUGUI _supporterCount;
    [SerializeField] private TextMeshProUGUI _damageValue;
    [SerializeField] private TextMeshProUGUI _rangeValue;
    [SerializeField] private TextMeshProUGUI _attackSpeedValue;
    [SerializeField] private TextMeshProUGUI _moveSpeedValue;
    [SerializeField] private TextMeshProUGUI _armorValue;
    [SerializeField] private TextMeshProUGUI _penetrateValue;
    [SerializeField] private Item_Slot _itemSlot;

    public override PopupID GetPopupID() { return PopupID.UIWakWindow; }

    void Awake()
    {
    }

    public override void SetInfo()
    {
        _itemSlot.assgiendUnit = IngameManager.WakgoodBehaviour.GetComponent<UnitCombat>();

        UpdateStat(ClassType.Warrior, IngameManager.WakgoodBehaviour.WakStats[0]);
        UpdateStat(ClassType.Shooter, IngameManager.WakgoodBehaviour.WakStats[1]);
        UpdateStat(ClassType.Supporter, IngameManager.WakgoodBehaviour.WakStats[2]);
        IngameManager.WakgoodBehaviour.WakStats[0] = 0;
        IngameManager.WakgoodBehaviour.WakStats[1] = 0;
        IngameManager.WakgoodBehaviour.WakStats[2] = 0;
    }
    public override void PostInitialize()
    {
    }

    public void UpdateStat(DisplayStat statType, int newValue)
    {
        switch (statType)
        {
            case DisplayStat.CrntHealth:
                {
                    _currentHealth.text = newValue.ToString();
                    break;
                }
            case DisplayStat.Mxhealth:
                {
                    _maxHealth.text = newValue.ToString();
                    break;
                }
            case DisplayStat.Dmg:
                {
                    _damageValue.text = newValue.ToString();
                    break;
                }
            case DisplayStat.Amr:
                {
                    _armorValue.text = newValue.ToString();
                    break;
                }
            case DisplayStat.CD:
            case DisplayStat.AtkSpd:
            case DisplayStat.Regen:
                {
                    break;
                }
            default:
                break;
        }
    }

    public void UpdateStat(ClassType weaponType, int newValue )
    {
        switch (weaponType)
        {
            case ClassType.Warrior:
                {
                    _warrierCount.text = newValue.ToString();
                    break;
                }
            case ClassType.Shooter:
                {
                    _shooterCount.text = newValue.ToString();
                    break;
                }
            case ClassType.Supporter:
                {
                    _supporterCount.text = newValue.ToString();
                    break;
                }
            default:
                break;
        }
    }

    public void UpdateStat(DisplayStat statType, float newValue)
    {
        switch (statType)
        {
            case DisplayStat.AtkSpd:
                {
                    _rangeValue.text = newValue.ToString();
                    break;
                }
            case DisplayStat.CD:
                {
                    _attackSpeedValue.text = newValue.ToString();
                    break;
                }
            case DisplayStat.Regen:
                {
                    _moveSpeedValue.text = newValue.ToString();
                    break;
                }
            default:
                break;
        }
    }
}
