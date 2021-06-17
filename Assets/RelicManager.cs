using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public ClassModifier WarriorModifier = new ClassModifier();
    public ClassModifier RangerModifier = new ClassModifier();
    public ClassModifier SupportModifier = new ClassModifier();
    public ClassModifier WakModifier = new ClassModifier();
    public bool ShowRewardValue = false;
    public int GoldPerKill = 1;
    public int BonusPanzee = 0;
    // Start is called before the first frame update
    void Start()
    {
        IngameManager.instance.SetRelicManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class ClassModifier
{
    public int Damage = 0;
    public int AP = 0;
    public int Armor = 0;
    public int MaxHP = 0;
    public float MovementSpeed = 0;
    public float AttackSpeed = 0;
    public float CooldownReduction = 0;
    public float CriticalChance = 0;
    public float Regen = 0;

    public void reset()
    {
        Damage = 0;
        AP = 0;
        Armor = 0;
        MaxHP = 0;
        MovementSpeed = 0;
        AttackSpeed = 0;
        CooldownReduction = 0;
        Regen = 0;
        CriticalChance = 0;
    }
}
