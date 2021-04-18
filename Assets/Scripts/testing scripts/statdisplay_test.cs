using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statdisplay_test : MonoBehaviour
{
    public panzeeInventory PI;
    // Start is called before the first frame update
    void Start()
    {
        PI.updateStat(DisplayStat.Level, 2);
        PI.updateStat(DisplayStat.Mxhealth, 2);
        PI.updateStat(DisplayStat.CrntHealth, 2);
        PI.updateStat(DisplayStat.Dmg, 2);
        PI.updateStat(DisplayStat.Range, 2);
        PI.updateStat(DisplayStat.Agi, 2);
        PI.updateStat(DisplayStat.Str, 2);
        PI.updateStat(DisplayStat.Inte, 2);
        PI.updateStat(DisplayStat.Amr, 2);
        PI.updateStat(DisplayStat.Ap, 2);
        PI.updateStat(DisplayStat.AtkSpd, 2.12f);
        PI.updateStat(DisplayStat.MvSpd, 2.00f);
        PI.updateStat(DisplayStat.Range, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
