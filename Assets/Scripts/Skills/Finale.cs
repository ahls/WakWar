using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finale : SkillBase
{
    private const float DURATION = 5;
    private float _bonusAmount = 0;
    private List<UnitCombat> _affectedUnits = new List<UnitCombat>();
    protected override void ForceStop()
    {
        _bonusAmount = 0;
    }

    protected override void SkillEffect(UnitCombat caster)
    {
        foreach (var uc in _affectedUnits)
        {
            uc.BaseAS += _bonusAmount;
        }
        _bonusAmount *= 2;
        
        StartCoroutine(SkillExpire(caster));
    }

    private IEnumerator  SkillExpire(UnitCombat caster)
    {
        yield return new WaitForSeconds(DURATION);
        SetBonusSpeed(caster);
        foreach (var uc in _affectedUnits)
        {
            uc.BaseAS -= _bonusAmount;
        }
    }

    public void SetBonusSpeed(UnitCombat caster)
    {
        _bonusAmount = caster.TotalDamage;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        UnitCombat uc = other.GetComponent<UnitCombat>();
        uc.BaseAS += _bonusAmount;
        uc.UpdateStats();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        UnitCombat uc = other.GetComponent<UnitCombat>();
        uc.BaseAS -= _bonusAmount;
        uc.UpdateStats();
    }
}
