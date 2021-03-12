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
    public float attackSpeed { get; set; } // 초당 공격
    private float attackTimer = 0; // 0일때 공격 가능
    public float attackArea { get; set; }
    public float projectileSpeed { get; set; }
    private Transform attackTarget;
    private float targetAcquisitionRange;
    //방어력
    public int armor { get; set; }

    public Sprite attackImage { get; set; }

    //타입
    public faction ownedFaction = faction.enemy;        //소유주. 유닛스탯에서 플레이어 init 할때 자동으로 아군으로 바꿔줌
    public faction targetFaction;                       //공격타겟
    public WeaponType weaponType;
    private UnitWeapon _weapon;
    private GameObject _effect;

    #endregion
    private void Start()
    {
        healthCurrent = healthMax;
        healthBar.maxValue = healthMax;
    }

    private void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else//else문을 넣음으로 공격에 후딜이 추가됨: 공격후 새로운 적을 잡거나 공격을 위해 적에게 다가가지 않음.
        {
            if (attackTarget != null)
            {
                if ((attackTarget.position - transform.position).magnitude <= attackRange)
                {//적이 사정거리 내에 있을경우
                    attack();
                }
                else
                {//적이 사정거리 내에 없을경우 타겟쪽으로 이동함

                }
            }
            /*
             공격 타겟이 없을경우 근처에 있는 적을 자동으로 추격하도록 할지 결정
             */

        }
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
    }

    public void UnEquipWeapon()
    {
        _weapon = null;
    }
    #endregion

    #region 공격관련
    public void attack()
    {
        //투사체 pull 해주세요
        //############
        if(_effect == null)
        {
            //_effect = Instantiate()
        }
        _effect.transform.position = transform.position;
        _effect.GetComponent<AttackEffect>().setup(this, attackTarget.position);

        attackTimer = 1 / attackSpeed;
        
    }
    public void targetAquire()
    {

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


