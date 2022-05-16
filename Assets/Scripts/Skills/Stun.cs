using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : SkillBase
{
    private UnitController _uc;
    private const float RADIUS = 0.5f;
    protected override void ForceStop()
    {
    }

    public override void UseSkill(UnitController caster)
    {
        print(this.name);
        if (Time.time > _timeReady)
        {
            _timeReady = TotalCD + Time.time;
            SkillEffect(caster);
        }
    }

    /// <summary>
    /// 광역스턴
    /// 주변 적들에게 스턴을 겁니당
    /// </summary>
    /// <param name="caster"></param>
    public override void SkillEffect(UnitController caster)
    {
        int stunAmount = (int)((caster.panzeeBehavior.GetItemRank() * + 2f) * 50);
        //2 ~ 5초
        Global.AudioManager.PlayOnce("StunSound");
        GameObject effect = Global.ObjectManager.SpawnObject("StunEffect");
        effect.transform.position = caster.transform.position;
        Collider2D[] hitByAttack = Physics2D.OverlapCircleAll(transform.position, RADIUS);
        foreach (var hitUnit in hitByAttack)
        {
            UnitController hitCombat = hitUnit.GetComponent<UnitController>();
            if (hitCombat != null && hitCombat.healthSystem.OwnedFaction == Faction.Enemy)
            {
                hitCombat.AddStun(stunAmount);
            }
        }
    }
}
