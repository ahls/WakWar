using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : SkillBase
{
    private bool _goodToShoot = true;
    private UnitCombat _uc;
    protected override void ForceStop()
    {
        _goodToShoot = false;
    }

    protected override void SkillEffect(UnitCombat caster)
    {
        caster.AddStun(50);
        int dmg = caster.TotalDamage * 10;
        StartCoroutine(fire(dmg, caster));
        
    }
    IEnumerator  fire(int dmg,UnitCombat uc)
    {
        yield return new WaitForSeconds(1);
        if(_goodToShoot && uc.AttackTarget != null)
        {
            GameObject bullet = Global.ResourceManager.LoadPrefab(Weapons.attackPrefab);
            bullet.transform.position = transform.position;
            bullet.GetComponent<AttackEffect>().Setup(dmg, 0.01f, 999, uc.AttackImage, 3, 0, 0,uc.AttackTarget.position, uc.TargetFaction);
        }
    }
}
