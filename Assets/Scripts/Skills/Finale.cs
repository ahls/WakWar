using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finale : SkillBase
{
    private const float DURATION = 5;
    private float _bonusAmount = 0;
    private GameObject _visualEffect;
    private List<UnitCombatNew> _affectedUnits = new List<UnitCombatNew>();
    protected override void ForceStop()
    {
        _bonusAmount = 0;
        Debug.LogWarning("THIS SKILL NEEDS TO IMPELMENET FORCE STOP");
    }
    public override void UseSkill(UnitController caster)
    {
        Global.AudioManager.SetInstrumPitch(caster.panzeeBehavior.GetItemRank());
        if (Time.time > _timeReady)
        {
            _timeReady = TotalCD + Time.time;
            SkillEffect(caster);
        }
    }

    public override void SkillEffect(UnitController caster)
    {
        foreach (var uc in _affectedUnits)
        {
            uc.BaseAS += _bonusAmount;
        }
        
        _bonusAmount *= 2;
        _visualEffect = Global.ObjectManager.SpawnObject("soundUlt");
        _visualEffect.transform.SetParent(caster.transform);
        _visualEffect.transform.localPosition = Vector3.zero;
        StartCoroutine(SkillExpire(caster));
    }
    private void SkillEffectTerminate(UnitController caster)
    {

        _bonusAmount = caster.unitCombat.TotalDamage;
        if (_visualEffect != null) Global.ObjectManager.ReleaseObject("soundUlt",_visualEffect);
        foreach (var uc in _affectedUnits)
        {
            uc.BaseAS -= _bonusAmount;
        }
    }
    private IEnumerator  SkillExpire(UnitController caster)
    {
        yield return new WaitForSeconds(DURATION);
        SkillEffectTerminate(caster);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UnitController uc = other.GetComponent<UnitController>();
        if (uc.healthSystem.OwnedFaction != Faction.Player) return;
        uc.unitCombat.BaseAS += _bonusAmount;
        uc.panzeeBehavior.UpdateStats();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        UnitController uc = other.GetComponent<UnitController>();
        if (uc.healthSystem.OwnedFaction != Faction.Player) return;
        uc.unitCombat.BaseAS -= _bonusAmount;
        uc.panzeeBehavior.UpdateStats();
    }
}
