using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tesUnitSetup : MonoBehaviour
{
    public bool CanControl = false;
    public float  range,  AOE, projSpeed,ms;
    public int hp,dmg,AP;
    public Sprite image;
    public float torque;
   
    private void Awake()
    {
        if(CanControl)
        GetComponent<UnitStats>().PlayerUnitInit("Dango");
        UnitCombat UC = GetComponent<UnitCombat>();
        UC.resultDamage = dmg;
        UC.resultRange = range;
        UC.resultAP = AP;
        UC.resultAOE = AOE;
        UC.projectileSpeed = projSpeed;
        UC.attackSpeed = 3;
        UC.armor = 0;
        UC.attackImage = image;
        UC.healthMax = hp;
        UC.attackTorque = torque;
        GetComponent<UnitStats>().MoveSpeed = ms;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
