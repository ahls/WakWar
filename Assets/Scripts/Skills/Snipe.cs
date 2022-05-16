using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : SkillBase
{
    private bool _goodToShoot = true;
    private UnitController _uc;
    private HealthSystem _target;
    protected override void ForceStop()
    {
        _goodToShoot = false;
    }

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
    /// 자체에 스턴걸고 시간 후에 평타 공격력의 10배 데미지를 방어무시로 때려박음.
    /// 대상이 없을경우 스킬 사용 실패 판정
    /// </summary>
    /// <param name="caster"></param>
    public override void SkillEffect(UnitController caster)
    {
        caster.AddStun(100);
        int dmg = caster.unitCombat.TotalDamage * 10;
        if (caster.unitCombat.attackTarget == null)
        {
            failed(caster.transform);
            return;
        }
        _target = caster.unitCombat.attackTarget.GetComponent<HealthSystem>();
        GameObject effect = Global.ObjectManager.SpawnObject("SnipeEffect");
        Transform head = caster.unitCombat.attackTarget.Find("HEAD");
        if (head != null)
        {
            effect.transform.SetParent(head);
        }
        else
        {
            effect.transform.SetParent(_target.transform);
        }
        StartCoroutine(fire(dmg, caster));
    }
    IEnumerator  fire(int dmg, UnitController uc)
    {
        yield return new WaitForSeconds(2);
        if (_goodToShoot)
        {
            if (_target.IsDead == false)
            {
                StartCoroutine(CreateTrail(uc));
                Global.AudioManager.PlayOnce("SnipeSound");
                GameObject bullet = Global.ObjectManager.SpawnObject(Weapons.attackPrefab);
                bullet.transform.position = uc.unitCombat.attackTarget.position;
                bullet.GetComponent<AttackEffect>().Setup(dmg, 0.005f, 999, uc.unitCombat.AttackImage, uc.unitCombat.ProjectileSpeed + 1, 
                                                            uc.unitCombat.attackTarget.position, uc.unitCombat.TargetFaction);
            }
            else
            {
                failed(uc.transform);
            }
        }
    }
    IEnumerator CreateTrail(UnitController uc)
    {
        int numTrails = Mathf.CeilToInt(uc.unitCombat.OffsetToTargetBound() / 0.5f);
        Quaternion facing = Quaternion.LookRotation(Vector3.forward,uc.transform.position - uc.unitCombat.attackTarget.position);
        Vector3 targetPosition = uc.unitCombat.attackTarget.position;
        for (int i = 1; i < numTrails+1; i++)
        {
            yield return new WaitForSeconds(0.05f);
            GameObject effect = Global.ObjectManager.SpawnObject("SnipeTrail");
            effect.transform.position = Vector3.MoveTowards(uc.transform.position, targetPosition, 0.1f * (i*i));
            effect.transform.rotation = facing;
        }
    }
    private void failed(Transform caster)
    {
        GameObject effect = Global.ObjectManager.SpawnObject("skillFail");
        effect.transform.position = caster.position;
        effect.transform.parent = caster;

    }
}
