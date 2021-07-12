using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public float BaseCD { get; set; } = 60;
    public float TotalCD { get; set; } = 5;
    protected float _timeReady = 0;

    /// <summary>
    /// 기본 쿨탐에서 주어진 skillHasteAmount 값만큼 쿨탐 제거
    /// </summary>
    /// <param name="skillHasteAmount"></param>
    /// <returns></returns>
    public void CooldownReductionSetup(float skillHasteAmount)
    {
        TotalCD = BaseCD * skillHasteAmount;
    }
    public abstract void UseSkill(UnitCombat caster);
    public abstract void SkillEffect(UnitCombat caster);
    protected abstract void ForceStop();
}
