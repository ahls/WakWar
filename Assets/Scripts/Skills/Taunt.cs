using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : SkillBase
{
    private static float[] SKILL_COEFFICIENT = { 0.2f, 0.4f, 0.6f, 0.8f };
    private const float SKILL_DURATION = 4;
    private UnitController _caster;
    private int _bonus = 0;
    private const float RADIUS = 0.7f;
    protected override void ForceStop() { }//단발성 스킬이라 필요 없음

    public override void UseSkill(UnitController caster)
    {
        print(this.name);
        if (Time.time > _timeReady)
        {
            Debug.Log("Skill Was able to be used");
            _timeReady = TotalCD + Time.time;
            SkillEffect(caster);
        }
    }

    /// <summary>
    /// 스킬설명: 인트는 방어력 보너스
    /// 4초간 지속되며 일정거리 내에 있는 적들의 타겟을 자신으로 강제설정한다
    /// </summary>
    /// <param name="target"></param>
    /// <param name="s"></param>
    public override void SkillEffect(UnitController caster)
    {
        _caster = caster;
        _bonus = (int)((caster.healthSystem.totalArmor) * SKILL_COEFFICIENT[caster.panzeeBehavior.GetItemRank()]);
        _caster.healthSystem.baseArmor += _bonus;
        _caster.panzeeBehavior.UpdateStats();
        Global.AudioManager.PlayOnce("TauntSound");
        GameObject effect = Global.ObjectManager.SpawnObject("TauntEffect");
        effect.transform.position = caster.transform.position;
        effect.transform.SetParent(caster.transform);

        Collider2D[] hitByAttack = Physics2D.OverlapCircleAll(transform.position, RADIUS);
        foreach (var hitUnit in hitByAttack)
        {
            UnitController hitController = hitUnit.GetComponent<UnitController>();
            if (hitController != null && hitController.healthSystem.OwnedFaction == Faction.Enemy)
            {
                hitController.unitCombat.attackTarget = _caster.transform;
            }
        }
        StartCoroutine(Effect());
    }
    IEnumerator Effect()
    {
        yield return new WaitForSeconds(4);
        _caster.healthSystem.baseArmor -= _bonus;
        _caster.panzeeBehavior.UpdateStats();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
