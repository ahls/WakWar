using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public float BaseCD { get; set; }
    private float _cooldownModifier;
    private float _totalCD;
    private float _timeReady = 0;

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
    public void UseSkill(Transform target,int passinValue)
    {
        if(Time.time > _timeReady)
        {
            _timeReady = _totalCD + Time.time;
            SkillEffect(target, passinValue);
        }
    }
    protected abstract void SkillEffect(Transform target,int s);
    protected abstract void ForceStop();
}
