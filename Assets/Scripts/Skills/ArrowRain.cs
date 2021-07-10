using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRain : SkillBase
{
    private const float ARROW_AOE = 0.1f;
    private const float AOE = 0.2f;
    private const int AP = 0;
    private const int NUM_ARROWS = 6;
    protected override void ForceStop(){}//죽어도 쏜 화살은 계속 떨어짐
    /// <summary>
    /// 스킬설명: 타겟은 시전자, 인트는 데미지
    /// 약간의 딜레이 뒤, 시전자의 타겟근처에 범위공격
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    protected override void SkillEffect(UnitCombat caster)
    {
        UnitCombat uc = caster.GetComponent<UnitCombat>();
        if(uc.AttackTarget == null)
        {//현재 공격대상이 없으면 바로 머리위로 화살비 쏟아냄
            transform.position = caster.transform.position;
        }
       else
        {
            transform.position = uc.AttackTarget.position;
        }
        transform.rotation = Quaternion.identity;

        int dmg = caster.TotalDamage;
        for (int i = 0; i < NUM_ARROWS; i++)
        {
            transform.rotation = Quaternion.AngleAxis(60 * i, Vector3.forward);
            Vector2 arrowLandingLocation = transform.position + transform.up * AOE;
            GameObject arrow = Global.ObjectManager.SpawnObject(Weapons.attackPrefab);
            arrow.transform.position = transform.position;
            AttackEffect attackEffect = arrow.GetComponent<AttackEffect>();
            attackEffect.Setup(dmg,ARROW_AOE,AP,uc.AttackImage,0.1f, arrowLandingLocation, uc.TargetFaction);
            attackEffect.AddTrajectory(180, 0.2f);
        }
    }

}
