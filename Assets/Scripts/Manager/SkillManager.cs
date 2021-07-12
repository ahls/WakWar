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
    public GameObject GetSkill(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Axe:
                return BladeStormPrefab;
            case WeaponType.Sword:
                return ChargePrefab;
            case WeaponType.Shield:
                return TauntPrefab;
            case WeaponType.Bow:
                return ArrowRainPrefab;
            case WeaponType.Gun:
                return SnipePrefab;
            case WeaponType.Throw:
                return RushPrefab;
            case WeaponType.Blunt:
                return StunPrefab;
            case WeaponType.Wand:
                return HealPrefab;
            case WeaponType.Instrument:
                return FinalePrefab;
            default:
                return null;
        }
    }
}
