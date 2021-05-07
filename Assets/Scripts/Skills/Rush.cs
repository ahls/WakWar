using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : SkillBase
{
    private const float SKILL_DURATION = 5;
    private UnitCombat _caster;
    private float _bonus = 0;
    private const float RADIUS = 0.7f;
    protected override void ForceStop() { }//단발성 스킬이라 필요 없음



    /// <summary>
    /// 스킬설명: 인트는 방어력 보너스
    /// 4초간 지속되며 일정거리 내에 있는 적들의 타겟을 자신으로 강제설정한다
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    protected override void SkillEffect(UnitCombat caster)
    {
        _caster = caster;
        switch (caster.GetItemRank())
        {
            case 0:
                _bonus = caster.TotalAS * 0.2f;
                break;
            case 1:
                _bonus = caster.TotalAS * 0.4f;
                break;
            case 2:
                _bonus = caster.TotalAS * 0.6f;
                break;
            case 3:
                _bonus = caster.TotalAS * 0.7f;
                break;
            default:
                break;
        }
        caster.BaseAS += _bonus;
        _caster.UpdateStats();

        StartCoroutine(Effect());
    }
    IEnumerator Effect()
    {
        yield return new WaitForSeconds(SKILL_DURATION);
        _caster.TotalAS -= _bonus;
        _caster.UpdateStats();
    }

}
