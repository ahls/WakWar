using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bladestorm : SkillBase
{
    private const int SKILL_DURATION = 150;
    private const float RADIUS = 0.3f;
    private int _durationTimer = -1;
    private int _damage;
    private Transform _caster;
    public override void UseSkill(UnitCombat caster)
    {
        print(this.name);
        if (Time.time > _timeReady)
        {
            Debug.Log("Skill Was able to be used");
            _timeReady = TotalCD + Time.time;
            SkillEffect(caster);
        }
    }

    protected override void ForceStop()
    {
        _durationTimer = -1;
    }
    /// <summary>
    /// 스킬설명: 타겟은 자신, 인트는 초당 데미지
    /// 3초동안 주변 적들이게 주어진 데미지만큼 1초마다 넣는다.
    /// 죽으면 바로 캔슬됨
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    public override void SkillEffect(UnitCombat caster)
    {

        _durationTimer = SKILL_DURATION;
        _caster = caster.transform;
        _damage = caster.TotalDamage - (2-caster.GetItemRank());
        caster.PlaySkillAnim();
        Global.AudioManager.PlayLoop("BladeStormLoop", 3);
        GameObject effect = Global.ObjectManager.SpawnObject("bladestorm");
        effect.transform.parent = caster.transform;
        effect.transform.position = caster.transform.position;

    }

    private void FixedUpdate()
    {
        if (_durationTimer == -1) return;
        if(_durationTimer%50 == 0)
        {
            Collider2D[] hitByAttack = Physics2D.OverlapCircleAll(transform.position, RADIUS);
            Global.AudioManager.PlayOnce("Cut",true);
            foreach (var hitUnit in hitByAttack)
            {
                UnitCombat hitCombat = hitUnit.GetComponent<UnitCombat>();
                if(hitCombat!=null && hitCombat.OwnedFaction == Faction.Enemy)
                {
                    hitCombat.TakeDamage(_damage);
                }
            }
        }
        if(_durationTimer>0)
        {
            transform.position = _caster.position;
            _durationTimer--;
        }
        else if(_durationTimer == 0)
        {//스크립트 비활성화
            _caster.GetComponent<UnitCombat>().PlaySkillAnim();
            Debug.Log("Skill is Ending");
            _durationTimer = -1;
        }
    }
}
