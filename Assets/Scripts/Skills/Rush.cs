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

    public override void UseSkill(UnitCombat caster)
    {
        if (Time.time > _timeReady)
        {
            Debug.Log("RUSH IS ON Use");

            _timeReady = TotalCD + Time.time;
            SkillEffect(caster);
        }
    }


    /// <summary>
    /// 스킬설명: 인트는 공격속도 보너스
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
                _bonus = caster.TotalAS * 0.3f;
                break;
            case 1:
                _bonus = caster.TotalAS * 0.5f;
                break;
            case 2:
                _bonus = caster.TotalAS * 0.75f;
                break;
            case 3:
                _bonus = caster.TotalAS * 0.75f;
                break;
            default:
                break;
        }
        caster.BaseAS += _bonus;
        _caster.UpdateStats();
        Global.AudioManager.PlayOnce("RushSound");
        StartCoroutine(Effect());
    }
    IEnumerator Effect()
    {
        yield return new WaitForSeconds(SKILL_DURATION);
        _caster.BaseAS -= _bonus;
        _caster.UpdateStats();
    }

}
