﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public string UnitName;
    static public bool EnemyAIenabled = false;

    //대상 탐지 관련

    private ushort _searchTimer;
    static private ushort _searchIndex = 0;
    private ushort _aggroLevel = 0; //한번 전투에 돌입하면 true 로 설정하고 계속 적과 싸움.
    [SerializeField] private float SearchRange;
    private Vector2 _restingPosition;

    private const ushort AGGRO_THRESHOLD = 200;
    private const ushort SEARCH_RATE = 64;

    //유닛 스탯 설정창
    [SerializeField] private int HP,Armor, Damage, AP, heightDelta,AttackSoundVariations = 1;
    [SerializeField] private float MoveSpeed, AttackSpeed, range;
    [SerializeField] private string ProjectileImage = "", ProjectileSound = "", ImpactSound = "", DeathSound = "";
    [SerializeField] UnitCombat _unitCombat;
    [SerializeField] UnitStats _unitStats;

    // Start is called before the first frame update
    void Awake()
    {
        _unitCombat.EnemySetup(HP, Armor, AP, Damage,range,AttackSpeed, Global.ResourceManager.LoadTexture(ProjectileImage),heightDelta);
        _unitCombat.SoundSetup(DeathSound, ProjectileSound, ImpactSound, AttackSoundVariations);
        _unitStats.MoveSpeed = MoveSpeed;
        _searchTimer = _searchIndex++;
        _searchIndex &= SEARCH_RATE;
    }
    private void Start()
    {

        _restingPosition = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(_searchTimer == 0)
        {
            _searchTimer = SEARCH_RATE;
            if (_aggroLevel > 0)
            {
                //현재 타겟 거리가 공격범위 밖이거나, 타겟이 얿을경우 새 타겟을 찾음
                if(_unitCombat.AttackTarget == null || _unitCombat.OffsetToTargetBound() > range)
                {
                    Search();
                    
                    if(_unitCombat.AttackTarget == null)
                    {//새로운 대상을 못찾을 경우 우왁굳을 향해서 어택땅
                        _unitStats.MoveToTarget(IngameManager.WakgoodBehaviour.transform.position, true);
                    }
                }
                //Debug.Log($"Aggro Level: {_aggroLevel - SEARCH_RATE}");


                LookOutEnemy(false, true);
            }
            else
            {//어그로 수준이 없음
                LookOutEnemy(true,false);
                if (_unitCombat.AttackTarget != null)
                {
                    _unitStats.MoveToTarget(_restingPosition);
                    _unitCombat.AttackTarget = null;
                }
            }
        }
        else
        {
            _searchTimer--;
        }
        
    }

    private void LookOutEnemy(bool alertNearby,bool decreaseAggro)
    {
        bool enemyFound = false;
        List<EnemyBehaviour> enemiesInRange = new List<EnemyBehaviour>();//적의 아군들
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, SearchRange);
        foreach (Collider2D selected in inRange)
        {
            UnitCombat selectedCombat = selected.GetComponent<UnitCombat>();
            if (selectedCombat != null)
            {
                if(selectedCombat.OwnedFaction == Faction.Enemy)
                {
                    enemiesInRange.Add(selectedCombat.EnemyBehavour);
                }
                else if(!enemyFound)
                {
                    if(!alertNearby)
                    {//주변 적 알릴 필요가 없으면 바로 종료
                        return;
                    }
                    enemyFound = true;
                    _unitCombat.AttackTarget = selectedCombat.transform;
                    _unitCombat.ActionStat = UnitCombat.ActionStats.Attack;
                }
            }
        }
        if(enemyFound)
        {
            //적들 알릴 필요가 없으면 위쪽에서 끝났으므로, 직므은 주변에 상황을 알림
            foreach (var item in enemiesInRange)
            {
                item.AggroChange(256);
            }

        }
        else if(decreaseAggro)
        {//주변에 적 없고 위의 조건 충족하면 어그로 수준 낮춤
            if (_aggroLevel - SEARCH_RATE < 0)
            {
                //어그로 레벨이 0이 되면 원래 자리로 돌아감
                _aggroLevel = 0;
            }
            else
                _aggroLevel -= SEARCH_RATE;
        }
    }

    /// <summary>
    /// 받은 피해에 비례한 범위 내의 적들에게 전투상황 알림
    /// </summary>
    /// <param name="damage"></param>
    public void AggroChange(int amount)
    {
      if(_aggroLevel + amount > ushort.MaxValue)
        {
            _aggroLevel = ushort.MaxValue;
        }
        else
        {
            _aggroLevel += (ushort)amount;
        }
    }
    public void Search()
    {

        
        Transform BestTarget = null;
        List<Transform> listInRange = new List<Transform>();

        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, SearchRange);


            foreach (Collider2D selected in inRange)
            {
                UnitCombat selectedCombat = selected.GetComponent<UnitCombat>();
                if (selectedCombat != null && selectedCombat != this)
                {
                    if (selectedCombat.OwnedFaction == Faction.Player)
                    {
                        listInRange.Add(selected.transform);

                    }
                }
            }
            BestTarget = _unitCombat.ReturnClosestUnit(listInRange);
        


        if (BestTarget != null)
        {
            _unitCombat.AttackTarget = BestTarget;
            _unitCombat.ActionStat = UnitCombat.ActionStats.Attack;
        }

    }
}
