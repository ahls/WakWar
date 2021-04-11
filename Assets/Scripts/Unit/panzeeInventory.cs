using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum DisplayStat {level,Mxhealth,CrntHealth,str,agi,inte,dmg,range,atkSpd,mvSpd,amr,ap }
public class panzeeInventory : MonoBehaviour
{
    #region 변수
    [SerializeField] private Text _name;
    [SerializeField] private GameObject _itemSlot;
    //스탯관련
    [SerializeField] private TextMeshProUGUI _statDisplay;
    private char[] _stats;
    private GameObject _unit;
    
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        _stats = _statDisplay.text.ToCharArray();
    }

    public void setup(string name, GameObject gameObject, Transform parent)
    {
        _name.text = name;
        _unit = gameObject;
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
        _itemSlot.GetComponent<Item_Slot>().assgiendUnit = _unit.GetComponent<UnitCombat>();
    }
    /// <param name="one_or_zero"></param>
    public void collapse(bool collapsing)
    {
        _itemSlot.SetActive(!collapsing);
        Vector2 sizeDelta = new Vector2(300, collapsing ? 50 : 108);
        GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }


    #region 판도라의 상자
    public void updateStat(DisplayStat statType, int newValue)
    {
        try
        {


            switch (statType)
            {
                case DisplayStat.level:
                    updateDisplay(21, 2, newValue);
                    break;
                case DisplayStat.Mxhealth:
                    updateDisplay(36, 3, newValue);
                    break;
                case DisplayStat.CrntHealth:
                    updateDisplay(32, 3, newValue);
                    break;
                case DisplayStat.str:
                    updateDisplay(0, 2, newValue);
                    break;
                case DisplayStat.agi:
                    updateDisplay(8, 2, newValue);
                    break;
                case DisplayStat.inte:
                    updateDisplay(17, 2, newValue);
                    break;
                case DisplayStat.dmg:
                    updateDisplay(43, 2, newValue);
                    break;

                case DisplayStat.amr:
                    updateDisplay(52, 2, newValue);
                    break;
                case DisplayStat.ap:
                    updateDisplay(61, 2, newValue);
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
        catch (Exception)
        {

            throw;
        }
    }
    public void updateStat(DisplayStat statType, float newValue)
    {

        switch (statType)
        {
            case DisplayStat.range:
                updateDisplay(64, newValue);
                break;

            case DisplayStat.atkSpd:
                updateDisplay(73, newValue);
                break;
            case DisplayStat.mvSpd:
                updateDisplay(82, newValue);
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
            _stats[index + i] =  newchars[i];
        }
        _statDisplay.SetCharArray(_stats);
    }
    private void updateDisplay(int index, float value)
    {
        char[] newchars = value.ToString("F").ToCharArray();
        for (int i = 0; i < 4; i++)
        {
            _stats[index + i] = newchars[i];
        }

        _statDisplay.SetCharArray(_stats);
    }
    #endregion
}
