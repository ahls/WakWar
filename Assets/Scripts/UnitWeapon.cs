using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Warrior,
    Shooter,
    Supporter,
    Wak
}

public class UnitWeapon : MonoBehaviour
{
    private float attackDamage, armor, attackSpeed, attackRange, attackArea;
    GameObject effect;
    WeaponType weaponType;

    public void equip(UnitCombat unit)
    {
        if (unit.weaponType != weaponType && unit.weaponType != WeaponType.Wak)
        {
            return;
        }

    }
}
