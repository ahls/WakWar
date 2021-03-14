using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackEffect : MonoBehaviour
{
    UnitCombat attackerInfo; 
    //레퍼런스를 잡긴 했는데 범위공격같은애들이 있으면, 공격 정보를 매번 끌어와야되니까 일단 아래 데미지랑 타겟팩션같은거는 담아둘 변수를 만들어놨어요
    private int _damage;
    private int _AP; // 아머피어싱
    private Vector2 _aoe;
    private Vector3 _destination;
    private faction _targetFaction;
    private GameObject onHitEffect;
    private int lifeTime;//착탄지점에 도착했는지 여부를 검사할때 쓰일거
    [SerializeField] private SpriteRenderer projectileImage;
    [SerializeField] private Rigidbody2D RB;
    // Start is called before the first frame update

    /// <summary>
    /// 셋업 하기 전에 공격자 위치로 미리 불러와야 합니다.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="destination"></param>
    public void setup(UnitCombat attacker, Vector3 destination)
    {
        attackerInfo = attacker;


        _damage = attackerInfo.resultDamage;
        _targetFaction = attackerInfo.targetFaction;
        _aoe = new Vector2(attacker.resultAOE, attacker.resultAOE);
        _AP = attackerInfo.resultAP;
        projectileImage.sprite = attacker.attackImage;

        Vector2 offsetToTarget = destination - transform.position;
        RB.velocity = offsetToTarget * attacker.projectileSpeed;

        // 거리 / 투사체 속도 = 목표까지 걸리는 시간
        // 목표까지 걸리는 시간 * 50(초당 프레임) = 목표까지 도달하는데 걸리는 fixedUpdate 프레임 수 
        // 매 프레임마다 거리 계산 하는거보다 int 비교 하는게 짧을거같아서 이렇게 했어요
        lifeTime = (int)(50 * offsetToTarget.magnitude/attacker.projectileSpeed);
    }
    private void FixedUpdate()
    {
        if(lifeTime ==0)
        {
            dealDamage();

            //인게임 매니져 오브젝트 풀링 매니져 생기면 아래 줄 수정

            //            .ObjectPooling(, gameObject);
        }
        else
        {
            lifeTime--;
        }

    }
    void dealDamage()
    {
        Collider2D []unitsInRange = Physics2D.OverlapAreaAll(_destination, _aoe);
        foreach (var target in unitsInRange)
        {
            UnitCombat targetCombat = target.GetComponent<UnitCombat>();
            if(targetCombat != null)
            {
                if(_targetFaction == faction.both || _targetFaction == targetCombat.ownedFaction)
                {
                    targetCombat.TakeDamage(_damage,_AP);
                }
            }
        }
    }
}
