using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using TMPro;
public class WakWindow : UIPopup
{
    static public WakWindow instance;
    [SerializeField] TextMeshProUGUI StatDisplay;
    char[] _stats;

    public override PopupID GetPopupID() { return PopupID.UIWakWindow; }

    // Start is called before the first frame update
    void Awake()
    {
        _stats = StatDisplay.text.ToCharArray();
        if(instance == null)
        {
            instance = this;
        }
    }

    #region 판도라의 상자 
    public void updateStat(DisplayStat statType, int newValue)
    {


        switch (statType)
        {
            case DisplayStat.CrntHealth:
                updateDisplay(0, 3, newValue);
                break;
            case DisplayStat.Mxhealth:
                updateDisplay(6, 3, newValue);
                break;
            
            case DisplayStat.inte:
                updateDisplay(24, 2, newValue);
                break;
            case DisplayStat.dmg:
                updateDisplay(30, 2, newValue);
                break;

            case DisplayStat.amr:
                updateDisplay(60, 2, newValue);
                break;
            case DisplayStat.ap:
                updateDisplay(70, 2, newValue);
                break;
            case DisplayStat.range:
            case DisplayStat.atkSpd:
            case DisplayStat.mvSpd:
                updateStat(statType, (float)newValue);
                break;
            default:
                break;
        }
    }
    public void updateStat(WeaponType weaponType, int newValue )
    {
        switch (weaponType)
        {
            case WeaponType.Warrior:
                updateDisplay(10, 2, newValue);
                break;
            case WeaponType.Shooter:
                updateDisplay(17, 2, newValue);
                break;
            case WeaponType.Supporter:
                updateDisplay(24, 2, newValue);
                break;
            default:
                break;
        }
    }
    public void updateStat(DisplayStat statType, float newValue)
    {

        switch (statType)
        {
            case DisplayStat.range:
                updateDisplay(38, newValue);
                break;

            case DisplayStat.atkSpd:
                updateDisplay(43, newValue);
                break;
            case DisplayStat.mvSpd:
                updateDisplay(53, newValue);
                break;
            default:
                break;
        }
    }

    private void updateDisplay(int index, int length, int value)
    {
        string tempString = value.ToString();
        char[] newchars = tempString.PadLeft(length).ToCharArray();
        for (int i = 0; i < length; i++)
        {
            _stats[index + i] = newchars[i];
        }
        StatDisplay.SetCharArray(_stats);
    }
    private void updateDisplay(int index, float value)
    {
        char[] newchars = value.ToString("F").ToCharArray();
        for (int i = 0; i < 4; i++)
        {
            _stats[index + i] = newchars[i];
        }

        StatDisplay.SetCharArray(_stats);
    }
    #endregion
}
