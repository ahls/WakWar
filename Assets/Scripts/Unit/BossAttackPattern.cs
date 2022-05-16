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
            HealthSystem selectedUnitCombat = unit.GetComponent<HealthSystem>();
            if (selectedUnitCombat != null)
            {
                if (selectedUnitCombat.OwnedFaction == TargetFaction)
                {
                    selectedUnitCombat.TakeDamage(Damage);
                }
            }
        }
    }
}
