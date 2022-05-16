using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : SkillBase
{
    private const float SKILL_DURATION = 4;
    private UnitController _caster;
    protected override void ForceStop() { }//안죽는 스킬이라 상관 없음

    public override void UseSkill(UnitController caster)
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
    /// 지속시간동안 체력이 1 밑으로 떨어지지 않고, 패시브로 잃은 체력에 비례해서 추가데미지를 줌.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    public override void SkillEffect(UnitController caster)
    {
        caster.healthSystem.CanBeKilled = false;
        _caster = caster;
        GameObject effect = Global.ObjectManager.SpawnObject("BerserkEffect");
        effect.transform.position = caster.transform.position;
        effect.transform.SetParent(caster.transform);

        StartCoroutine(Effect());
    }
    IEnumerator Effect()
    {
        yield return new WaitForSeconds(SKILL_DURATION);
        _caster.healthSystem.CanBeKilled = true;
   
    }

}
