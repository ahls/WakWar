﻿using System.Collections;
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
    public static bool AIenabled = false;
    public Transform AttackTarget;
    public float SearchRange = 0;
    public bool AttackGround { get; set; } = false;
    public bool SeekTarget = false; //현재 공격대상이 없으면 왁굳을 향해 공격하러 오는 유닛들은 true
    public WeaponType PreferredTarget; //선호하는 공격클래스. 
    private int _searchCooldown = 25;
    private int _searchTimer;
    private static int _searchAssign = 0;
    private float _AIsearchTimer = 0;

    //방어력
    public int BaseArmor { get; set; }


    //타입
    public Faction OwnedFaction = Faction.Enemy;        //소유주. 유닛스탯에서 플레이어 init 할때 자동으로 아군으로 바꿔줌
    public Faction TargetFaction;                       //공격타겟
    public WeaponType weaponType = WeaponType.Null;
    private int _weaponIndex = 0;
    private GameObject _effect;
    public Sprite AttackImage { get; set; }
    public float AttackTorque { get; set; } = 0;
    private UnitStats _unitstats;
    [SerializeField] private SpriteRenderer _equippedImage;
    private Animator _animator;
    private float _heightDelta;
    private int _torque;
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
                                _unitstats.StopMoving();
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
                            _unitstats.StopMoving();
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
        UpdateStats();
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
            _heightDelta = 0;
            _torque = 0;
        }
        else
        {
            TotalDamage = BaseDamage + Weapons.DB[_weaponIndex].damage;
            TotalAOE = BaseAOE + Weapons.DB[_weaponIndex].AttackArea;
            TotalRange = BaseRange + Weapons.DB[_weaponIndex].AttackRange;
            _totalAS = BaseAS + Weapons.DB[_weaponIndex].AttackSpeed;
            _totalArmor = BaseArmor + Weapons.DB[_weaponIndex].Armor;
            ProjectileSpeed = Weapons.DB[_weaponIndex].projSpeed;
            _heightDelta = Weapons.DB[_weaponIndex].heightDelta;
            _torque = Weapons.DB[_weaponIndex].torque;
            TargetFaction = Weapons.DB[_weaponIndex].targetFaction;
        }
    }
    #endregion

    #region 공격관련
    public void Fire()
    {
        if (AttackTarget == null) return; // 카이팅 안되게 막는 함수
        _effect = Global.ResourceManager.LoadPrefab(effectPrefab.name);
        _effect.transform.position = transform.position;
        _effect.GetComponent<AttackEffect>().Setup(this, AttackTarget.position, effectPrefab.name,_torque,_heightDelta);

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
    /// <summary>
    /// 현재 각 서치마다 이터레이션을 두번 돌립니다. 범위 내에 유닛 찾기, 그리고 그 유닛 내에서 가장 가까운 적 찾기.
    /// 혹시 너무 무겁다면 탐색범위를 줄이고 빈도를 낮추는 방식으로 가야할 것 같습니다.
    /// </summary>
    private void Search()
    {
        if(!AIenabled)
        {//AI 켜져있나 확인
            AttackTarget = null;
            return;
        }

        Transform BestTarget = null;
        List<Transform> listInRange = new List<Transform>();

        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, TotalRange + SearchRange);
        if (PreferredTarget != WeaponType.Null)
        {//선호대상이 있는경우
            List<Transform> preferredList = new List<Transform>();

            foreach (Collider2D selected in inRange)
            {
                UnitCombat selectedCombat = selected.GetComponent<UnitCombat>();
                if (selectedCombat != null && selectedCombat != this)
                {
                    if (selectedCombat.OwnedFaction == TargetFaction)
                    { 
                        if(selectedCombat.weaponType == PreferredTarget)
                        {
                            preferredList.Add(selected.transform);
                        }
                        else
                        {
                            listInRange.Add(selected.transform);
                        }


                    }
                }
            }
            BestTarget = ReturnClosestUnit(preferredList);
            if(BestTarget == null)
            {
            BestTarget = ReturnClosestUnit(listInRange);
            }

        }
        else
        {//선호대상이 없는경우
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
        }


        if (BestTarget != null)
        { AttackTarget = BestTarget;
            ActionStat = ActionStats.Attack;
        }

    }
    private Transform ReturnClosestUnit(List<Transform> inputList)
    {
        Transform currentBestTarget = null;
        float closestDistance = float.PositiveInfinity;
        foreach (var currentUnit in inputList)
        {
            float currentDistance = (currentUnit.position - transform.position).magnitude;

            if (currentDistance< closestDistance )
            {
                closestDistance = currentDistance;
                currentBestTarget = currentUnit;
            }
        }

        return currentBestTarget;
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
        Vector2 unitBoundLoc = GetComponent<Collider2D>().ClosestPoint(AttackTarget.position);
        return (targetBoundLoc - unitBoundLoc).magnitude;
    }

    private void EnemySearchAI()
    {
        if (!AIenabled) return;
        if(SeekTarget && AttackTarget == null && _AIsearchTimer < Time.time)
        {
            _unitstats.MoveToTarget(IngameManager.WakgoodBehaviour.transform.position);
            AttackGround = true;
            _AIsearchTimer = Time.time + 5f;
        }
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
        if (dmg > 0)
        {
            _healthCurrent -= (dmg - Mathf.Clamp(_totalArmor - armorPierce, 0, 999));
        }
        else if(dmg< 0)
        {
            _healthCurrent -= Mathf.Min(dmg + _healthCurrent, HealthMax);
        }
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
        _unitstats.StopMoving();
        _animator.SetTrigger("Die");
        IsDead = true;
        GetComponent<Rigidbody2D>().simulated = false;
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