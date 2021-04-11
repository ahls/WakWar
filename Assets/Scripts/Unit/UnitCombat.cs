using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum faction { player, enemy, both }//유닛 컴뱃에 부여해서 피아식별

public class UnitCombat : MonoBehaviour
{
    public enum ActionStats
    {
        Idle,
        Move,
        Attack
    }

    #region 변수

    private ActionStats _actionStat;

    public ActionStats ActionStat
    {
        get
        {
            return _actionStat;
        }
        set
        {
            ResetAttackTimer();
            ResetSearchTimer();

            _actionStat = value;
        }
    }


    public GameObject effectPrefab;

    //체력
    public int healthMax { get; set; } = 10;
    public bool _isDead { get; set; } = false;
    private int healthCurrent;
    [SerializeField] private Slider healthBar;

    //공격관련
    public int attackDamage { get; set; }
    public float attackRange { get; set; }
    public float attackSpeed { get; set; } // 초당 공격
    public float attackArea { get; set; }
    public int armorPiercing { get; set; }

    //타겟 관련
    public Transform attackTarget;
    private int searchCooldown = 15;
    private int searchTimer;
    private static int searchAssign = 0;


    //방어력
    public int armor { get; set; }


    //타입
    public faction ownedFaction = faction.enemy;        //소유주. 유닛스탯에서 플레이어 init 할때 자동으로 아군으로 바꿔줌
    public faction targetFaction;                       //공격타겟
    public WeaponType weaponType;
    private int weaponIndex;
    private GameObject _effect;
    public Sprite attackImage { get; set; }
    public float attackTorque { get; set; } = 0;
    private UnitStats _unitstats;
    [SerializeField] private SpriteRenderer _equippedImage;
    private Animator _animator;
    //장비 장착후 스탯
    public int resultDamage { get; set; }
    public float resultRange { get; set; }
    public float resultAOE { get; set; }
    public int resultAP { get; set; }
    public float projectileSpeed { get; set; }

    private float resultSpeed= 2;
    private float attackTimer = 0; // 0일때 공격 가능
    private int resultArmor;

    
    public void playerSetup(WeaponType inputWeaponType)
    {
        weaponType = inputWeaponType;
        ownedFaction = faction.player;
        HealthBarColor(Color.green);

    }

    #endregion
    private void Start()
    {
        _unitstats = GetComponent<UnitStats>();
        healthCurrent = healthMax;
        healthBar.maxValue = healthMax;
        HealthBarUpdate();
        //모든 유닛이 같은 프레임에 대상을 탐지하는것을 방지
        searchTimer = searchAssign++ % searchCooldown;
        searchAssign %= searchCooldown;

        ActionStat = ActionStats.Idle;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_unitstats._isMoving)
        {
            ActionStat = ActionStats.Move;
        }

