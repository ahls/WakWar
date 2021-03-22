using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statdisplay_test : MonoBehaviour
{
    public panzeeInventory PI;
    // Start is called before the first frame update
    void Start()
    {
        PI.updateStat(DisplayStat.level, 2);
        PI.updateStat(DisplayStat.Mxhealth, 2);
        PI.updateStat(DisplayStat.CrntHealth, 2);
        PI.updateStat(DisplayStat.dmg, 2);
        PI.updateStat(DisplayStat.range, 2);
        PI.updateStat(DisplayStat.agi, 2);
        PI.updateStat(DisplayStat.str, 2);
        PI.updateStat(DisplayStat.inte, 2);
        PI.updateStat(DisplayStat.amr, 2);
        PI.updateStat(DisplayStat.ap, 2);
        PI.updateStat(DisplayStat.atkSpd, 2.12f);
        PI.updateStat(DisplayStat.mvSpd, 2.00f);
        PI.updateStat(DisplayStat.range, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
