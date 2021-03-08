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
    public int AttackDamage { get; }
    public int Armor { get; }
    public float AttackSpeed { get; }
    public float AttackRange { get; }
    public float AttackArea { get; }

    public GameObject effect;
    public WeaponType weaponType;
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
