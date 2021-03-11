using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum faction { player, enemy, both }//유닛 컴뱃에 부여해서 피아식별
public class UnitCombat : MonoBehaviour
{
    #region 변수
    //체력
    public int healthMax { get; set; } = 10;
    private int healthCurrent;
    [SerializeField] private Slider healthBar;

    //공격관련
    public int attackDamage { get; set; }
    public float attackRange { get; set; }
    public float attackSpeed { get; set; }
    public float attackArea { get; set; }

    //방어력
    public int armor { get; set; }

    public GameObject effect { get; set; }

    //타입
    public faction ownedFaction;
    public WeaponType weaponType;
    private UnitWeapon _weapon;

    #endregion
    private void Start()
    {
        healthCurrent = healthMax;
        healthBar.maxValue = healthMax;
    }



    #region 장비관련
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
    #endregion



    #region 체력관련
    public void takeDamage(int damageAmount)
    {
        healthCurrent -= damageAmount;
        healthBarUpdate();
    }

    private void healthBarUpdate()
    {
        healthBar.value = healthCurrent;
    }
    #endregion 
}


