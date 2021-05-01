using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : SkillBase
{
    private const int SKILL_DURATION = 200;
    private int _timer = -1;
    private UnitCombat _caster;
    private int _bonus = 0;
    private const float RADIUS = 0.7f;
    protected override void ForceStop() { }//단발성 스킬이라 필요 없음



    /// <summary>
    /// 스킬설명: 인트는 방어력 보너스
    /// 4초간 지속되며 일정거리 내에 있는 적들의 타겟을 자신으로 강제설정한다
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    protected override void SkillEffect(UnitCombat caster)
    {
        _caster = caster;
        switch (caster.GetItemRank())
        {
            case 0:
                _bonus = (int)((caster.BaseArmor +15) * 15);
                break;
            case 1:
                _bonus = (int)((caster.BaseArmor + 20) * 30);
                break;
            case 2:
                _bonus = (int)((caster.BaseArmor + 25) * 45);
                break;
            case 3:
                _bonus = (int)((caster.BaseArmor + 30) * 60);
                break;
            default:
                break;
        }
        _caster.BaseArmor += _bonus;


        Collider2D[] hitByAttack = Physics2D.OverlapCircleAll(transform.position, RADIUS);
        foreach (var hitUnit in hitByAttack)
        {
            UnitCombat hitCombat = hitUnit.GetComponent<UnitCombat>();
            if (hitCombat != null && hitCombat.OwnedFaction == Faction.Enemy)
            {
                hitCombat.AttackTarget = _caster.transform;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
        if(_timer > 0)
        {
            _timer--;
        }
        else if(_timer == 0)
        {
            _caster.BaseArmor -= _bonus;
            _timer = -1;
        }
    }
}
