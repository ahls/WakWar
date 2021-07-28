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
        Attack,
        Stun
    }
    public bool debugAction = false;
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
            //ResetAttackTimer();
            //ResetSearchTimer();

            _actionStat = value;
        }
    }


    //체력
    public int HealthMax { get; set; } = 10;
    private float _healthInversed;
    public bool IsDead { get; set; } = false;
    public bool CanBeKilled { get; set; } = true;
    private int _healthCurrent;
    [SerializeField] private Slider _healthBar;
    private int _stunTimer = 0;
    private string _deathSound = "";

    //공격관련
    public int BaseDamage { get; set; }
    public float BaseRange { get; set; }
    public float BaseAS { get; set; } // 초당 공격
    public float BaseAOE { get; set; }
    public int BaseAP { get; set; }
    private string _attackAudio = "null";
    private string _impactAudio = "null";

    public float CritChance { get; set; } = 0f;
    public float CritDmg { get; set; } = 1.5f;
    public float LifeSteal { get; set; } = 0f;


    //타겟 관련
    public static bool AIenabled = false;
    [HideInInspector]public Transform AttackTarget;
    public bool AttackGround { get; set; } = false;
    [HideInInspector]public bool SeekTarget = false; //현재 공격대상이 없으면 왁굳을 향해 공격하러 오는 유닛들은 true
    private int _searchCooldown = 25;
    private int _searchTimer;
    private static int _searchAssign = 0;

    //방어력
    public int BaseArmor { get; set; }


    //타입
    public Faction OwnedFaction = Faction.Enemy;        //소유주. 유닛스탯에서 플레이어 init 할때 자동으로 아군으로 바꿔줌
    public Faction TargetFaction;                       //공격타겟
    [HideInInspector]public ClassType UnitClassType = ClassType.Null;       //클래스 타입
    private int _weaponIndex = 0;                       //무기 번호
    private GameObject _effect;
    public Sprite AttackImage { get; set; }
    public float AttackTorque { get; set; } = 0;
    private UnitStats _unitstats;
    [SerializeField]private SpriteRenderer _equippedImage;
    private Animator _animator;
    private float _heightDelta;
    private int _torque;

    //장비 장착후 스탯
    public int TotalDamage { get; set; }
    public float TotalRange { get; set; }
    public float TotalAOE { get; set; }
    public int TotalAP { get; set; }
    public float ProjectileSpeed { get; set; }

    public float TotalAS { get; set; }
    private float _attackTimer = 0; // 0일때 공격 가능
    public int TotalArmor { get; set; }


    //스킬관련
    [HideInInspector]public SkillBase Skill;

    public EnemyBehaviour EnemyBehavour = null; //적이 아니라면 null. 있을경우엔 피격시 어그로 레벨 상승

    public void playerSetup(ClassType inputWeaponType)
    {
        UnitClassType = inputWeaponType;
        OwnedFaction = Faction.Player;
        HealthBarColor(Color.green);
        _deathSound = "panzeeDeath0";
    }
    public void EnemySetup(int HP,int armor, int armorPiercing, int damage, float range,float attackSpeed,Sprite projImage, string deathSound,string projSound, string impactSound )
    {
        HealthMax = HP;
        TotalArmor = armor;
        TotalAP = armorPiercing;
        TotalDamage = damage;
        TotalAS = attackSpeed;
        TotalRange = range;
        ProjectileSpeed = 0.5f;

        AttackImage = projImage;
        _deathSound = deathSound;
        _attackAudio = projSound;
        _impactAudio = impactSound;
        
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
                            if (OffsetToTargetBound() <= TotalRange)
                            {//적이 사정거리 내에 들어온경우 공격
                                _unitstats.StopMoving();
                                ActionStat = ActionStats.Attack;
                            }
                            else
                            {
                                _unitstats.MoveToTarget(AttackTarget.position);
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
                            if (!_unitstats.IsMoving)
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
                case ActionStats.Stun:
                    {
                        if (_stunTimer > 0)
                        {
                            _stunTimer--;
                            return;
                        }
                        else
                        {
                            ActionStat = ActionStats.Idle;
                        }
                        break;
                    }
                default: break;
            }
            _attackTimer -= Time.deltaTime;
            if (ActionStat == ActionStats.Attack)
            {
                if (AttackTarget != null && !AttackTarget.GetComponent<UnitCombat>().IsDead)
                {//살아있는 공격대상이 있을경우
                    if (debugAction) Debug.Log($"공격쿨탐 남은시간: {_attackTimer}");
                    if (_attackTimer <= 0)
                    {//공격 쿨탐이 된경우
                        if (debugAction) Debug.Log("공격준비 완료");
                        if (OffsetToTargetBound() <= TotalRange)
                        {//적이 사정거리 내에 있을경우
                            if (debugAction) Debug.Log($"사정거리: {TotalRange} || 대상과의 거리: {OffsetToTargetBound()}");
                            _unitstats.StopMoving();
                            Attack();
                        }
                        else
                        {//적이 사정거리 내에 없을경우 타겟쪽으로 이동함
                            if (debugAction) Debug.Log("적에게 이동중");
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
        if (Weapons.DB[_weaponIndex].projImage != "null")
        {
            AttackImage = Global.ResourceManager.LoadTexture(Weapons.DB[_weaponIndex].projImage);
        }
        else
        {
            _equippedImage.sprite = null;
        }
        if (Weapons.DB[_weaponIndex].equipImage != "null")
        {
            _equippedImage.sprite = Global.ResourceManager.LoadTexture(Weapons.DB[_weaponIndex].equipImage);
        }
        else
        {
            _equippedImage.sprite = null;
        }
        ChangeEquipAnimation();

        ChangeSkill();

        //공격 사운드
        _attackAudio = Weapons.DB[_weaponIndex].projSound;
        _impactAudio = Weapons.DB[_weaponIndex].impctSound;
        UpdateStats();
    }

    /// <summary>
    /// 장비 애니메이션 변경
    /// </summary>
    public void ChangeEquipAnimation()
    {
        switch (GetWeaponType())
        {
            case WeaponType.Shield:
                _animator.SetTrigger("Shield");
                break;
            case WeaponType.Bow:
                _animator.SetTrigger("Bow");
                break;
            case WeaponType.Gun:
                _animator.SetTrigger("Gun");
                break;
            case WeaponType.Instrument:
                _animator.SetTrigger("Inst");
                break;
            default:
                _animator.SetTrigger("Regular");
                break;
        }
    }
    public void UnEquipWeapon()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Regular");
        }
        _equippedImage.sprite = null;
        switch (UnitClassType)
        {
            case ClassType.Warrior:
            case ClassType.Wak:
                _weaponIndex = 10;
                break;
            case ClassType.Shooter:
                _weaponIndex = 20;
                break;
            case ClassType.Supporter:
                _weaponIndex = 30;
                break;
        }
        UpdateStats();
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
        else
        {
            TotalDamage = BaseDamage + Weapons.DB[_weaponIndex].damage;
            TotalAOE = BaseAOE + Weapons.DB[_weaponIndex].AttackArea;
            TotalRange = BaseRange + Weapons.DB[_weaponIndex].AttackRange;
            TotalAS = BaseAS + Weapons.DB[_weaponIndex].AttackSpeed;
            TotalArmor = BaseArmor + Weapons.DB[_weaponIndex].Armor;
            ProjectileSpeed = Weapons.DB[_weaponIndex].projSpeed;
            _heightDelta = Weapons.DB[_weaponIndex].heightDelta;
            _torque = Weapons.DB[_weaponIndex].torque;
            TargetFaction = Weapons.DB[_weaponIndex].targetFaction;
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
        Debug.Log($"Weapon Index: {_weaponIndex},");
        Debug.Log($"Weapon name: {Weapons.DB[_weaponIndex].name}");
        return Weapons.DB[_weaponIndex].weaponType;
    }
    #endregion

    #region 공격관련
    public void Fire()
    {
        if (AttackTarget == null) return; // 카이팅 안되게 막는 함수
        _effect = Global.ObjectManager.SpawnObject(Weapons.attackPrefab);
        _effect.transform.position = transform.position;

        AttackEffect attackEffectScript = _effect.GetComponent<AttackEffect>();
        if (_weaponIndex * 0.1 == 10000) // 무기가 검인경우 공격데미지 수정
        {
            attackEffectScript.Setup(this, CalculateBerserkDamage(), AttackTarget.position);
        }
        else
        {
            attackEffectScript.Setup(this, TotalDamage, AttackTarget.position);
        }



        if (_torque > 0 || _heightDelta > 0)
        {
            attackEffectScript.AddTrajectory(_torque, _heightDelta);
        }
        if (LifeSteal > 0 || CritChance > 0)
        {
            attackEffectScript.AddHitEffect(CritChance, CritDmg, LifeSteal);
        }

        if (_attackAudio != "null" )
        {
            Global.AudioManager.PlayOnceAt(_attackAudio, transform.position, true);
        }
    }

    public void Attack()
    {
        ResetAttackTimer();

        UpdatePlaybackSpeed();
        _animator.SetTrigger("Attack");
        _unitstats.RotateDirection(AttackTarget.transform.position.x - transform.position.x);
    }

    private void ResetAttackTimer()
    {
        _attackTimer = 1 / TotalAS;
    }

    public void UpdatePlaybackSpeed()
    {
        _animator.speed = Mathf.Max(TotalAS, 1f);
    }
    public void AddStun(int numFrames)
    {
        _unitstats.StopMoving();
        ActionStat = ActionStats.Stun;
        _stunTimer += numFrames;
    }
    public string GetImpactSound()
    {
        return _impactAudio;
    }
    #endregion

    #region 탐색 관련
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
            UnitCombat selectedCombat = selected.GetComponent<UnitCombat>();
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
            ActionStat = ActionStats.Attack;
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
        _searchTimer = _searchCooldown;
    }

    public float OffsetToTargetBound()
    {
        Vector2 targetBoundLoc = AttackTarget.GetComponent<Collider2D>().ClosestPoint(transform.position);
        Vector2 unitBoundLoc = GetComponent<Collider2D>().ClosestPoint(AttackTarget.position);
        return (targetBoundLoc - unitBoundLoc).magnitude;
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
        if(EnemyBehavour != null)        EnemyBehavour.AggroChange(1024); //적이라면 일정시간동안 어그로수준 추가

        if (_healthCurrent <= 0)
        {
            if (CanBeKilled == false)
            {
                _healthCurrent = 1;
            }
            else
            {
                Death();
            }
        }
        HealthBarUpdate();
    }
    /// <summary>
    /// 방어관통 있는 버젼
    /// </summary>
    /// <param name="dmg"></param>
    /// <param name="armorPierce"></param>
    public void TakeDamage(int dmg, int armorPierce)
    {
        if (dmg > 0)
        {
            TakeDamage(dmg - Mathf.Clamp(TotalArmor - armorPierce, 0, (dmg-1)));
        }
        else if (dmg < 0)
        {
            Heal(-dmg);
        }

    }
    public void Heal(int amount, string effect = "HealEffect")
    {
        _healthCurrent = Mathf.Min(_healthCurrent + amount, HealthMax);
        GameObject healEffect = Global.ObjectManager.SpawnObject(effect);
        healEffect.transform.position = transform.position;
    }

    private void HealthBarUpdate()
    {
        _healthBar.value = _healthCurrent;
    }

    private void HealthBarColor(Color newColor)
    {
        _healthBar.transform.GetChild(0).GetComponent<Image>().color = newColor;

    }

    private void Death()
    {
        _unitstats.StopMoving();
        _animator.SetTrigger("Die");
        IsDead = true;
        GetComponent<Rigidbody2D>().simulated = false;
        Global.AudioManager.PlayOnce(_deathSound, true);
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
        GameObject skillObject = Instantiate(Global.ObjectManager.SpawnObject("skillcarrier"), transform);
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
                break;
            default:
                break;
        }
    }
    public void useSkill()
    {
        Skill.UseSkill(this);
    }
    private int CalculateBerserkDamage()
    {

        float maxDmgBonus = 1 + (GetItemRank() + 1) * 0.5f; // 50%, 100% 150% 200% 퍼센트의 추뎀
        int additionalDmg = Mathf.FloorToInt(maxDmgBonus * (HealthMax - _healthCurrent) * _healthInversed);

        return additionalDmg;

    }
    #endregion
}