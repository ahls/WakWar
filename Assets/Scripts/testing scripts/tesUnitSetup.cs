using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tesUnitSetup : MonoBehaviour
{
    public bool CanControl = false;
    public float  range,  AOE, projSpeed,ms,AS;
    public int hp,dmg,AP;
    public int weaponID;
    public float torque;
    private bool used = false;
    UnitCombat UC;
    private void Start()
    {
        if (CanControl)
        { GetComponent<UnitStats>().PlayerUnitInit("Dango"); }

         UC = GetComponent<UnitCombat>();
        UC.BaseAS = 0;
        UC.BaseArmor = 0;
        UC.HealthMax = hp;
        UC.AttackTorque = torque;
        UC.BaseRange = range;
        UC.BaseAS = AS;
        Debug.Log(gameObject.name + "'s range has been set");
        GetComponent<UnitStats>().MoveSpeed = ms;
        UC.UpdateStats();
    }
    // Update is called once per frame
    void Update()
    {
        if (!used)
        {
            if (Time.time > 1)
            {
                used = true;
                if(weaponID != 0)
                UC.EquipWeapon(weaponID);
            }
        }
    }
}
