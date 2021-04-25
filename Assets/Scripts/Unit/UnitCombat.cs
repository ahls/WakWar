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

    public ActionStats _actionStat;

    public ActionStats ActionStat
    {
        get
        {
            return _actionStat;
        }
        set
        {
            //ResetAttackTimer();
            //ResetSearchTimer();

            _actionStat = value;
        }
    }


    public GameObject effectPrefab;

    //체력
    public int HealthMax { get; set; } = 10;
    public bool IsDead { get; set; } = false;
    private int _healthCurrent;
    [SerializeField] private Slider _healthBar;

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
    private static int _searchAssign = 0;
    public bool AttackGround = false;


    //방어력
    public int BaseArmor { get; set; }


    //타입
    public Faction OwnedFaction = Faction.Enemy;        //소유주. 유닛스탯에서 플레이어 init 할때 자동으로 아군으로 바꿔줌
    public Faction TargetFaction;                       //공격타겟
    public WeaponType weaponType;
    private int _weaponIndex = 0;
    private GameObject _effect;
    public Sprite AttackImage { get; set; }
    public float AttackTorque { get; set; } = 0;
    private UnitStats _unitstats;
    [SerializeField] private SpriteRenderer _equippedImage;
    private Animator _animator;
    //장비 장착후 스탯
    public int TotalDamage { get; set; }
    public float TotalRange { get; set; }
    public float TotalAOE { get; set; }
    public int TotalAP { get; set; }
    public float ProjectileSpeed { get; set; }

    private float _totalAS= 2;
    private float _attackTimer = 0; // 0일때 공격 가능
    private int _totalArmor;

    
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
        _healthBar.maxValue = HealthMax;
        HealthBarUpdate();

        //모든 유닛이 같은 프레임에 대상을 탐지하는것을 방지
        _searchTimer = _searchAssign++ % _searchCooldown;
        _searchAssign %= _searchCooldown;

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
        if (!IsDead)
        {
            switch (ActionStat)
            {

                case ActionStats.Move:
                    {
                        if (AttackTarget != null)
                        {
                            if (OffSetToTargetBound() <= TotalRange)
                            {//적이 사정거리 내에 들어온경우 공격
                                _unitstats._isMoving = false;
                                ActionStat = ActionStats.Attack;
                            }
                            else
                            {
                                _unitstats.SetMoveToTarget(AttackTarget.position);
                                //MoveIntoRange();
                            }
                        }
                        else
                        {
                            if (AttackGround)
                            {
                                SearchShell();
                                if (AttackTarget != null)
                                {//대상을 찾은 경우
                                    MoveIntoRange();
                                }
                            }
                            if (!_unitstats._isMoving)
                            {
                                ActionStat = ActionStats.Idle;
                            }
                        }
                        break;
                    }

                case ActionStats.Idle:
                    {
                        if (AttackTarget != null)
                        {
                            ActionStat = ActionStats.Attack;
                        }
                        else
                        {
                            SearchShell();
                        }

                        break;
                    }
                default: break;
            }
            _attackTimer -= Time.deltaTime;
            if (ActionStat == ActionStats.Attack)
            {
                if (AttackTarget != null && !AttackTarget.GetComponent<UnitCombat>().IsDead)
                {
                    if (_attackTimer <= 0)
                    {
                        if (OffSetToTargetBound() <= TotalRange)
                        {//적이 사정거리 내에 있을경우
                            _animator.SetBool("Move", false);
                            _unitstats._isMoving = false;
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
        }
    }

    #region 장비관련

    public void EquipWeapon(int weaponID)
    {       
        _weaponIndex = weaponID;
        _equippedImage.sprite = Global.ResourceManager.LoadTexture(Weapons.DB[_weaponIndex].equipImage);
        
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
        UpdateStats();
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
                _weaponIndex = 10;
                break;
            case WeaponType.Shooter:
                _weaponIndex = 20;
                break;
            case WeaponType.Supporter:
                _weaponIndex = 30;
                break;
        }
    }
    public void UpdateStats()
    {
        if (_weaponIndex == 0)
        {
            TotalDamage = BaseDamage;
            TotalAOE = BaseAOE;
            TotalRange = BaseRange;
            _totalAS = BaseAS;
            _totalArmor = BaseArmor;
            ProjectileSpeed = 1;

        }
        else
        {
            TotalDamage = BaseDamage + Weapons.DB[_weaponIndex].damage;
            TotalAOE = BaseAOE + Weapons.DB[_weaponIndex].AttackArea;
            TotalRange = BaseRange + Weapons.DB[_weaponIndex].AttackRange;
            _totalAS = BaseAS + Weapons.DB[_weaponIndex].AttackSpeed;
            _totalArmor = BaseArmor + Weapons.DB[_weaponIndex].Armor;
            ProjectileSpeed = Weapons.DB[_weaponIndex].projSpeed;
        }
    }
    #endregion

    #region 공격관련
    public void Fire()
    {
        if (AttackTarget == null) return; // 카이팅 안되게 막는 함수
        _effect = Global.ResourceManager.LoadPrefab(effectPrefab.name);
        _effect.transform.position = transform.position;
        _effect.GetComponent<AttackEffect>().Setup(this, AttackTarget.position, effectPrefab.name,AttackTorque);

    }

    public void Attack()
    {
        ResetAttackTimer();

        UpdatePlaybackSpeed();
        _animator.SetTrigger("Attack");
        _unitstats.RotateDirection( AttackTarget.transform.position.x - transform.position.x);
    }

    private void ResetAttackTimer()
    {
        _attackTimer = 1 / _totalAS;
    }

    public void UpdatePlaybackSpeed()
    {
        _animator.speed = Mathf.Max(_totalAS, 1f);
    }
    #endregion

    #region 탐색 관련

    private void Search()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, TotalRange + 0.3f);
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
        _unitstats.MoveToTarget(Vector2.MoveTowards(AttackTarget.position, transform.position, TotalRange),false);
    }

    private void ResetSearchTimer()
    {
        _searchTimer = _searchCooldown;
    }

    private float OffSetToTargetBound()
    {
        Vector2 targetBoundLoc = AttackTarget.GetComponent<Collider2D>().ClosestPoint(transform.position);
        return (targetBoundLoc - (Vector2)transform.position).magnitude;
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
        _healthCurrent -= (dmg - Mathf.Clamp(_totalArmor - armorPierce, 0, 999));
        HealthBarUpdate();
    }

    private void HealthBarUpdate()
    {
        _healthBar.value = _healthCurrent;
        if(_healthCurrent<= 0)
        {
            Death();
        }

    }

    private void HealthBarColor(Color newColor)
    {
       _healthBar.transform.GetChild(0).GetComponent<Image>().color = newColor;

    }

    private void Death()
    {
        _animator.SetTrigger("Die");
        IsDead = true;
        GetComponent<Rigidbody2D>().simulated = false;
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