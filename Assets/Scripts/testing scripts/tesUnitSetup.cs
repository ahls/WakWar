using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tesUnitSetup : MonoBehaviour
{
    public bool CanControl = false;
    public float  range,  AOE, projSpeed,ms;
    public int hp,dmg,AP;
    public int weaponID;
    public float torque;
    private bool used = false;
    UnitCombat UC;
    private void Start()
    {
        if(CanControl)
        GetComponent<UnitStats>().PlayerUnitInit("Dango");
         UC = GetComponent<UnitCombat>();
        UC.BaseAS = 3;
        UC.BaseArmor = 0;
        UC.HealthMax = hp;
        UC.AttackTorque = torque;
        GetComponent<UnitStats>().MoveSpeed = ms;
    }
    // Update is called once per frame
    void Update()
    {
        if (!used)
        {
            if (Time.time > 1)
            {
                UC.EquipWeapon(weaponID);
                used = true;
            }
        }
    }
}
