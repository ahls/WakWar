using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : SkillBase
{
    private static float[] SKILL_COEFFICIENT = { 0.3f, 0.5f, 0.75f, 0.75f };
    private const float SKILL_DURATION = 5;
    private UnitController _caster;
    private float _bonus = 0;
    private const float RADIUS = 0.7f;
    protected override void ForceStop() { }//단발성 스킬이라 필요 없음

    public override void UseSkill(UnitController caster)
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
    public override void SkillEffect(UnitController caster)
    {
        _caster = caster;
        GameObject effect = Global.ObjectManager.SpawnObject("RushEffect");
        effect.transform.position = caster.transform.position;
        effect.transform.SetParent(caster.transform);
        _bonus = SKILL_COEFFICIENT[caster.panzeeBehavior.GetItemRank()] * caster.unitCombat.TotalAS;
        caster.unitCombat.BaseAS += _bonus;
        _caster.panzeeBehavior.UpdateStats();
        Global.AudioManager.PlayOnce("RushSound");
        StartCoroutine(Effect());
    }
    IEnumerator Effect()
    {
        yield return new WaitForSeconds(SKILL_DURATION);
        _caster.unitCombat.BaseAS -= _bonus;
        _caster.panzeeBehavior.UpdateStats();
    }

}
