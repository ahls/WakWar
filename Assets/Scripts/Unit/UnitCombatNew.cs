using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatNew : MonoBehaviour
{
    [HideInInspector] public UnitController unitController;
    private PanzeeBehaviour _panzee;

    private const float RANDOM_ATTACK_DELAY_RANGE = 0.05f;
    
    //기본스탯
    public int BaseDamage { get; set; }
    public float BaseRange { get; set; }
    public float BaseAS { get; set; } // 초당 공격
    public float BaseAOE { get; set; }
    public int BaseAP { get; set; }

    public float BaseCrit = 0f;
    public float BaseLD = 0f;//life drain
                             //최종스탯
    public int TotalDamage { get; set; }
    public float TotalRange { get; set; }
    public float TotalAS { get; set; } // 초당 공격
    public float TotalAOE { get; set; }
    public int TotalAP { get; set; }

    public float TotalCrit = 0f;
    public float TotalLD = 0f;//life drain



    public float CritChance { get; set; } = 0f;
    public float CritDmg { get; set; } = 1.5f;
    public float LifeSteal { get; set; } = 0f;

    public Transform attackTarget;

    //소리

    public int _soundVariation = 1;
    public string _attackAudio = string.Empty;
    public string _impactAudio = string.Empty;

    //이미지
    private static Vector2 _attackHeight = new Vector2(0, 0.1f);
    public float _heightDelta; //음수일경우 attack effect의 SetAngle 불러옴
    public int _torque;
    public string _impactEffect = null;
    private GameObject _effect;
    public float ProjectileSpeed { get; set; }
    public Sprite AttackImage { get; set; }



    public Faction TargetFaction;
    // Start is called before the first frame update

    void Start()
    {
        _panzee = GetComponent<PanzeeBehaviour>();
    }
    #region 공격관련
    public void Fire()
    {
        if (attackTarget == null) return; // 카이팅 안되게 막는 함수
        _effect = Global.ObjectManager.SpawnObject(Weapons.attackPrefab);
        _effect.transform.position = GetComponent<Collider2D>().ClosestPoint(attackTarget.position) + _attackHeight;

        AttackEffect attackEffectScript = _effect.GetComponent<AttackEffect>();
        if (_panzee.weaponIndex * 0.1 == 10000) // 무기가 검인경우 공격데미지 수정
        {
            attackEffectScript.Setup(this, CalculateBerserkDamage(), attackTarget.position);
        }
        else
        {
            attackEffectScript.Setup(this, TotalDamage, attackTarget.position);
        }


        if (_heightDelta < 0)
        {
            attackEffectScript.SetAngle(-_heightDelta);
        }
        else if (_torque > 0 || _heightDelta > 0)
        {
            attackEffectScript.AddTrajectory(_torque, _heightDelta);
        }

        if (LifeSteal > 0 || CritChance > 0)
        {
            attackEffectScript.AddHitEffect(CritChance, CritDmg, LifeSteal);
        }

        if (_impactEffect != null)
        {
            attackEffectScript.AddEffect(_impactEffect);
        }

        if (_attackAudio != "")
        {
            if (_soundVariation > 1)
            {
                Global.AudioManager.PlayOnceAt(_attackAudio + UnityEngine.Random.Range(0, _soundVariation).ToString(), transform.position);
            }
            else
            {
                Global.AudioManager.PlayOnceAt(_attackAudio, transform.position, true);
            }
        }
    }

    public void Attack()
    {
        ResetAttackTimer();
        UpdatePlaybackSpeed();
        unitController.animator.SetTrigger("Attack");
        unitController.unitStats.RotateDirection(attackTarget.transform.position.x - transform.position.x);
    }

    private void ResetAttackTimer()
    {
        StartCoroutine(unitController.AttackDelay(UnityEngine.Random.Range(0, RANDOM_ATTACK_DELAY_RANGE) + (1 / TotalAS)));

    }

    public void UpdatePlaybackSpeed()
    {
        unitController.animator.speed = Mathf.Max(TotalAS, 1f);
    }
    public string GetImpactSound()
    {
        return _impactAudio;
    }

    #endregion

    #region helper functions
    private int CalculateBerserkDamage()
    {

        float maxDmgBonus = 1 + (_panzee.GetItemRank() + 1) * 0.5f; // 50%, 100% 150% 200% 퍼센트의 추뎀
        int additionalDmg = Mathf.FloorToInt(maxDmgBonus * (unitController.healthSystem.GetMissingHealth()) * unitController.healthSystem._healthInversed);

        return additionalDmg;
    }

    public float OffsetToTargetBound()
    {
        Vector2 targetBoundLoc = attackTarget.GetComponent<Collider2D>().ClosestPoint(transform.position);
        Vector2 unitBoundLoc = GetComponent<Collider2D>().ClosestPoint(attackTarget.position);
        return (targetBoundLoc - unitBoundLoc).magnitude;
    }
    #endregion
}
