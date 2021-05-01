using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject BladeStormPrefab, TauntPrefab, ChargePrefab;  //전사스킬
    public GameObject SnipePrefab, ArrowRainPrefab, RushPrefab;     //사수스킬
    public GameObject StunPrefab, HealPrefab, FinalePrefab;         //지원스킬
    // Start is called before the first frame update
    void Start()
    {
        IngameManager.instance.SetSkillManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetSkill(string skillName)
    {
        switch (skillName)
        {
            case "BladeStorm":
                return BladeStormPrefab;
            case "Taunt":
                return TauntPrefab;
            case "Charge":
                return ChargePrefab;
            case "Snipe":
                return SnipePrefab;
            case "ArrowRain":
                return ArrowRainPrefab;
            default:
                return null;
        }
    }
}
