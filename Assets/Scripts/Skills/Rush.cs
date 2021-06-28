﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : SkillBase
{
    private const float SKILL_DURATION = 4;
    private UnitCombat _caster;
    private float _bonus = 0;
    private const float RADIUS = 0.7f;
    protected override void ForceStop() { }//단발성 스킬이라 필요 없음



    /// <summary>
    /// 스킬설명: 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    protected override void SkillEffect(UnitCombat caster)
    {
        _caster = caster;
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
