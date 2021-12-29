﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : SkillBase
{
    private bool _goodToShoot = true;
    private UnitCombat _uc;
    private UnitCombat _target;
    protected override void ForceStop()
    {
        _goodToShoot = false;
    }

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

    /// <summary>
    /// 자체에 스턴걸고 시간 후에 평타 공격력의 10배 데미지를 방어무시로 때려박음.
    /// 대상이 없을경우 스킬 사용 실패 판정
    /// </summary>
    /// <param name="caster"></param>
    public override void SkillEffect(UnitCombat caster)
    {
        caster.AddStun(100);
        int dmg = caster.TotalDamage * 10;
        if (caster.AttackTarget == null)
        {
            failed(caster.transform);
            return;
        }
        _target = caster.AttackTarget.GetComponent<UnitCombat>();
        GameObject effect = Global.ObjectManager.SpawnObject("SnipeEffect");
        Transform head = caster.AttackTarget.Find("HEAD");
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
    IEnumerator  fire(int dmg,UnitCombat uc)
    {
        yield return new WaitForSeconds(2);
        if (_goodToShoot)
        {
            if (_target.IsDead == false)
            {
                StartCoroutine(CreateTrail(uc));
                Global.AudioManager.PlayOnce("SnipeSound");
                GameObject bullet = Global.ObjectManager.SpawnObject(Weapons.attackPrefab);
                bullet.transform.position = uc.AttackTarget.position;
                bullet.GetComponent<AttackEffect>().Setup(dmg, 0.01f, 999, uc.AttackImage, uc.ProjectileSpeed + 1, uc.AttackTarget.position, uc.TargetFaction);
            }
            else
            {
                failed(uc.transform);
            }
        }
    }
    IEnumerator CreateTrail(UnitCombat uc)
    {
        int numTrails = Mathf.CeilToInt(uc.OffsetToTargetBound() / 0.5f);
        Quaternion facing = Quaternion.LookRotation(Vector3.forward,uc.transform.position - uc.AttackTarget.position);
        for (int i = 0; i < numTrails; i++)
        {
            yield return new WaitForSeconds(0.05f);
            GameObject effect = Global.ObjectManager.SpawnObject("SnipeTrail");
            effect.transform.position = Vector3.MoveTowards(uc.transform.position, uc.AttackTarget.position, 0.3f * (i+1));
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
