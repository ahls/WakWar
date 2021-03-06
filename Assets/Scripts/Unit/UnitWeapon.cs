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
    private int attackDamage, armor;
    private float attackSpeed, attackRange, attackArea;
    GameObject effect;
    WeaponType weaponType;

    public void equip(UnitCombat unit)
    {
        if (unit.weaponType != weaponType && unit.weaponType != WeaponType.Wak)
        {
            return;
        }
        unit.attackDamage += attackDamage;
        unit.attackSpeed += attackSpeed;
        unit.attackRange += attackRange;
        unit.armor += armor;
        unit.attackArea += attackArea;
        unit.effect = effect;
    }
    public void unequip(UnitCombat unit)
    {
        unit.attackDamage -= attackDamage;
        unit.attackSpeed -= attackSpeed;
        unit.attackRange -= attackRange;
        unit.armor -= armor;
        unit.attackArea -= attackArea;
        unit.effect = null;
    }
}
public class WeaponEffect : MonoBehaviour   
{
    public int damage;
    public float area;
    public SpriteRenderer SR;
    public Rigidbody2D RB;

    public void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

}
