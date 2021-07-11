using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : SkillBase
{
    private const float SKILL_DURATION = 4;
    private UnitCombat _caster;
    private int _bonus = 0;
    private const float RADIUS = 0.7f;
    protected override void ForceStop() { }//단발성 스킬이라 필요 없음

    public override void UseSkill(UnitCombat caster)
    {
        print(this.name);
        if (Time.time > _timeReady)
        {
            Debug.Log("Skill Was able to be used");
            _timeReady = TotalCD + Time.time;
            SkillEffect(caster);
        }
    }

    /// <summary>
    /// 스킬설명: 인트는 방어력 보너스
    /// 4초간 지속되며 일정거리 내에 있는 적들의 타겟을 자신으로 강제설정한다
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    public override void SkillEffect(UnitCombat caster)
    {
        _caster = caster;
        switch (caster.GetItemRank())
        {
            case 0:
                _bonus = (int)(caster.TotalArmor * 0.2);
                break;
            case 1:
                _bonus = (int)(caster.TotalArmor * 0.4);
                break;
            case 2:
                _bonus = (int)(caster.TotalArmor * 0.6);
                break;
            case 3:
                _bonus = (int)(caster.TotalArmor * 0.8);
                break;
            default:
                break;
        }
        _caster.BaseArmor += _bonus;
        _caster.UpdateStats();


        Collider2D[] hitByAttack = Physics2D.OverlapCircleAll(transform.position, RADIUS);
        foreach (var hitUnit in hitByAttack)
        {
            UnitCombat hitCombat = hitUnit.GetComponent<UnitCombat>();
            if (hitCombat != null && hitCombat.OwnedFaction == Faction.Enemy)
            {
                hitCombat.AttackTarget = _caster.transform;
            }
        }
        StartCoroutine(Effect());
    }
    IEnumerator Effect()
    {
        yield return new WaitForSeconds(4);
        _caster.BaseArmor -= _bonus;
        _caster.UpdateStats();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
