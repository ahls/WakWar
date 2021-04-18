using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackPattern : MonoBehaviour
{
    public int Damage;
    public float AOE;
    public Faction TargetFaction;
    public ParticleSystem MyParticleSystem;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PatternCalled()
    {
        MyParticleSystem.Play();
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
