using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public float BaseCD { get; set; }
    protected float _cooldownModifier;
    protected float _totalCD;
    protected float _timeReady = 0;

    /// <summary>
    /// 퍼센트로 입력, 1이 100프로
    /// </summary>
    /// <param name="amount"></param>
    public void CooldownOffset(float amount)
    {
        _cooldownModifier += amount;
        _cooldownModifier = Mathf.Max(0.3f, _cooldownModifier); //최대 70퍼센트까지 감소 가능
        _totalCD = BaseCD * _cooldownModifier;
    }
    public void UseSkill(UnitCombat caster)
    {
        if(Time.time > _timeReady)
        {
            _timeReady = _totalCD + Time.time;
            SkillEffect(caster);
        }
    }
    protected abstract void SkillEffect(UnitCombat caster);
    protected abstract void ForceStop();
}
