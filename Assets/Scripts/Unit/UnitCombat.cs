﻿using System.Collections;
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

    //체력
    public int healthMax { get; set; } = 10;
    private int healthCurrent;
    [SerializeField] private Slider healthBar;

    //공격관련
    public int attackDamage { get; set; }
    public float attackRange { get; set; }
    public float attackSpeed { get; set; } // 초당 공격
    public float attackArea { get; set; }
    public int armorPiercing { get; set; }

    //타겟 관련
    private Transform attackTarget;
    private int searchCooldown = 15;
    private int searchTimer;
    private static int searchAssign = 0;


    //방어력
    public int armor { get; set; }

    
    //타입
    public faction ownedFaction = faction.enemy;        //소유주. 유닛스탯에서 플레이어 init 할때 자동으로 아군으로 바꿔줌
    public faction targetFaction;                       //공격타겟
    public WeaponType weaponType;
    private UnitWeapon _weapon;
    private GameObject _effect;
    public Sprite attackImage { get; set; }

    private UnitStats _unitstats;
    //장비 장착후 스탯
    public int resultDamage { get; set; }
    public float resultRange { get; set; }
    public float resultAOE { get; set; }
    public int resultAP { get; set; }
    public float projectileSpeed { get; set; }

    private float resultSpeed;
    private float attackTimer = 0; // 0일때 공격 가능
    private int resultArmor;



    #endregion
    private void Start()
    {
        _unitstats = GetComponent<UnitStats>();
        healthCurrent = healthMax;
        healthBar.maxValue = healthMax;

        //모든 유닛이 같은 프레임에 대상을 탐지하는것을 방지
        searchTimer = searchAssign++ % searchCooldown;
        searchAssign %= searchCooldown;

        ActionStat = ActionStats.Idle;
    }

    private void Update()
    {
        if (_unitstats._isMoving)
        {
            ActionStat = ActionStats.Move;
        }
<<<<<<< HEAD
        else//else문을 넣음으로 공격에 후딜이 추가됨: 공격후 적을 탐색하거나 공격을 위해 적에게 다가가지 않음.
        {
            if(_unitstats._isMoving)
            {//움직이고 있으면 아래를 돌리지 않음
                return;
            }
            if (attackTarget != null)
            {
                if (offsetToTarget() <= attackRange)
                {//적이 사정거리 내에 있을경우
                    attack();
                }
                else
                {//적이 사정거리 내에 없을경우 타겟쪽으로 이동함
                    float distanceOffset = offsetToTarget() - resultRange;//현재 거리 - 무기 사정거리
                    _unitstats.MoveToTarget(Vector3.MoveTowards(transform.position,attackTarget.position,distanceOffset));
=======

        switch (ActionStat)
        {
            case ActionStats.Idle:
                {
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
                                Attack();
                            }
                            else
                            {//적이 사정거리 내에 없을경우 타겟쪽으로 이동함

                            }
                        }
                    }

                    break;
                }
            default:
                {
                    break;
>>>>>>> 8ce87b69d49893630d3b99d22bd2f928323518c8
                }
        }
    }

    private void FixedUpdate()
    {
<<<<<<< HEAD
        searchShell();
=======
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
>>>>>>> 8ce87b69d49893630d3b99d22bd2f928323518c8
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

    public void Attack()
    {
        //투사체 pull 해주세요
        //############
        if(_effect == null)
        {
            //_effect = Instantiate()
        }
        _effect.transform.position = transform.position;
        _effect.GetComponent<AttackEffect>().setup(this, attackTarget.position);
        ResetAttackTimer();
    }
    
    private void ResetAttackTimer()
    {
        attackTimer = 1 / attackSpeed;
    }

    #endregion

    #region 탐색 관련
<<<<<<< HEAD
    private void searchShell()
    {
        if (searchTimer <= 0)
        {
            searchTimer = searchCooldown;//계속 돌려서 프레임당 최대한 적은 수의 탐색이 돌도록 함
            if (!_unitstats._isMoving && offsetToTarget() > resultRange)
            {//움직이고 있지 않으며, 현재 타겟이 사정거리 밖으로 나가면 대상 취소
                attackTarget = null;
            }
            if (attackTarget == null)
            {
                search();
            }
        }
        else
        {
            searchTimer--;
        }
    }
    private void search()
=======

    private void Search()
>>>>>>> 8ce87b69d49893630d3b99d22bd2f928323518c8
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

    public void TakeDamage(int damageAmount)
    {
        healthCurrent -= (damageAmount - resultArmor);
        HealthBarUpdate();
    }

    public void TakeDamage(int dmg, int armorPierce)
    {
        healthCurrent -= (dmg - Mathf.Clamp(resultArmor - armorPierce, 0, 999));
        HealthBarUpdate();
    }

    private void HealthBarUpdate()
    {
        healthBar.value = healthCurrent;
    }
    
    #endregion 
}


