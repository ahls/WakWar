using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackEffect : MonoBehaviour
{
    private UnitCombat _attackerInfo;
    //레퍼런스를 잡긴 했는데 범위공격같은애들이 있으면, 공격 정보를 매번 끌어와야되니까 일단 아래 데미지랑 타겟팩션같은거는 담아둘 변수를 만들어놨어요
    private int _damage;
    private int _AP; // 아머피어싱
    private float _aoe;
    private Vector3 _destination;
    private Faction _targetFaction;
    private GameObject _onHitEffect;
    private int _lifeTime;//착탄지점에 도착했는지 여부를 검사할때 쓰일거
    [SerializeField] private Rigidbody2D _rigidBody;

    private string _name;

    //그래픽
    [SerializeField] private SpriteRenderer _projectileImage;
    private float _heightDelta = 0.1f;
    private float _deltaDelta;




    /// <summary>
    /// 셋업 하기 전에 공격자 위치로 미리 불러와야 합니다.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="destination"></param>
    public void Setup(UnitCombat attacker, Vector3 destination,string name, int torque, float initDelta)
    {
        _name = name;
        _attackerInfo = attacker;

        _damage = _attackerInfo.TotalDamage;
        _targetFaction = _attackerInfo.TargetFaction;
        _aoe = attacker.TotalAOE;
        _AP = _attackerInfo.TotalAP;
        _destination = destination;
        _projectileImage.sprite = attacker.AttackImage;
        Vector2 offsetToTarget = destination - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.back,offsetToTarget);
        _rigidBody.velocity = offsetToTarget.normalized * attacker.ProjectileSpeed;
        _rigidBody.AddTorque(torque);

        // 거리 / 투사체 속도 = 목표까지 걸리는 시간
        // 목표까지 걸리는 시간 * 50(초당 프레임) = 목표까지 도달하는데 걸리는 fixedUpdate 프레임 수 
        // 매 프레임마다 거리 계산 하는거보다 int 비교 하는게 짧을거같아서 이렇게 했어요
        _lifeTime = (int)(50 * offsetToTarget.magnitude / attacker.ProjectileSpeed);

        if (_lifeTime > 0 && initDelta > 0)
        {//투사체 포물선 그리게

            _heightDelta = initDelta;
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

            if (!string.IsNullOrWhiteSpace(_name))
            {
                Global.ObjectPoolManager.ObjectPooling(_name, this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
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
