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
    private static float CURSE_CHANCE = 0.2f;
    public abstract string Name { get;}
    public abstract string Desc { get; }
    public bool Cursed = false;
    protected string cursedPrefix = "저주받은 ";
    protected string cursedDesc = "\n마법부여/장착해제 불가능. 최대체력 30% 감소, 판매가격 반감";
    public virtual void Effect() { }
    public virtual void OnEquip(Item_Drag itemDrag, PanzeeBehaviour pb) 
    {
        if (Cursed)
        {
            CurseOnEquip(itemDrag, pb);
        }
        else
        {
            pb.OnUnequipItem += OnUnequipItem;
            pb.UpdateStats();
        }
    }
    public abstract void OnUnequipItem(PanzeeBehaviour uc);
    protected bool CursedCheck()
    {
        if (UnityEngine.Random.value <= CURSE_CHANCE)
        {
            Global.AudioManager.PlayOnce("cursed_ambient");
            Cursed = true;
            return true;
        }
        return false;
    }
    protected void CurseOnEquip(Item_Drag itemDrag, PanzeeBehaviour uc)
    {
        itemDrag.SetDraggable(false);
        uc.unitController.healthSystem.HealthMax /= 3;
        uc.UpdateStats();
    }
    
}

public class Enchant_Dmg : EnchantBase
{
    public override string Name => _name;
    private string _name = "강화 ";

    public override string Desc => _desc;

    private string _desc;
    private int _amount;
    public Enchant_Dmg()
    {
        if (CursedCheck())
        {
            _amount = UnityEngine.Random.Range(3, 10);
            _desc = $"공격력이 {_amount} 증가. " +cursedDesc;
            _name = cursedPrefix + _name;
        }
        else
        {
            _amount = UnityEngine.Random.Range(1, 5);
            _desc = $"공격력이 {_amount} 증가.";
        }
    }
    public override void OnEquip(Item_Drag itemDrag, PanzeeBehaviour panzee)
    {
        panzee.uc.BaseDamage += _amount;
        base.OnEquip(itemDrag, panzee);
    }

    public override void OnUnequipItem(PanzeeBehaviour uc)
    {
        Debug.Log("UNEQUIP IS PROPERLY CALLD");
        uc.uc.BaseDamage -= _amount;
        uc.UpdateStats();
    }
}
public class Enchant_AttackSpeed : EnchantBase
{
    public override string Name => _name;

    public override string Desc => _desc;

    private string _desc = "";
    private float _amount;
    private string _name = "경량화 ";

    public Enchant_AttackSpeed()
    {
        if (CursedCheck())
        {

            _amount = (float)UnityEngine.Random.Range(3, 6) / 10;
            _desc = $"공격속도가 {_amount} 증가. " + cursedDesc;
            _name = cursedPrefix + _name;
        }
        else
        {
            _amount = (float)UnityEngine.Random.Range(2, 4) / 10;
            _desc = $"공격속도가 {_amount} 증가.";
        }
    }
    public override void OnEquip(Item_Drag itemDrag, PanzeeBehaviour uc)
    {
        uc.uc.BaseAS += _amount;
        base.OnEquip(itemDrag, uc);
    }

    public override void OnUnequipItem(PanzeeBehaviour uc)
    {
        uc.uc.BaseAS -= _amount;
        uc.UpdateStats();
    }
}
public class Enchant_HP : EnchantBase
{
    public override string Name => _name;
    private string _name = "영양만점 ";
    public override string Desc => _desc;
    private string _desc;
    private int _amount;
    public Enchant_HP()
    {
        if (CursedCheck())
        {
            _amount = UnityEngine.Random.Range(4, 10) * 10;
            _desc = $"체력이 {_amount} 증가. " + cursedDesc;
            _name = cursedPrefix + _name;
        }
        else
        {
            _amount = UnityEngine.Random.Range(1, 5) * 10;
            _desc = $"체력 {_amount} 증가.";
        }
    }
    public override void OnEquip(Item_Drag itemDrag, PanzeeBehaviour uc)
    {
        uc.unitController.healthSystem.HealthMax += _amount;
        base.OnEquip(itemDrag, uc);
    }

    public override void OnUnequipItem(PanzeeBehaviour uc)
    {
        uc.unitController.healthSystem.HealthMax -= _amount;
        uc.UpdateStats();
    }
}
public class Enchant_Armor : EnchantBase
{
    public override string Name => _name;
    private string _name = "철갑 ";
    public override string Desc => _desc;
    private string _desc;
    private int _amount;
    public Enchant_Armor()
    {
        if (CursedCheck())
        {
            _amount = UnityEngine.Random.Range(4, 10) * 10;
            _desc = $"방어력이 {_amount} 증가. " + cursedDesc;
            _name = cursedPrefix + _name;
        }
        else
        {
            _amount = UnityEngine.Random.Range(1, 5);
            _desc = $"방어력 {_amount} 증가.";
        }
    }
    public override void OnEquip(Item_Drag itemDrag, PanzeeBehaviour uc)
    {
        uc.unitController.healthSystem.baseArmor += _amount;
        base.OnEquip(itemDrag, uc);
    }

