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
    private UnitCombat _attacker;
    private int _lifeTime;//착탄지점에 도착했는지 여부를 검사할때 쓰일거
    [SerializeField] private Rigidbody2D _rigidBody;

    private float _critChance = 0f;
    private float _critMult = 1f;
    private float _lifeDrain = 0f;

    //그래픽
    [SerializeField] private SpriteRenderer _projectileImage;
    private float _heightDelta = 0.1f;
    private float _deltaDelta;
    private GameObject _onHitEffect;
    private bool _torque;

    public string _impactAudio;

    /// <summary>
    /// 셋업 하기 전에 공격자 위치로 미리 불러와야 합니다.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="destination"></param>
    public void Setup(UnitCombat attacker,int damage, Vector3 destination)
    {
        _impactAudio = attacker.ImpactAudio;
        _attacker = attacker;
        Setup(  damage, 
                attacker.TotalAOE, 
                attacker.TotalAP, 
                attacker.AttackImage, 
                attacker.ProjectileSpeed, 
                destination, attacker.TargetFaction);
    }
    public void Setup(int dmg, float aoe,int ap,Sprite projImage,float projSpeed, Vector3 destination,Faction targetFaction)
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
        _torque = false;
        // 거리 / 투사체 속도 = 목표까지 걸리는 시간
        // 목표까지 걸리는 시간 * 50(초당 프레임) = 목표까지 도달하는데 걸리는 fixedUpdate 프레임 수 
        // 매 프레임마다 거리 계산 하는거보다 int 비교 하는게 짧을거같아서 이렇게 했어요
        _lifeTime = (int)(50 * offsetToTarget.magnitude / projSpeed);
    }
    private void FixedUpdate()
    {
        _lifeTime--;
        if (_lifeTime == 0)
        {
            DealDamage();
            if (_impactAudio != "null")
            {
                Global.AudioManager.PlayOnce(_impactAudio, true);
                _impactAudio = "null";
            }
            ResetAdditionalProperties();
            Global.ObjectManager.ReleaseObject(Weapons.attackPrefab, this.gameObject);
        }
        else
        {
            if (_deltaDelta != 0)
            {
                _heightDelta += _deltaDelta;
                transform.position += new Vector3(0, _heightDelta, 0);
            }
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
                    
                    if(_critChance > 0)
                    {//크리티컬 확률이 있으면 확인
                        if(Random.Range(0,1) >= _critChance)
                        {
                            targetCombat.TakeDamage((int)(_damage *  _critMult), _AP);
                            LifeSteal(_critMult * _lifeDrain);
                        }
                    }
                    targetCombat.TakeDamage(_damage, _AP);
                    LifeSteal(_lifeDrain);
                }
            }
        }
    }




    public void AddSound(string attackSound)
    {
        _impactAudio = attackSound;
    }
    private void LifeSteal(float drainAmount)
    {
        if(_lifeDrain > 0)
        {
            _attacker.TakeDamage((int)(-_damage * drainAmount));
        }
    }

    /// <summary>
    /// 투사체에 회전을 넣거나 포물선을 넣을때 사용됨
    /// </summary>
    /// <param name="torq"></param>
    /// <param name="initHDelta"></param>
    public void AddTrajectory(int torq, float initHDelta)
    {
        if (torq != 0)
        {
            _rigidBody.AddTorque(torq);
            _torque = true;
        }
        if (_lifeTime > 0 && initHDelta > 0)
        {//투사체 포물선 그리게

            _heightDelta = initHDelta;
            transform.position += new Vector3(0, 0.15f, 0);
            _deltaDelta = _heightDelta * -2 / _lifeTime;
        }
    }

    /// <summary>
    /// 크리티컬이나 라이프 스틸 효과를 추가할때 사용됨
    /// </summary>
    /// <param name="chance"></param>
    /// <param name="multi"></param>
    /// <param name="lifeDrain"></param>
    /// <param name="attacker"></param>
    public void AddHitEffect(float chance, float multi, float lifeDrain,UnitCombat attacker = null)
    {
        _critChance = chance;
        _critMult = multi;
        _lifeDrain = lifeDrain;
        if (attacker != null)
        {
            _attacker = attacker;
        }
    }
    private void ResetAdditionalProperties()
    {
        _attacker = null;
        _critChance = 0f;
        _critMult = 1f;
        _lifeDrain = 0f;
        _heightDelta = 0f;
        _deltaDelta = 0f;
    }
}
