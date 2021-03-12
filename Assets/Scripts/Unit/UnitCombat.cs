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

    //타겟 관련
    private Transform attackTarget;
    private int searchCooldown = 15;
    private int searchTimer;
    private static int searchAssign = 0;


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

        //모든 유닛이 같은 프레임에 대상을 탐지하는것을 방지
        searchTimer = searchAssign++ % searchCooldown;
        searchAssign %= searchCooldown;
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
        }
    }
    private void FixedUpdate()
    {
        if (searchTimer <= 0)
        {
            searchTimer = searchCooldown;
            if(attackTarget ==null)
            {
                search();
            }
        }
        else
        {
            searchTimer--;
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
    #endregion
    #region 탐색 관련
    private void search()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D selected in inRange)
        {
            UnitCombat selectedCombat = selected.GetComponent<UnitCombat>();
            if (selectedCombat != null)
            {
                if (selectedCombat.ownedFaction != ownedFaction)
                {
                    attackTarget = selectedCombat.transform;
                    return;
                }
            }
        }
        
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


