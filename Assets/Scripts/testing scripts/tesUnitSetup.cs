using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tesUnitSetup : MonoBehaviour
{
    public bool CanControl = false;

    private void Awake()
    {
        if(CanControl)
        GetComponent<UnitStats>().playerUnitInit("Dango");
        UnitCombat UC = GetComponent<UnitCombat>();
        UC.resultDamage = 1;
        UC.resultRange = 5;
        UC.resultAP = 0;
        UC.resultAOE = 1;
        UC.projectileSpeed = 1;
        UC.attackSpeed = 1;
        UC.armor = 0;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
