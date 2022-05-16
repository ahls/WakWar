using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveKeeperBehavior : MonoBehaviour
{
    #region consts
    private const float DELAY_BETWEEN_TOMBSTONE = 5f;
    private float _skillAOE = 1f;
    #endregion

    [SerializeField] private int HP, Armor, Damage, AP, heightDelta, AttackSoundVariations = 1;
    [SerializeField] private float MoveSpeed, AttackSpeed, range;
    [SerializeField] private string ProjectileSound = "", ImpactSound = "", DeathSound = "";
   // [SerializeField] DepreccatedUnitCombat _unitCombat;
    [SerializeField] UnitMove _unitStats;
    [SerializeField] Animator _animator;


    private Coroutine _currentAction;

    #region skill related
    public UnitCombatNew tombstoneUnitCombat;
    [SerializeField] private GameObject _skillLocator;
    private bool _skillReady = true;
    private Vector3 _skillTargetLocation;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        //_unitCombat.EnemySetup(HP, Armor, AP, Damage, range, AttackSpeed, null, heightDelta);
        //_unitCombat.SoundSetup(DeathSound, ProjectileSound, ImpactSound, AttackSoundVariations);
        _unitStats.MoveSpeed = MoveSpeed;
        Wrapper();
    }

    private void Wrapper()
    {
        if (_skillReady)
        {
            CastSpell();
        }
        //else if (_unitCombat.AttackTarget != null)
        //{
        //    _unitCombat.Attack();
        //}
        else
        {
            LookForTarget();
        }
    }

    private void LookForTarget()
    {
    }

    private void CastSpell()
    {
        _skillReady = false;
        _unitStats.StopMoving();
        _animator.Play("Graveyard_Spell");
        if (IngameManager.WakgoodBehaviour == null) return;
        //묘지 떨어질 위치 정하고 표시 남겨둠. 
       
        _skillTargetLocation = IngameManager.WakgoodBehaviour.transform.position;
    }
    public void AfterSpell()
    {
        //위치에 있던 유닛들 데미지 입히고 묘비 소환함
        _currentAction = StartCoroutine(ActionDelay(1f));

        Collider2D[] unitsInRange = Physics2D.OverlapCapsuleAll(transform.position, new Vector2(1.3f, 0.63f), CapsuleDirection2D.Horizontal, 0);
        foreach (Collider2D unit in unitsInRange)
        {
            UnitCombatNew selectedUnitCombat = unit.GetComponent<UnitCombatNew>();
            if (selectedUnitCombat != null)
            {
                if (selectedUnitCombat.TargetFaction == Faction.Player)
                {
                    //selectedUnitCombat.TakeDamage(Damage);
                }
            }
        }

    }


    private void OnTombstoneDied()
    {
        StartCoroutine(SkillDelayAfterTombstoneDestroy());
    }

    #region ienumerators
    IEnumerator ActionDelay(float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);
        Wrapper();
    }
    IEnumerator SkillDelayAfterTombstoneDestroy()
    {
        yield return new WaitForSeconds(DELAY_BETWEEN_TOMBSTONE);
        _skillReady = true;
    }
    #endregion
    IEnumerator SpellKnockback(Collider2D[] affectedUnits)
    {
        foreach (Collider2D unit in affectedUnits)
        {
            unit.GetComponent<UnitController>().AddStun(10);
        }
        for (float speed = 1; speed > 0; speed -= 0.1f)
        {
            foreach (Collider2D unit in affectedUnits)
            {
                Vector3 offset = ((Vector2)(unit.transform.position - _skillTargetLocation)).normalized;
                unit.transform.position += offset * speed;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
