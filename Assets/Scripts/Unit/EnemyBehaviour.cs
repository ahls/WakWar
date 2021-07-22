using System.Collections;
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
    [SerializeField] private int HP,Armor, Damage, AP;
    [SerializeField] private float MoveSpeed, AttackSpeed, range;
    [SerializeField] private string ProjectileImage = null, ProjectileSound = null, ImpactSound = null, DeathSound = null;
    [SerializeField] UnitCombat _unitCombat;
    [SerializeField] UnitStats _unitStats;

    // Start is called before the first frame update
    void Awake()
    {
        _unitCombat.EnemySetup(HP, Armor, AP, Damage,range, Global.ResourceManager.LoadTexture(ProjectileImage), DeathSound,ProjectileSound,ImpactSound);
        _unitStats.MoveSpeed = MoveSpeed;
        _searchTimer = _searchIndex++;
        _searchIndex &= SEARCH_RATE;
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
                }
                Debug.Log($"Aggro Level: {_aggroLevel - SEARCH_RATE}");
                if (_aggroLevel - SEARCH_RATE < 0)
                {
                    //어그로 레벨이 0이 되면 원래 자리로 돌아감
                    _aggroLevel = 0;
                }
                else
                    _aggroLevel -= SEARCH_RATE;
            }
            else
            {
                if(_unitCombat.AttackTarget != null)
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
