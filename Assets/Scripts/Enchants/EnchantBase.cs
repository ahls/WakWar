using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class EnchantDB
{
    public static List<Type> Enchants;
    public static List<Type> GetEnchants()
    {
        if (Enchants == null)
            GetAllEnchantments();
        return Enchants;
    }
    private static void GetAllEnchantments()
    {
        Enchants = new List<Type>();
        foreach (Type type in
            Assembly.GetAssembly(typeof(EnchantBase)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(EnchantBase)))) 
        {
            Enchants.Add(type);
        }
    }
}
 abstract public class EnchantBase
{
    public abstract string Name { get;}
    public abstract string Desc { get; }
    public bool Cursed;
    public virtual void Effect() { }
    public virtual void OnEquip(UnitCombat uc) { }
    public virtual void OnUnequip() { }
     
}

public class Enchant_Dmg : EnchantBase
{
    public override string Name => "예리한 ";

    public override string Desc => _desc;

    private string _desc;
    private int _amount;
    public Enchant_Dmg()
    {
        _amount = UnityEngine.Random.Range(1, 5);
        _desc = $"무기의 공격력 {_amount} 증가합니다.";
    }
    public override void OnEquip(UnitCombat uc)
    {
        uc.BaseDamage += _amount;
        uc.UpdateStats();
        uc.OnUnequipItem += Uc_OnUnequipItem;
    }

    private void Uc_OnUnequipItem(UnitCombat uc)
    {
        uc.BaseDamage -= _amount;
        uc.UpdateStats();
    }
}
public class Enchant_AttackSpeed : EnchantBase
{
    public override string Name => "가벼운 ";

    public override string Desc => _desc;

    private string _desc = "";
    private float _amount;
    
    public Enchant_AttackSpeed()
    {
        _amount = (float)UnityEngine.Random.Range(2, 5) / 10;
        _desc = $"공격속도 {_amount} 증가합니다.";
    }
    public override void Effect()
    {
        throw new System.NotImplementedException();
    }
}
public class Enchant_HP : EnchantBase
{
    public override string Name => "영양만점 ";
    public override string Desc => _desc;
    private string _desc;
    private int _amount;
    public Enchant_HP()
    {
        _amount = UnityEngine.Random.Range(1,5) * 10;
        _desc = $"체력 {_amount} 증가합니다.";
    }
}
public class Enchant_Armor : EnchantBase
{
    public override string Name => "철갑 ";
    public override string Desc => _desc;
    private string _desc;
    private int _amount;
    public Enchant_Armor()
    {
        _amount = UnityEngine.Random.Range(1, 5);
        _desc = $"방어력 {_amount} 증가합니다.";
    }
}
public class Enchant_AP : EnchantBase
{
    public override string Name => "꿰뚫는 ";
    public override string Desc => _desc;
    private string _desc;
    private int _amount;
    public Enchant_AP()
    {
        _amount = UnityEngine.Random.Range(1, 5);
        _desc = $"방어 관통 {_amount} 증가합니다.";
    }
}
public class Enchant_LifeDrain : EnchantBase
{
    public override string Name => "흡혈 ";
    public override string Desc => "피해의 10% 흡혈합니다.";
}

public class Enchant_Crit : EnchantBase
{
    public override string Name => "회심의 ";
    public override string Desc => _desc;
    private string _desc;
    private float _amount;
    public Enchant_Crit()
    {
        _amount = UnityEngine.Random.Range(1,10) * 0.05f;
        _desc = $"치명타 확률 {_amount * 100}% 증가합니다.";
    }
}

public class Enchant_heal : EnchantBase
{
    public override string Name => "자가수복 ";

    public override string Desc => "초당 체력 2 회복합니다.";
    public Enchant_heal()
    {
    }
}
public class Enchant_meteor : EnchantBase
{
    public override string Name => "궁극의 ";

    public override string Desc => "스킬시전시 메테오를 소환합니다.";
    public Enchant_meteor() { }
}