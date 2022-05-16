using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassHeal : SkillBase
{
    private const float RADIUS = 1f;
    private const string EFFECT_TARGET = "MassHealTarget";
    private const string EFFECT = "MassHealEffect";
    protected override void ForceStop() { }
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
    /// 현재 힐 대상 혹은 자기 주변에 광역으로 힐을 뿌립니다. 
    /// </summary>
    /// <param name="caster"></param>
    public override void SkillEffect(UnitController caster)
    {
        Global.AudioManager.PlayOnce("MassHeal");
        if(caster.unitCombat.attackTarget!=null)
        {//힐링 타겟이 있으면 그 주변을 기반으로 힐
            transform.position = caster.unitCombat.attackTarget.position;
        }
       else
        {//그 외에는 자기 중심으로 힐
            transform.position = caster.transform.position;
        }
        GameObject HealEffect = Global.ObjectManager.SpawnObject(EFFECT);
        HealEffect.transform.position = transform.position;
        int healAmount = caster.panzeeBehavior.GetItemRank() * 20 + 20;
        //20 ~ 80
        Collider2D[] hitByAttack = Physics2D.OverlapCircleAll(transform.position, RADIUS);
        foreach (var hitUnit in hitByAttack)
        {
            HealthSystem hitCombat = hitUnit.GetComponent<HealthSystem>();
            if (hitCombat != null && hitCombat.OwnedFaction == Faction.Player)
            {
                hitCombat.Heal(healAmount,EFFECT_TARGET);
            }
        }

    }

}
