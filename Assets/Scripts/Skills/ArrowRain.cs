using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRain : SkillBase
{
    private const float ARROW_AOE = 0.1f;
    private const int AP = 0;
    private const int NUM_ARROWS = 6;
    protected override void ForceStop(){}//죽어도 쏜 화살은 계속 떨어짐

    public override void UseSkill(UnitCombat caster)
    {
        print(this.name);
        if (Time.time > _timeReady)
        {
            _timeReady = TotalCD + Time.time;
            SkillEffect(caster);
        }
    }

    /// <summary>
    /// 스킬설명: 타겟은 시전자, 인트는 데미지
    /// 약간의 딜레이 뒤, 시전자의 타겟근처에 범위공격
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    public override void SkillEffect(UnitCombat caster)
    {
        if (caster.AttackTarget == null)
        {//현재 공격대상이 없으면 실패판정
            GameObject effect = Global.ObjectManager.SpawnObject("skillFail");
            effect.transform.position = caster.transform.position;
            effect.transform.parent = caster.transform;
            effect.GetComponent<Effect>().PlayAnimation();
            return;
        }
        Global.AudioManager.PlayOnce("ArrowRain");
        caster.AddStun(40);
        caster.PlaySkillAnim();

        transform.position = caster.AttackTarget.position;
       
        transform.rotation = Quaternion.identity;

        int dmg = caster.TotalDamage;
        for (int i = 0; i < NUM_ARROWS + caster.GetItemRank() * 3 ; i++)
        {
            transform.rotation = Quaternion.AngleAxis(60 * i, Vector3.forward);
            Vector2 arrowLandingLocation = transform.position + transform.up * Random.Range(0.1f,0.5f);
            GameObject arrow = Global.ObjectManager.SpawnObject(Weapons.attackPrefab);
            arrow.transform.position = transform.position + Vector3.up * 10;
            AttackEffect attackEffect = arrow.GetComponent<AttackEffect>();
            float randomSpeed = Random.Range(-0.3f,0.5f) + 5.5f;
            attackEffect.Setup(dmg,ARROW_AOE,AP, caster.AttackImage,randomSpeed, arrowLandingLocation, caster.TargetFaction);
            attackEffect.AddSound(caster.ImpactAudio);
        }
    }

}
