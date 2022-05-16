using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public enum UnitState
    {
        Idle,
        Move,
        Attack,
        Stun,
        Chase
    }
    [HideInInspector] public UnitState unitState;
    public UnitCombatNew unitCombat;
    public UnitMove unitStats;
    public HealthSystem healthSystem;
    public Animator animator;
    public Transform Head;

    public PanzeeBehaviour panzeeBehavior;
    private bool _attackReady = true;
    private bool _attackGround = false;
    [HideInInspector] public bool holdPosition;
    private Vector3 _targetPosition;
    // Start is called before the first frame update
    void Awake()
    {
        if (unitCombat != null) unitCombat.unitController = this;
        if (unitStats != null)
        {
            unitStats.unitController = this;
            healthSystem.OnUnitDeath += unitStats.OnUnitDeath;
        }
        if (healthSystem != null) healthSystem.unitController = this;

    }

    public void AddStun(int amount)
    {

    }
    public void ChangeFaction(Faction toWhichFaction)
    {
        switch (toWhichFaction)
        {
            case Faction.Player:
                healthSystem.HealthBarColor(Color.green);
                healthSystem.OwnedFaction = toWhichFaction;
                unitStats.Selectable = true;
                unitCombat.TargetFaction = Weapons.DB[panzeeBehavior.weaponIndex].targetFaction;
                break;
            case Faction.Enemy:
                IngameManager.UnitManager.DeselectUnit(gameObject);
                healthSystem.HealthBarColor(Color.red);
                healthSystem.OwnedFaction = toWhichFaction;
                unitStats.Selectable = false;
                unitCombat.TargetFaction = Faction.Player;
                break;
            default:
                break;
        }
    }
    public void MoveIntoRange()
    {
        unitStats.MoveToTarget(Vector2.MoveTowards(unitCombat.attackTarget.position, transform.position, unitCombat.TotalRange), false);
    }
    public void OrderAttackGround(bool state, Vector3? targetLoc = null)
    {
        _attackGround = state;
        if (state)
            _targetPosition = (Vector3)targetLoc;
    }

    public Transform ReturnClosestUnit(List<Transform> inputList)
    {
        Transform currentBestTarget = null;
        float closestDistance = float.PositiveInfinity;
        foreach (var currentUnit in inputList)
        {
            float currentDistance = (currentUnit.position - transform.position).magnitude;

            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                currentBestTarget = currentUnit;
            }
        }

        return currentBestTarget;
    }


    public IEnumerator AttackDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _attackReady = true;
    }
}
