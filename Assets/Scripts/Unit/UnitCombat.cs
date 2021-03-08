using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    #region 변수

    //공격관련
    public int attackDamage { get; set; }
    public float attackRange { get; set; }
    public float attackSpeed { get; set; }
    public float attackArea { get; set; }

    //방어력
    public int armor { get; set; }

    public GameObject effect { get; set; }

    //타입
    public WeaponType weaponType;

    private UnitWeapon _weapon;

    #endregion

    public void EquipWeapon(UnitWeapon weapon)
    {
        if (weapon.weaponType != weaponType && weapon.weaponType != WeaponType.Wak)
        {
            Global.UIManager.PushNotiMsg("직업에 맞지 않는 장비입니다.", 1f);
            return;
        }

        _weapon = weapon;
        effect = weapon.effect;
    }

    public void UnEquipWeapon()
    {
        _weapon = null;
        effect = null;
    }
}
