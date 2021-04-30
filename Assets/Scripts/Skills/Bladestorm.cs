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

    protected override void ForceStop()
    {
        _durationTimer = -1;
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 스킬설명: 타겟은 자신, 인트는 초당 데미지
    /// 3초동안 주변 적들이게 주어진 데미지만큼 1초마다 넣는다.
    /// 죽으면 바로 캔슬됨
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    protected override void SkillEffect(Transform target,int damage)
    {
        _durationTimer = SKILL_DURATION;
        _caster = transform;
        _damage = damage;
    }

    private void FixedUpdate()
    {
        transform.position = _caster.position;
        if(_durationTimer%50 == 0)
        {
            Collider2D[] hitByAttack = Physics2D.OverlapCircleAll(transform.position, RADIUS);
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
            _durationTimer--;
        }
        else
        {//스크립트 비활성화
            gameObject.SetActive(false);
        }
    }
}
