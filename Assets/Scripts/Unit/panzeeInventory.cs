using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DisplayStat {level,Mxhealth,CrntHealth,str,agi,inte,dmg,range,atkSpd,mvSpd,amr,ap }
public class panzeeInventory : MonoBehaviour
{
    [SerializeField] Text StatDisplay;
    char[] _stats;
    // Start is called before the first frame update
    void Awake()
    {
        _stats = StatDisplay.text.ToCharArray();
    }


    public void updateStat(DisplayStat statType, int newValue)
    {
        try
        {


            switch (statType)
            {
                case DisplayStat.level:
                    updateDisplay(1, 2, newValue);
                    break;
                case DisplayStat.Mxhealth:
                    updateDisplay(16, 3, newValue);
                    break;
                case DisplayStat.CrntHealth:
                    updateDisplay(12, 3, newValue);
                    break;
                case DisplayStat.str:
                    updateDisplay(20, 2, newValue);
                    break;
                case DisplayStat.agi:
                    updateDisplay(28, 2, newValue);
                    break;
                case DisplayStat.inte:
                    updateDisplay(37, 2, newValue);
                    break;
                case DisplayStat.dmg:
                    updateDisplay(43, 2, newValue);
                    break;

                case DisplayStat.amr:
                    updateDisplay(86, 2, newValue);
                    break;
                case DisplayStat.ap:
                    updateDisplay(101, 2, newValue);
                    break;
                default:
                    updateStat(statType, (float)newValue);
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
                updateDisplay(57, newValue);
                break;

            case DisplayStat.atkSpd:
                updateDisplay(63, newValue);
                break;
            case DisplayStat.mvSpd:
                updateDisplay(78, newValue);
                break;
            default:
                break;
        }
    }

    private void updateDisplay(int index, int length, int value)
    {   
       string tempString = value.ToString();
        char[] newchars = tempString.PadLeft(length).ToCharArray();
        Debug.Log(new string(newchars));
        Debug.Log($"lenght {length}, length of value {newchars.Length}");
        for (int i = 0; i < length; i++)
        {
            _stats[index + i] =  newchars[i];
        }
        StatDisplay.text = new String(_stats);
    }
    private void updateDisplay(int index, float value)
    {
        char[] newchars = value.ToString("F").ToCharArray();
        for (int i = 0; i < 4; i++)
        {
            _stats[index + i] = newchars[i];
        }

        StatDisplay.text = new String(_stats);
    }

}
