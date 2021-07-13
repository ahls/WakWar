using System.Collections;
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
    /// </summary>
    /// <param name="caster"></param>
    public override void SkillEffect(UnitCombat caster)
    {
        caster.AddStun(100);
        int dmg = caster.TotalDamage * 10;
        if (caster.AttackTarget == null)
        {
            return;
        }
        _target = caster.AttackTarget.GetComponent<UnitCombat>();


        StartCoroutine(fire(dmg, caster));
        
    }
    IEnumerator  fire(int dmg,UnitCombat uc)
    {
        yield return new WaitForSeconds(2);
        if(_goodToShoot && _target.IsDead == false)
        {
            Global.AudioManager.PlayOnce("SnipeSound");
            GameObject bullet = Global.ObjectManager.SpawnObject(Weapons.attackPrefab);
            bullet.transform.position = transform.position;
            bullet.GetComponent<AttackEffect>().Setup(dmg, 0.01f, 999, uc.AttackImage, uc.ProjectileSpeed + 1,uc.AttackTarget.position, uc.TargetFaction);
        }
    }
}
