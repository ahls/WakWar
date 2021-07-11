﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : SkillBase
{
    private const float SKILL_DURATION = 4;
    private UnitCombat _caster;
    private float _bonus = 0;
    private const float RADIUS = 0.7f;
    protected override void ForceStop() { }//안죽는 스킬이라 상관 없음

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
    /// 스킬설명: 
    /// 지속시간동안 죽지 않음
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    public override void SkillEffect(UnitCombat caster)
    {
        _caster = caster;
        _bonus = caster.GetItemRank() * 0.2f + 0.1f;
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
