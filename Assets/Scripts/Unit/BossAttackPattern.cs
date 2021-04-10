using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackPattern : MonoBehaviour
{
    public int Damage;
    public float AOE;
    public faction TargetFaction;
    public ParticleSystem PS;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void patternCalled()
    {
        PS.Play();
        Collider2D [] unitsInRange = Physics2D.OverlapCircleAll(transform.position, AOE);
        foreach(Collider2D unit in unitsInRange)
        {
            UnitCombat selectedUnitCombat = unit.GetComponent<UnitCombat>();
            if (selectedUnitCombat != null)
            {
                if (selectedUnitCombat.targetFaction != TargetFaction)
                {
                    selectedUnitCombat.TakeDamage(Damage);
                }
            }
        }
    }
}
