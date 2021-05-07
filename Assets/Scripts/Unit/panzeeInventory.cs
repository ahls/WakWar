using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum DisplayStat { Level, Mxhealth, CrntHealth, Str, Agi, Inte, Dmg, CD, AtkSpd, Regen, Amr, Ap }

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

    public void Setup(string name, GameObject gameObject, Transform parent)
    {
        _name.text = name;
        _unit = gameObject;
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
        _itemSlot.GetComponent<Item_Slot>().assgiendUnit = _unit.GetComponent<UnitCombat>();
    }
    /// <param name="one_or_zero"></param>
    public void Collapse(bool collapsing)
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
                case DisplayStat.Level:
                    updateDisplay(21, 2, newValue);
                    break;
                case DisplayStat.Mxhealth:
                    updateDisplay(36, 3, newValue);
                    break;
                case DisplayStat.CrntHealth:
                    updateDisplay(32, 3, newValue);
                    break;
                case DisplayStat.Str:
                    updateDisplay(0, 2, newValue);
                    break;
                case DisplayStat.Agi:
                    updateDisplay(8, 2, newValue);
                    break;
                case DisplayStat.Inte:
                    updateDisplay(17, 2, newValue);
                    break;
                case DisplayStat.Dmg:
                    updateDisplay(43, 2, newValue);
                    break;

                case DisplayStat.Amr:
                    updateDisplay(52, 2, newValue);
                    break;
                case DisplayStat.Ap:
                    updateDisplay(61, 2, newValue);
                    break;
                case DisplayStat.CD:
                case DisplayStat.AtkSpd:
                case DisplayStat.Regen:
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
            case DisplayStat.AtkSpd:
                updateDisplay(64, newValue);
                break;

            case DisplayStat.CD:
                updateDisplay(73, newValue);
                break;
            case DisplayStat.Regen:
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
