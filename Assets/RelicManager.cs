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
    public int StatMutliplier = 1;
    public int BonusPanzee = 0;
    // Start is called before the first frame update
    void Start()
    {
        IngameManager.instance.SetRelicManager(this);
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