        switch (ActionStat)
        {
            case ActionStats.Idle:
                {
                    if (attackTarget != null)
                    {
                        ActionStat = ActionStats.Attack;
                    }
                    break;
                }
            case ActionStats.Move:
                {
                    if (!_unitstats._isMoving)
                    {
                        ActionStat = ActionStats.Idle;
                    }
                    break;
                }
            case ActionStats.Attack:
                {
                    if (attackTarget != null && attackTarget.gameObject.activeSelf)
                    {
                        if (attackTimer > 0)
                        {
                            attackTimer -= Time.deltaTime;
                        }
                        else
                        {
                            if ((attackTarget.position - transform.position).magnitude <= resultRange)
                            {//적이 사정거리 내에 있을경우
                                Attack();
                            }
                            else
                            {//적이 사정거리 내에 없을경우 타겟쪽으로 이동함

                            }

                        }
                    }
                    else
                    {//타겟이 없거나 비활성화 되어있으면 바로 타겟 비우고 대기상태로 변환
                        attackTarget = null;
                        ActionStat = ActionStats.Idle;
                    }
                }
                break;

            default:
                {
                    break;
                }
        }
    }

    private void FixedUpdate()
    {
        switch (ActionStat)
        {
            case ActionStats.Idle:
                {
                    if (searchTimer <= 0)
                    {
                        ResetSearchTimer();//계속 돌려서 프레임당 최대한 적은 수의 탐색이 돌도록 함

                        if (!_unitstats._isMoving && attackTarget != null && OffSetToTarget() > resultRange)
                        {//움직이고 있지 않으며, 현재 타겟이 사정거리 밖으로 나가면 대상 취소
                            attackTarget = null;
                        }

                        if (attackTarget == null)
                        {
                            Search();
                        }
                    }
                    else
                    {
                        searchTimer--;
                    }

                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    #region 장비관련

    public void EquipWeapon(int weaponID)
    {       
        weaponIndex = weaponID;
        _equippedImage.sprite = Global.ResourceManager.LoadTexture(Weapons.DB[weaponIndex].equipImage);
        
        //장비 이미지 바꾸는 코드
        if (100200 <= weaponID && weaponID <= 100203)
        {
            _animator.SetTrigger("Shield");
        }
        else if (200000 <= weaponID && weaponID <= 200003)
        {
            _animator.SetTrigger("Gun");
        }
        else if (200100 <= weaponID && weaponID <= 200103)
        {
            _animator.SetTrigger("Bow");
        }
        else if (300200 <= weaponID && weaponID <= 300203)
        {
            _animator.SetTrigger("Inst");
        }
        else
        {
            _animator.SetTrigger("Regular");
        }
    }

    public void UnEquipWeapon()
    {
        _animator.SetTrigger("Regular");
        _equippedImage.sprite = null;
        switch (weaponType)
        {
            case WeaponType.Warrior:
            case WeaponType.Wak:
                weaponIndex = 10;
                break;
            case WeaponType.Shooter:
                weaponIndex = 20;
                break;
            case WeaponType.Supporter:
                weaponIndex = 30;
                break;
        }
    }
    public void UpdateStats()
    {
        resultDamage = attackDamage + Weapons.DB[weaponIndex].damage;
        resultAOE = attackArea + Weapons.DB[weaponIndex].AttackArea;
        resultRange = attackRange + Weapons.DB[weaponIndex].AttackRange;
        resultSpeed = attackSpeed + Weapons.DB[weaponIndex].AttackSpeed;
        resultArmor = armor + Weapons.DB[weaponIndex].Armor;
        
    }
    #endregion

    #region 공격관련

    public void Attack()
    {
        _animator.SetTrigger("Attack");
        _effect = Global.ResourceManager.LoadPrefab(effectPrefab.name);
        _effect.transform.position = transform.position;
        _effect.GetComponent<AttackEffect>().setup(this, attackTarget.position, effectPrefab.name,attackTorque);
        ResetAttackTimer();
    }

    private void ResetAttackTimer()
    {
        attackTimer = 1 / resultSpeed;
    }

    #endregion

    #region 탐색 관련

    private void Search()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, resultRange);
        foreach (Collider2D selected in inRange)
        {
            UnitCombat selectedCombat = selected.GetComponent<UnitCombat>();
            if (selectedCombat != null)
            {
                if (selectedCombat.ownedFaction != ownedFaction)
                {
                    attackTarget = selectedCombat.transform;
                    ActionStat = ActionStats.Attack;
                    return;
                }
            }
        }

    }

    private void ResetSearchTimer()
    {
        searchTimer = searchCooldown;
    }

    private float OffSetToTarget()
    {
        return (attackTarget.position - transform.position).magnitude;
    }

    #endregion

    #region 체력관련
    /// <summary>
    /// 방어무시 공격
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakeDamage(int damageAmount)
    {
        healthCurrent -= (damageAmount);
        HealthBarUpdate();
    }
    /// <summary>
    /// 방어관통 있는 버젼
    /// </summary>
    /// <param name="dmg"></param>
    /// <param name="armorPierce"></param>
    public void TakeDamage(int dmg, int armorPierce)
    {
        healthCurrent -= (dmg - Mathf.Clamp(resultArmor - armorPierce, 0, 999));
        HealthBarUpdate();
    }

    private void HealthBarUpdate()
    {
        healthBar.value = healthCurrent;
        if(healthCurrent<= 0)
        {
            death();
        }

    }
    private void HealthBarColor(Color newColor)
    {
       healthBar.transform.GetChild(0).GetComponent<Image>().color = newColor;

    }
    private void death()
    {
        _animator.SetTrigger("Die");
        _isDead = true;
        _unitstats._isMoving = false;
        _unitstats.setSelectionCircleState(false);
        _unitstats.Selectable = false;
        IngameManager.UnitManager.deselectUnit(gameObject);
        StartCoroutine(deathDelay());
    }
    IEnumerator deathDelay()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    #endregion
}