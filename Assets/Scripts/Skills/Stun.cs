﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : SkillBase
{
    private bool _goodToShoot = true;
    private UnitCombat _uc;
    private const float RADIUS = 0.5f;
    protected override void ForceStop()
    {
        _goodToShoot = false;
    }
    /// <summary>
    /// 광역스턴
    /// 주변 적들에게 스턴을 겁니당
    /// </summary>
    /// <param name="caster"></param>
    protected override void SkillEffect(UnitCombat caster)
    {
        int stunAmount = (int)((caster.GetItemRank() * 0.5f + 1.5f) * 50);
        Collider2D[] hitByAttack = Physics2D.OverlapCircleAll(transform.position, RADIUS);
        foreach (var hitUnit in hitByAttack)
        {
            UnitCombat hitCombat = hitUnit.GetComponent<UnitCombat>();
            if (hitCombat != null && hitCombat.OwnedFaction == Faction.Enemy)
            {
                hitCombat.AddStun(stunAmount);
            }
        }
    }
}