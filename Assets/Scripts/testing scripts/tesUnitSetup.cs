using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tesUnitSetup : MonoBehaviour
{
    public bool CanControl = false;
    public Sprite image;
    private void Awake()
    {
        if(CanControl)
        GetComponent<UnitStats>().playerUnitInit("Dango");
        UnitCombat UC = GetComponent<UnitCombat>();
        UC.resultDamage = 1;
        UC.resultRange = 2;
        UC.resultAP = 0;
        UC.resultAOE = 0.1f;
        UC.projectileSpeed = 3;
        UC.attackSpeed = 3;
        UC.armor = 0;
        UC.attackImage = image;
        UC.healthMax = 10;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
