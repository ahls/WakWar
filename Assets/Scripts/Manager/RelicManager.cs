using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public Dictionary<ClassType, ClassModifier> ClassModifiers = new Dictionary<ClassType, ClassModifier>();
    public bool RerollReward = false;
    public int GoldPerKill = 1;
    public int StatMutliplier = 1;
    public int BonusPanzee = 0;
    // Start is called before the first frame update
    void Start()
    {
        IngameManager.instance.SetRelicManager(this);
    }
    private void Awake()
    {
        
        ClassModifiers[ClassType.Shooter] = new ClassModifier();
        ClassModifiers[ClassType.Supporter] = new ClassModifier();
        ClassModifiers[ClassType.Wak] = new ClassModifier();
        ClassModifiers[ClassType.Warrior] = new ClassModifier();
        foreach (var modifier in ClassModifiers.Values)
        {
            modifier.init();
        }
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
            StatMutliplier += 1;
        }
        else if (itemID == 30003)
        {//몬티홀의 상자
            RerollReward = true;
        }
        else if(itemID == 30012)
        {//코기 인형

        }
        else
        {//그 외의 스탯 올려주는 유물들
            foreach (var effect in Items.DB[itemID].RelicEffects)
            {
                if (effect.ClassType == ClassType.Null)
                {
                    ClassModifiers[ClassType.Shooter].ApplyEffect(effect.attribute,effect.Value);
                    ClassModifiers[ClassType.Warrior].ApplyEffect(effect.attribute, effect.Value);
                    ClassModifiers[ClassType.Wak].ApplyEffect(effect.attribute, effect.Value);
                    ClassModifiers[ClassType.Supporter].ApplyEffect(effect.attribute, effect.Value);
                }
                else
                {
                    ClassModifiers[effect.ClassType].ApplyEffect(effect.attribute, effect.Value);
                }
            }
        }

    }

    public class ClassModifier
    {
        public static readonly string[] ATTRIBUTES = { "Damage", "AP", "Armor", "MaxHP", "MovementSpeed", "AttackSpeed", "CDReduction", "Regen", "CritChance","LifeSteal" };
        public Dictionary<string, float> Modifiers = new Dictionary<string, float>();

        public void init()
        {
            foreach (string attribute in ATTRIBUTES)
            {
                switch (attribute)
                {
                    case "MaxHP":
                        Modifiers[attribute] = 50;
                        break;
                    case "MovementSpeed":
                        Modifiers[attribute] = 0.1f;
                        break;
                    default:
                        Modifiers[attribute] = 0;
                        break;
                }
            }

        }
        public void ApplyEffect(string whichAttribute, float value)
        {
            Modifiers[whichAttribute] += value;
        }
    }
}
