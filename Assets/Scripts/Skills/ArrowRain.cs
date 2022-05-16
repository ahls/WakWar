using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRain : SkillBase
{
    private const float ARROW_AOE = 0.1f;
    private const int AP = 0;
    private const int NUM_ARROWS = 6;
    private static Sprite ARROW_IMAGE = null;
    protected override void ForceStop(){}//죽어도 쏜 화살은 계속 떨어짐
    private void OnEnable()
    {
        if(ARROW_IMAGE == null)
        {
            ARROW_IMAGE = Global.ResourceManager.LoadTexture("arrow_rain");
        }
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
    /// 스킬설명: 타겟은 시전자, 인트는 데미지
    /// 약간의 딜레이 뒤, 시전자의 타겟근처에 범위공격
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    public override void SkillEffect(UnitController caster)
    {
        if (caster.unitCombat.attackTarget == null)
        {//현재 공격대상이 없으면 실패판정
            GameObject failEffect = Global.ObjectManager.SpawnObject("skillFail");
            failEffect.transform.position = caster.transform.position;
            failEffect.transform.parent = caster.transform;
            return;
        }
        Global.AudioManager.PlayOnce("ArrowRain");
        caster.AddStun(40);
        caster.panzeeBehavior.PlaySkillAnim();
        transform.position = caster.unitCombat.attackTarget.position;
        transform.rotation = Quaternion.identity;

        int dmg = caster.unitCombat.TotalDamage;
        Vector3 arrowStartLocation = caster.transform.position;
        for (int i = 0; i <NUM_ARROWS*(1 + caster.panzeeBehavior.GetItemRank() )  ; i++)
        {
            //transform.rotation = Quaternion.AngleAxis(60 * i, Vector3.forward);
            //Vector2 arrowLandingLocation = transform.position + transform.up * Random.Range(0.1f,0.5f);
            //GameObject arrow = Global.ObjectManager.SpawnObject(Weapons.attackPrefab);
            //arrow.transform.position = transform.position + Vector3.up * 10;
            //AttackEffect attackEffect = arrow.GetComponent<AttackEffect>();
            //float randomSpeed = Random.Range(-0.3f,0.5f) + 5.5f;
            //attackEffect.Setup(dmg,ARROW_AOE,AP,ARROW_IMAGE,randomSpeed, arrowLandingLocation, caster.TargetFaction);
            //attackEffect.AddSound("ArrowHit");
            transform.rotation = Quaternion.AngleAxis(60 * i, Vector3.forward);
            Vector2 arrowLandingLocation = transform.position + transform.up * Random.Range(0.1f, 0.5f);
            GameObject arrow = Global.ObjectManager.SpawnObject(Weapons.attackPrefab);
            arrow.transform.position = arrowStartLocation;
            AttackEffect attackEffect = arrow.GetComponent<AttackEffect>();
            float randomAirTime = Random.Range(-0.15f, 0.15f) + 1f;
            attackEffect.SetupWithFixedAirtime(dmg, ARROW_AOE, AP, ARROW_IMAGE, randomAirTime, arrowLandingLocation, caster.unitCombat.TargetFaction);
            attackEffect.AddTrajectory(0, 0.4f);
            attackEffect.SetAngle(180);
            GameObject trail = Global.ObjectManager.SpawnObject("ArrowRainTrailEffect");
            trail.transform.position = arrow.transform.position;
            trail.transform.SetParent(arrow.transform);
            attackEffect.AddSound("ArrowHit");
        }
    }

}
