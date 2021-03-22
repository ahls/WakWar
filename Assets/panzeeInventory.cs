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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateStat(DisplayStat statType, int newValue)
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
                updateDisplay(36, 2, newValue);
                break;
            case DisplayStat.dmg:
                updateDisplay(43, 2, newValue);
                break;

            case DisplayStat.amr:
                updateDisplay(85, 2, newValue);
                break;
            case DisplayStat.ap:
                updateDisplay(100, 2, newValue);
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
                updateDisplay(56, newValue);
                break;

            case DisplayStat.atkSpd:
                updateDisplay(62, newValue);
                break;
            case DisplayStat.mvSpd:
                updateDisplay(77, newValue);
                break;
            default:
                break;
        }
    }

    private void updateDisplay(int index, int length, int value)
    {
        string tempString = value.ToString();
        while (length > tempString.Length)
        {
            tempString.Insert(0, " ");
        }
        char[] newchars = tempString.ToCharArray();
        for (int i = 0; i < length; i++)
        {
            _stats[index + i] = newchars[i];
        }
        StatDisplay.text = _stats.ToString();
    }
    private void updateDisplay(int index, float value)
    {
        char[] newchars = value.ToString("F").ToCharArray();
        for (int i = 0; i < 4; i++)
        {
            _stats[index + i] = newchars[i];
        }
        StatDisplay.text = _stats.ToString();
    }

}
