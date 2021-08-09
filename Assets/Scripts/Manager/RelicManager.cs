using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public ClassModifier WarriorModifier = new ClassModifier();
    public ClassModifier RangerModifier = new ClassModifier();
    public ClassModifier SupportModifier = new ClassModifier();
    public ClassModifier WakModifier = new ClassModifier();
    public bool RerollReward = false;
    public int GoldPerKill = 1;
    public int StatMutliplier = 1;
    public int BonusPanzee = 0;
    // Start is called before the first frame update
    void Start()
    {
        IngameManager.instance.SetRelicManager(this);
    }

    internal void EquipRelic(int itemID)
    {
        if (itemID == 31001)
        {//노잣돈 주머니 유물
            GoldPerKill += 2;
        }
        else if (itemID == 30001)
        {//난민의 증표
            BonusPanzee += 3;
        }
        else if (itemID == 30000)
        {//치킨 조각
            StatMutliplier +=1;
        }
        else if(itemID == 30003)
        {
            RerollReward = true;
        }
        else
        {//그 외의 스탯 올려주는 유물들

        }
        
    }
}

public class ClassModifier
{
    private static readonly string[] ATTRIBUTES = { "Damage", "AP", "Armor", "MaxHP", "MovementSpeed", "AttackSpeed", "CDReduction", "Regen", "CritChance" };
    public Dictionary<string, float> Modifiers = new Dictionary<string, float>(); 

    public void init()
    {
        foreach (string attribute in ATTRIBUTES)
        {
            Modifiers[attribute] = 0;
        }

    }
}
