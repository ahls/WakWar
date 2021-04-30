using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackEffect : MonoBehaviour
{
    //레퍼런스를 잡긴 했는데 범위공격같은애들이 있으면, 공격 정보를 매번 끌어와야되니까 일단 아래 데미지랑 타겟팩션같은거는 담아둘 변수를 만들어놨어요
    private int _damage;
    private int _AP; // 아머피어싱
    private float _aoe;
    private Vector3 _destination;
    private Faction _targetFaction;
    private int _lifeTime;//착탄지점에 도착했는지 여부를 검사할때 쓰일거
    [SerializeField] private Rigidbody2D _rigidBody;


    //그래픽
    [SerializeField] private SpriteRenderer _projectileImage;
    private float _heightDelta = 0.1f;
    private float _deltaDelta;
    private GameObject _onHitEffect;




    /// <summary>
    /// 셋업 하기 전에 공격자 위치로 미리 불러와야 합니다.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="destination"></param>
    public void Setup(UnitCombat attacker, Vector3 destination, int torque, float initDelta)
    {
        Setup(  attacker.TotalDamage, 
                attacker.TotalAOE, 
                attacker.TotalAP, 
                attacker.AttackImage, 
                attacker.ProjectileSpeed, 
                torque, initDelta, destination, 
                attacker.TargetFaction);
    }
    public void Setup(int dmg, float aoe,int ap,Sprite projImage,float projSpeed,int torque,float initHDelta, Vector3 destination,Faction targetFaction)
    {
        _damage = dmg;
        _targetFaction = targetFaction;
        _aoe = aoe;
        _AP = ap;
        _destination = destination;
        _projectileImage.sprite = projImage;
        Vector2 offsetToTarget = destination - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.back, offsetToTarget);
        _rigidBody.velocity = offsetToTarget.normalized * projSpeed;
        _rigidBody.AddTorque(torque);

        // 거리 / 투사체 속도 = 목표까지 걸리는 시간
        // 목표까지 걸리는 시간 * 50(초당 프레임) = 목표까지 도달하는데 걸리는 fixedUpdate 프레임 수 
        // 매 프레임마다 거리 계산 하는거보다 int 비교 하는게 짧을거같아서 이렇게 했어요
        _lifeTime = (int)(50 * offsetToTarget.magnitude / projSpeed);

        if (_lifeTime > 0 && initHDelta > 0)
        {//투사체 포물선 그리게

            _heightDelta = initHDelta;
            transform.position += new Vector3(0, 0.15f, 0);
            _deltaDelta = _heightDelta * -2 / _lifeTime;
        }
        else
        {
            _deltaDelta = 0;
            _heightDelta = 0;
        }
    }
    private void FixedUpdate()
    {
        if (_lifeTime == 0)
        {
            DealDamage();

            Global.ObjectPoolManager.ObjectPooling(Weapons.attackPrefab, this.gameObject);
        }
        else
        {
            _lifeTime--;
            _heightDelta += _deltaDelta;
            transform.position += new Vector3(0, _heightDelta, 0);

        }

    }

    private void DealDamage()
    {
        Collider2D[] unitsInRange = Physics2D.OverlapCircleAll(_destination, _aoe);
        foreach (var target in unitsInRange)
        {
            UnitCombat targetCombat = target.GetComponent<UnitCombat>();
            if (targetCombat != null)
            {
                if (_targetFaction == targetCombat.OwnedFaction || _targetFaction == Faction.Both)
                {
                    //Debug.Log(_attackerInfo.gameObject.name + " dealt damage to " + targetCombat.gameObject.name + _damage);

                    targetCombat.TakeDamage(_damage, _AP);
                }
            }
        }
    }
}