    public override void OnUnequipItem(PanzeeBehaviour uc)
    {
        uc.unitController.healthSystem.baseArmor -= _amount;
        uc.UpdateStats();
    }
}
public class Enchant_AP : EnchantBase
{
    public override string Name => _name;
    private string _name = "관통 ";
    public override string Desc => _desc;
    private string _desc;
    private int _amount;
    public Enchant_AP()
    {
        if (CursedCheck())
        {
            _amount = UnityEngine.Random.Range(2, 6);
            _desc = $"방어관통이 {_amount} 증가. " + cursedDesc;
            _name = cursedPrefix + _name;
        }
        else
        {
            _amount = UnityEngine.Random.Range(1, 3);
            _desc = $"방어관통이 {_amount} 증가.";
        }
    }
    public override void OnEquip(Item_Drag itemDrag, PanzeeBehaviour uc)
    {
        uc.uc.BaseAP += _amount;
        base.OnEquip(itemDrag, uc);
    }

    public override void OnUnequipItem(PanzeeBehaviour uc)
    {
        uc.uc.BaseAP -= _amount;
        uc.UpdateStats();
    }
}
public class Enchant_LifeDrain : EnchantBase
{
    public override string Name => _name;
    private string _name = "흡혈 ";
    public override string Desc => _desc;
    private string _desc;
    private float _amount;
    public Enchant_LifeDrain()
    {
        if(CursedCheck())
        {
            _amount = 0.2f;
            _desc = $"피해의 {_amount*100}% 흡혈. " + cursedDesc;
            _name = cursedPrefix + _name;
        }
        else
        {
            _amount = 0.1f;
            _desc = $"피해의 {_amount * 100}% 흡혈.";
        }
    }
    public override void OnEquip(Item_Drag itemDrag, PanzeeBehaviour uc)
    {
        uc.uc.BaseLD += 0.1f;
        base.OnEquip(itemDrag, uc);
    }

    public override void OnUnequipItem(PanzeeBehaviour uc)
    {
        uc.uc.BaseLD -= 0.1f;
        uc.UpdateStats();
    }
}

public class Enchant_Crit : EnchantBase
{
    public override string Name => _name;
    private string _name = "회심의 ";
    public override string Desc => _desc;
    private string _desc;
    private float _amount;
    public Enchant_Crit()
    {
        if (CursedCheck())
        {

            _amount = UnityEngine.Random.Range(4, 10) * 0.05f;
            _desc = $"치명타 확률 {_amount * 100}% 증가." + cursedDesc;
            _name = cursedPrefix + _name;
        }
        else
        {
            _amount = UnityEngine.Random.Range(1, 4) * 0.05f;
            _desc = $"치명타 확률 {_amount * 100}% 증가.";
        }
    }
    public override void OnEquip(Item_Drag itemDrag, PanzeeBehaviour uc)
    {
        uc.uc.BaseCrit += _amount;
        base.OnEquip(itemDrag, uc);
    }

    public override void OnUnequipItem(PanzeeBehaviour uc)
    {
        uc.uc.BaseCrit -= _amount;
        uc.UpdateStats();
    }
}

public class Enchant_heal : EnchantBase
{
    public override string Name => _name;
    private string _name = "자가수복 ";

    public override string Desc => _desc;
    private string _desc = "초당 체력 2 회복.";
    private int _amount = 2;
    public Enchant_heal()
    {
    if(CursedCheck())
        {
            _amount += 2;
            _desc += cursedDesc;
            _name = cursedPrefix + _name;
        }
    }
    public override void OnEquip(Item_Drag itemDrag, PanzeeBehaviour uc)
    {
        uc.OnEachSecondAlive += heal; ;
        base.OnEquip(itemDrag, uc);
    }

    private void heal(PanzeeBehaviour uc)
    {
        uc.unitController.healthSystem.Heal(_amount);
    }

    public override void OnUnequipItem(PanzeeBehaviour uc)
    {
        uc.OnEachSecondAlive -= heal;
    }
}
/*
public class Enchant_meteor : EnchantBase
{
    public override string Name => "궁극의 ";

    public override string Desc => "스킬시전시 메테오를 소환.";
    public Enchant_meteor() { }
}
*/