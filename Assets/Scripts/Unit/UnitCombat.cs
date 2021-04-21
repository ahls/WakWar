using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Faction { Player, Enemy, Both }//유닛 컴뱃에 부여해서 피아식별

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
    public int HealthMax { get; set; } = 10;
    public bool IsDead { get; set; } = false;
    private int _healthCurrent;
    [SerializeField] private Slider healthBar;

    //공격관련
    public int BaseDamage { get; set; }
    public float BaseRange { get; set; }
    public float BaseAS { get; set; } // 초당 공격
    public float BaseAOE { get; set; }
    public int BaseAP { get; set; }

    //타겟 관련
    public Transform AttackTarget;
    private int _searchCooldown = 15;
    private int _searchTimer;
    private static int searchAssign = 0;
    public bool AttackGround = false;


    //방어력
    public int BaseArmor { get; set; }


    //타입
    public Faction OwnedFaction = Faction.Enemy;        //소유주. 유닛스탯에서 플레이어 init 할때 자동으로 아군으로 바꿔줌
    public Faction TargetFaction;                       //공격타겟
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
        OwnedFaction = Faction.Player;
        HealthBarColor(Color.green);

    }

    #endregion
    private void Start()
    {
        _unitstats = GetComponent<UnitStats>();
        _healthCurrent = HealthMax;
        healthBar.maxValue = HealthMax;
        HealthBarUpdate();
        //모든 유닛이 같은 프레임에 대상을 탐지하는것을 방지
        _searchTimer = searchAssign++ % _searchCooldown;
        searchAssign %= _searchCooldown;

        ActionStat = ActionStats.Idle;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //if (_unitstats._isMoving)
        //{
        //    ActionStat = ActionStats.Move;
        //}
    }

    private void FixedUpdate()
    {

        switch (ActionStat)
        {
            case ActionStats.Idle:
                {
                    SearchShell();
                    if (AttackTarget != null)
                    {
                        ActionStat = ActionStats.Attack;
                    }
                    break;
                }
            case ActionStats.Move:
                {
                    if(AttackGround)
                    {
                        SearchShell();
                        if(AttackTarget!= null)
                        {//대상을 찾은 경우
                            MoveIntoRange();
                        }
                    }
                    if (!_unitstats._isMoving)
                    {
                        ActionStat = ActionStats.Idle;
                    }
                    break;
                }
            case ActionStats.Attack:
                {
                    if (AttackTarget != null && !AttackTarget.GetComponent<UnitCombat>().IsDead)
                    {
                        if (attackTimer > 0)
                        {
                            attackTimer -= Time.deltaTime;
                        }
                        else
                        {
                            if ((AttackTarget.position - transform.position).magnitude <= resultRange)
                            {//적이 사정거리 내에 있을경우
                                Attack();
                            }
                            else
                            {//적이 사정거리 내에 없을경우 타겟쪽으로 이동함
                                MoveIntoRange();
                            }

                        }
                    }
                    else
                    {//타겟이 없거나 비활성화 되어있으면 바로 타겟 비우고 대기상태로 변환
                        AttackTarget = null;
                        ActionStat = ActionStats.Idle;
                    }
                }
                break;                
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
        if (_animator != null)
        {
            _animator.SetTrigger("Regular");
        }
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
        resultDamage = BaseDamage + Weapons.DB[weaponIndex].damage;
        resultAOE = BaseAOE + Weapons.DB[weaponIndex].AttackArea;
        resultRange = BaseRange + Weapons.DB[weaponIndex].AttackRange;
        resultSpeed = BaseAS+ Weapons.DB[weaponIndex].AttackSpeed;
        resultArmor = BaseArmor + Weapons.DB[weaponIndex].Armor;
        
    }
    #endregion

    #region 공격관련
    public void Fire()
    {
        _effect = Global.ResourceManager.LoadPrefab(effectPrefab.name);
        _effect.transform.position = transform.position;
        _effect.GetComponent<AttackEffect>().Setup(this, AttackTarget.position, effectPrefab.name,attackTorque);

    }

    public void Attack()
    {
        UpdatePlaybackSpeed();
        _animator.SetTrigger("Attack");        
        ResetAttackTimer();
    }

    private void ResetAttackTimer()
    {
        attackTimer = 1 / resultSpeed;
    }

    public void UpdatePlaybackSpeed()
    {
        _animator.speed = Mathf.Max(resultSpeed, 1f);
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
                if (selectedCombat.OwnedFaction != OwnedFaction)
                {
                    AttackTarget = selectedCombat.transform;
                    ActionStat = ActionStats.Attack;
                    return;
                }
            }
        }

    }

    private void SearchShell()
    {
        if (_searchTimer <= 0)
        {
            ResetSearchTimer();//계속 돌려서 프레임당 최대한 적은 수의 탐색이 돌도록 함

            //if (AttackTarget != null && !AttackTarget.gameObject.activeSelf)
            //{//
            //    AttackTarget = null;
            //}
            if (AttackTarget == null)
            {
                Search();
            }
        }
        else
        {
            _searchTimer--;
        }
    }

    public void MoveIntoRange()
    {
        _unitstats.MoveToTarget(Vector2.MoveTowards(AttackTarget.position, transform.position, resultRange));
    }

    private void ResetSearchTimer()
    {
        _searchTimer = _searchCooldown;
    }

    private float OffSetToTarget()
    {
        return (AttackTarget.position - transform.position).magnitude;
    }

    #endregion

    #region 체력관련
    /// <summary>
    /// 방어무시 공격
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakeDamage(int damageAmount)
    {
        _healthCurrent -= (damageAmount);
        HealthBarUpdate();
    }
    /// <summary>
    /// 방어관통 있는 버젼
    /// </summary>
    /// <param name="dmg"></param>
    /// <param name="armorPierce"></param>
    public void TakeDamage(int dmg, int armorPierce)
    {
        _healthCurrent -= (dmg - Mathf.Clamp(resultArmor - armorPierce, 0, 999));
        HealthBarUpdate();
    }

    private void HealthBarUpdate()
    {
        healthBar.value = _healthCurrent;
        if(_healthCurrent<= 0)
        {
            Death();
        }

    }

    private void HealthBarColor(Color newColor)
    {
       healthBar.transform.GetChild(0).GetComponent<Image>().color = newColor;

    }

    private void Death()
    {
        _animator.SetTrigger("Die");
        IsDead = true;
        _unitstats._isMoving = false;
        _unitstats.SetSelectionCircleState(false);
        _unitstats.Selectable = false;
        IngameManager.UnitManager.DeselectUnit(gameObject);
        StartCoroutine(DeathDelay());
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    #endregion
}