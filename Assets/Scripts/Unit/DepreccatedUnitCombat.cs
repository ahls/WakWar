using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Faction { Player, Enemy, Both }//유닛 컴뱃에 부여해서 피아식별
/*
public class DepreccatedUnitCombat : MonoBehaviour
{
    public enum UnitState
    {
        Idle,
        Move,
        Attack,
        Stun,
        Chase

    }
    public bool debugAction = false;
    #region 변수

    private UnitState _actionStat;
    public UnitState ActionStat
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


    ////체력
    //public int BaseHP = 10;
    //public int HealthMax { get; set; } 
    //private float _healthInversed;
    //public bool IsDead { get; set; } = false;
    //public bool CanBeKilled { get; set; } = true;
    //private int _healthCurrent;
    //[SerializeField] private Slider _healthBar;
    //private int _stunTimer = 0;


    ////방어력
    //public int BaseArmor { get; set; }

    ////공격관련
    //private const float RANDOM_ATTACK_DELAY_RANGE = 0.1f;
    //public int BaseDamage { get; set; }
    //public float BaseRange { get; set; }
    //public float BaseAS { get; set; } // 초당 공격
    //public float BaseAOE { get; set; }
    //public int BaseAP { get; set; }

    //public float BaseCrit = 0f;
    //public float BaseLD = 0f;//life drain
    //public float CritChance { get; set; } = 0f;
    //public float CritDmg { get; set; } = 1.5f;
    //public float LifeSteal { get; set; } = 0f;


    //타겟 관련
    private byte _actionTimer;
    private bool _attackGround = false; //어택땅
    private bool _chasing = false;
    public static bool AIenabled = false;
    public bool HoldPosition = false;
    [HideInInspector]public Transform AttackTarget;
    [HideInInspector]public bool SeekTarget = false; //현재 공격대상이 없으면 왁굳을 향해 공격하러 오는 유닛들은 true
    private const int TIMER_COOLDOWN = 5;
    private static int _searchAssign = 0;
    private int _searchTimer;
    public float SearchRadius = 1;
    public float MaxStrayDistance = 1; // 0일경우 계속 따라감.s
    private Vector3 _targetPosition;

   


    //타입
    public Faction OwnedFaction = Faction.Enemy;        //소유주. 유닛스탯에서 플레이어 init 할때 자동으로 아군으로 바꿔줌
    public Faction TargetFaction;                       //공격타겟
    [HideInInspector]public ClassType UnitClassType = ClassType.Null;       //클래스 타입
    private int _weaponIndex = 0;                       //무기 번호
    [SerializeField]private UnitMove _unitstats;



    //이미지 관련
    [SerializeField] private Animator _animator;
    public Sprite AttackImage { get; set; }
    [SerializeField]private SpriteRenderer _equippedImage;
    
    private float _heightDelta; //음수일경우 attack effect의 SetAngle 불러옴
    private int _torque;
    private GameObject _effect;
    private static Vector2 _attackHeight = new Vector2(0, 0.1f);
    private string _impactEffect = null;
    private GameObject _instrumentEffect = null;
    public Transform Head;


    //사운드
    private string _attackAudio = string.Empty;
    private string _impactAudio = string.Empty;
    private string _deathSound = string.Empty;

    //장비 장착후 스탯
    //public int TotalDamage { get; set; }
    //public float TotalRange { get; set; }
    //public float TotalAOE { get; set; }
    //public int TotalAP { get; set; }
    //public float ProjectileSpeed { get; set; }

    //public float TotalAS { get; set; }
    //private float _attackTimer = 0; // 0일때 공격 가능
    //public int TotalArmor { get; set; }

    //스킬관련

    public EnemyBehaviour EnemyBehavour = null; //적이 아니라면 null. 있을경우엔 피격시 어그로 레벨 상승

    //이벤트

    [HideInInspector]public SkillBase Skill;
    public event UnitCombatEvent OnSkillUse;
    public event UnitCombatEvent OnEachSecondAlive; // 초당 체력회복등에 사용
    public event UnitCombatEvent OnUnequipItem;
    public delegate void UnitCombatEvent(DepreccatedUnitCombat uc);
    private Coroutine _aliveSecondCoroutine;
    private Coroutine _attackCoroutine;
    public void playerSetup(ClassType inputWeaponType)
    {
        UnitClassType = inputWeaponType;
        OwnedFaction = Faction.Player;
        HealthBarColor(Color.green);
        _deathSound = "panzeeDeath0";
    }
    public void EnemySetup(int HP,int armor, int armorPiercing, int damage, float range,float attackSpeed,Sprite projImage,int heightDelta )
    {
        TotalArmor = armor;
        TotalAP = armorPiercing;
        TotalDamage = damage;
        TotalAS = attackSpeed;
        TotalRange = range;
        ProjectileSpeed = 1.5f;
        _heightDelta = heightDelta;
        AttackImage = projImage;
    }
    public void SoundSetup(string deathSound, string projSound, string impactSound, int numVariations = 1)
    {
        _deathSound = deathSound;
        _attackAudio = projSound;
        _impactAudio = impactSound;
        _soundVariation = numVariations;
    }
    #endregion
    private void Start()
    {
        if(_unitstats == null)_unitstats = GetComponent<UnitMove>();
        HealthBarUpdate();

        //모든 유닛이 같은 프레임에 대상을 탐지하는것을 방지
        _searchTimer = _searchAssign++ % TIMER_COOLDOWN;
        _actionTimer = (byte)(_searchAssign++ % TIMER_COOLDOWN);
        _searchAssign %= TIMER_COOLDOWN;

        ActionStat = UnitState.Idle;
        StartCoroutine(EachSecondAliveCoroutine());
        if(_animator == null)        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //if (_unitstats._isMoving)
        //{
        //    ActionStat = ActionStats.Move;
        //}
    }
    private void Action()
    {
        float biggerSearchRadius = SearchRadius + TotalRange;
        switch (ActionStat)
        {
            case UnitState.Idle:
                if(HoldPosition)
                {
                    if (SearchInRadius(TotalRange))
                    {
                        ActionStat = UnitState.Attack;
                    }
                }    
                else
                {
                    if (SearchInRadius(biggerSearchRadius))
                    {
                        ActionStat = UnitState.Attack;
                    }
                }
                break;
            case UnitState.Move:
                if(AttackTarget!= null && 
                   !AttackTarget.GetComponent<DepreccatedUnitCombat>().IsDead &&
                   OffsetToTargetBound() <= TotalRange)
                {
                    _unitstats.StopMoving();
                    ActionStat = UnitState.Attack;
                }
                else if(_attackGround)
                {
                    if (SearchInRadius(biggerSearchRadius))
                    {
                        ActionStat = UnitState.Attack;
                    }
                }
                break;
            case UnitState.Attack:
                if (AttackTarget == null || AttackTarget.GetComponent<DepreccatedUnitCombat>().IsDead)
                {
                    //공격스테이트에서 적이 없음
                    if(SearchInRadius(biggerSearchRadius))
                    {
                        _unitstats.MoveToTarget(_targetPosition,false);
                    }
                }

                break;
            case UnitState.Stun:
                if(_stunTimer > 0)
                {
                    _stunTimer-= TIMER_COOLDOWN;
                    return;
                }
                ActionStat = UnitState.Idle;
                break;
            case UnitState.Chase:
                if (SearchInRadius(TotalRange))
                {
                    ActionStat = UnitState.Attack;
                }
                if((transform.position - _targetPosition).magnitude > MaxStrayDistance && MaxStrayDistance != 0)
                {
                    _unitstats.MoveToTarget(_targetPosition);
                }
                else
                {
                    MoveIntoRange();
                    ActionStat = UnitState.Chase;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 프레임당 돌리게 됨
    /// </summary>
    private void OnAttackTimerExpire()
    {
        _attackTimer -= Time.fixedDeltaTime;
        if (_attackTimer > 0) return;
        if (ActionStat == UnitState.Attack)
        {
            if (AttackTarget != null && !AttackTarget.GetComponent<DepreccatedUnitCombat>().IsDead)
            {//살아있는 공격대상이 있을경우

                if (debugAction) Debug.Log($"사정거리: {TotalRange} || 대상과의 거리: {OffsetToTargetBound()}");
                if (OffsetToTargetBound() <= TotalRange)
                {//적이 사정거리 내에 있을경우

                    _unitstats.StopMoving();
                    Attack();
                }
                else
                {
                    if(HoldPosition)
                    {
                        //위치사수 상태였을 시 그대로 대기
                        ActionStat = UnitState.Idle;
                    }
                    else if(!_chasing)
                    {
                        //적이 사정거리 내에 없을경우 추격 시작
                        _chasing = true;
                        if(!_attackGround)
                        {
                            _targetPosition = transform.position;
                        }
                    }
                    ActionStat = UnitState.Chase;
                }


            }
        }
    }
    private void FixedUpdate()
    {
        if (!IsDead)
        {

            _actionTimer--;
            if (_actionTimer <= 0)
            {
                //Debug.Log(ActionStat.ToString());
                _actionTimer = TIMER_COOLDOWN;
                Action();
            }
            OnAttackTimerExpire();
        }
    }

    IEnumerator EachSecondAliveCoroutine()
    {

        while (true)
        {
            yield return new WaitForSeconds(1);
            OnEachSecondAlive?.Invoke(this);
        }
    }

    public void UpdateStats()
    {
        if (_weaponIndex == 0)
        {
            TotalDamage = BaseDamage;
            TotalAOE = BaseAOE;
            TotalRange = BaseRange;
            TotalAS = BaseAS;
            TotalArmor = BaseArmor;
            ProjectileSpeed = 1;
            _heightDelta = 0;
            _torque = 0;
        }
        else//무기 인덱스가 0이 아니면 플레이어가 조종하는 유닛임.
        {
            Dictionary<string, float> classModifier = IngameManager.RelicManager.ClassModifiers[UnitClassType].Modifiers;
            Weapon weaponInfo = Weapons.DB[_weaponIndex];
            //+classModifier.Modifiers[""]
            TotalDamage = BaseDamage + weaponInfo.damage + (int)classModifier["Damage"];
            
            TotalRange = BaseRange + weaponInfo.AttackRange;
            TotalAP = BaseAP + weaponInfo.AP + (int)classModifier["AP"];
            TotalAS = BaseAS + weaponInfo.AttackSpeed + classModifier["AttackSpeed"];
            TotalArmor = BaseArmor + weaponInfo.Armor + (int)classModifier["Armor"];
            HealthMax = BaseHP +(int)classModifier["MaxHP"];
            CritChance =BaseCrit + classModifier["CritChance"];
            LifeSteal = BaseLD + classModifier["LifeSteal"];
            _unitstats.MoveSpeed = classModifier["MovementSpeed"];

            TotalAOE = BaseAOE + weaponInfo.AttackArea;
            ProjectileSpeed = weaponInfo.projSpeed;
            _heightDelta = weaponInfo.heightDelta;
            _torque = weaponInfo.torque;
            TargetFaction = weaponInfo.targetFaction;
        }
        _healthInversed = 1 / HealthMax;
    }
    /// <summary>
    /// 0부터 3까지의 단계
    /// </summary>
    /// <returns></returns>
    public int GetItemRank()
    {
        return _weaponIndex % 10;
    }

    public WeaponType GetWeaponType()
    {
        return Weapons.DB[_weaponIndex].weaponType;
    }

   
    public void AddStun(int numFrames)
    {
        _unitstats.StopMoving();
        ActionStat = UnitState.Stun;
        _stunTimer += numFrames;
    }

    #region 탐색 관련
    /// <summary>
    /// 주어진 거리 내에 적을 찾아보고, 적 발견시 타겟으로 지정 및 true 리턴, 실패시 false 리턴함.
    /// </summary>
    /// <param name="radius"></param>
    /// <returns></returns>
    private bool SearchInRadius(float radius)
    {
        Transform BestTarget = null;
        List<Transform> listInRange = new List<Transform>();

        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D selected in inRange)
        {
            DepreccatedUnitCombat selectedCombat = selected.GetComponent<DepreccatedUnitCombat>();
            if (selectedCombat != null && selectedCombat != this)
            {
                if (selectedCombat.OwnedFaction == TargetFaction)
                {
                    listInRange.Add(selected.transform);

                }
            }
        }
        BestTarget = ReturnClosestUnit(listInRange);

        if (BestTarget != null)
        {
            AttackTarget = BestTarget;
            ActionStat = UnitState.Attack;
            return true;
        }
        else
        {
            return false;
        }

    }
    /// <summary>
    /// 현재 각 서치마다 이터레이션을 두번 돌립니다. 범위 내에 유닛 찾기, 그리고 그 유닛 내에서 가장 가까운 적 찾기.
    /// 혹시 너무 무겁다면 탐색범위를 줄이고 빈도를 낮추는 방식으로 가야할 것 같습니다.
    /// </summary>
    private void Search()
    {
        if (!AIenabled)
        {//AI 켜져있나 확인
            AttackTarget = null;
            return;
        }

        Transform BestTarget = null;
        List<Transform> listInRange = new List<Transform>();

        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, TotalRange);

        foreach (Collider2D selected in inRange)
        {
            DepreccatedUnitCombat selectedCombat = selected.GetComponent<DepreccatedUnitCombat>();
            if (selectedCombat != null && selectedCombat != this)
            {
                if (selectedCombat.OwnedFaction == TargetFaction)
                {
                    listInRange.Add(selected.transform);

                }
            }
        }
        BestTarget = ReturnClosestUnit(listInRange);



        if (BestTarget != null)
        {
            AttackTarget = BestTarget;
            ActionStat = UnitState.Attack;
        }

    }
    public Transform ReturnClosestUnit(List<Transform> inputList)
    {
        Transform currentBestTarget = null;
        float closestDistance = float.PositiveInfinity;
        foreach (var currentUnit in inputList)
        {
            float currentDistance = (currentUnit.position - transform.position).magnitude;

            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                currentBestTarget = currentUnit;
            }
        }

        return currentBestTarget;
    }

    private void SearchShell()
    {
        if (OwnedFaction == Faction.Enemy)            return; // 적일경우 독자적인 EnemyBehvaiour으로 행동함.
        
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
        _unitstats.MoveToTarget(Vector2.MoveTowards(AttackTarget.position, transform.position, TotalRange), false);
    }

    private void ResetSearchTimer()
    {
        _searchTimer = TIMER_COOLDOWN;
    }

    public float OffsetToTargetBound()
    {
        Vector2 targetBoundLoc = AttackTarget.GetComponent<Collider2D>().ClosestPoint(transform.position);
        Vector2 unitBoundLoc = GetComponent<Collider2D>().ClosestPoint(AttackTarget.position);
        return (targetBoundLoc - unitBoundLoc).magnitude;
    }

    public void OrderAttackGround(bool state, Vector3 ?targetLoc = null)
    {
        _attackGround = state;
        if (state)
            _targetPosition = (Vector3)targetLoc;
    }

    #endregion

    #region 체력관련
    /// <summary>
    /// 방어무시 공격
    /// </summary>
    /// <param name="damageAmount"></param>
    //public void TakeDamage(int damageAmount)
    //{
    //    if (IsDead)
    //    {
    //        return;
    //    }

    //    _healthCurrent -= (damageAmount);
    //    if(EnemyBehavour != null)        EnemyBehavour.AggroChange(1024); //적이라면 일정시간동안 어그로수준 추가

    //    if (_healthCurrent <= 0)
    //    {
    //        if (CanBeKilled == false)
    //        {
    //            _healthCurrent = 1;
    //        }
    //        else
    //        {
    //            Death();
    //        }
    //    }
    //    HealthBarUpdate();
    //}
    ///// <summary>
    ///// 방어관통 있는 버젼
    ///// </summary>
    ///// <param name="dmg"></param>
    ///// <param name="armorPierce"></param>
    //public void TakeDamage(int dmg, int armorPierce)
    //{
    //    if (dmg > 0)
    //    {
    //        TakeDamage(dmg - Mathf.Clamp(TotalArmor - armorPierce, 0, (dmg-1)));
    //    }
    //    else if (dmg < 0)
    //    {
    //        Heal(-dmg);
    //    }

    //}
    //public void Heal(int amount, string effect = "HealEffect")
    //{
    //    _healthCurrent = Mathf.Min(_healthCurrent + amount, HealthMax);
    //    GameObject healEffect = Global.ObjectManager.SpawnObject(effect);
    //    healEffect.transform.position = transform.position;
    //}

    //private void HealthBarUpdate()
    //{
    //    _healthBar.value = _healthCurrent;
    //}

    //private void HealthBarColor(Color newColor)
    //{
    //    _healthBar.transform.GetChild(0).GetComponent<Image>().color = newColor;
    //}
    public void ChangeFaction(Faction toWhichFaction)
    {
        switch (toWhichFaction)
        {
            case Faction.Player:
                HealthBarColor(Color.green);
                _unitstats.Selectable = true;
                OwnedFaction = toWhichFaction;
                TargetFaction = Faction.Enemy;
                break;
            case Faction.Enemy:
                HealthBarColor(Color.red);
                _unitstats.Selectable = false;
                IngameManager.UnitManager.DeselectUnit(gameObject);
                OwnedFaction = toWhichFaction;
                TargetFaction = Faction.Player;
                break;
            default:
                break;
        }
    }
    public void Death()
    {
        _unitstats.DisableMovement();
        _animator.SetTrigger("Die");
        if (_aliveSecondCoroutine != null) StopCoroutine(_aliveSecondCoroutine);
        IsDead = true;
        GetComponent<Collider2D>().enabled = false;
        Global.AudioManager.PlayOnceAt(_deathSound,transform.position, true);
        _unitstats.SetSelectionCircleState(false);
        _unitstats.Selectable = false;
        IngameManager.UnitManager.DeselectUnit(gameObject);
        IngameManager.EnemyManager.EnemyDeath(gameObject);
        if(UnitClassType == ClassType.Wak)
        {
            Global.UIManager.GameOver();
        }
        StartCoroutine(DeathDelay());
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    #endregion

    #region 스킬관련
    public void PlaySkillAnim()
    {
        _animator.SetTrigger("Skill");
    }
    private void ChangeSkill()
    {
        if (Skill != null)
        {
            Destroy(Skill);
        }
        GameObject skillObject = Instantiate(Global.ObjectManager.SpawnObject("skillcarrier"));
        skillObject.transform.SetParent(transform);
        skillObject.transform.localPosition = Vector3.zero;
        switch (Weapons.DB[_weaponIndex].weaponType)
        {
            case WeaponType.Axe:
                Skill = skillObject.AddComponent<Bladestorm>();
                break;
            case WeaponType.Sword:
                Skill = skillObject.AddComponent<Berserk>();
                break;
            case WeaponType.Shield:
                Skill = skillObject.AddComponent<Taunt>();
                break;
            case WeaponType.Bow:
                Skill = skillObject.AddComponent<ArrowRain>();
                break;
            case WeaponType.Gun:
                Skill = skillObject.AddComponent<Snipe>();
                break;
            case WeaponType.Throw:
                Skill = skillObject.AddComponent<Rush>();
                break;
            case WeaponType.Blunt:
                Skill = skillObject.AddComponent<Stun>();
                break;
            case WeaponType.Wand:
                Skill = skillObject.AddComponent<MassHeal>();
                break;
            case WeaponType.Instrument:
                Skill = skillObject.AddComponent<Finale>();
                Skill.GetComponent<PolygonCollider2D>().enabled = true;
                break;
            default:
                break;
        }
    }
    public void useSkill()
    {
        Skill.UseSkill(this);
    }

    #endregion
}*/