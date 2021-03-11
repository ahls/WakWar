using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackEffect : MonoBehaviour
{
    private int damage;
    private float speed;
    private Vector2 aoe;
    private Vector3 destination;
    private faction targetFaction;
    private GameObject projectileEffect;
    private GameObject onHitEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(float Aoe, float Speed , int Damage,faction Targets , Vector3 Destination, GameObject ProjectileEffect = null, GameObject OnHitEffect = null)
    {
        aoe = new Vector2(Aoe,Aoe);
        speed = Speed;
        damage = Damage;
        destination = Destination;
        targetFaction = Targets;
        projectileEffect = ProjectileEffect;
        onHitEffect = OnHitEffect;
    }

    void dealDamage()
    {
        Collider2D []unitsInRange = Physics2D.OverlapAreaAll(transform.position, aoe);
        foreach (var target in unitsInRange)
        {
            UnitCombat targetCombat = target.GetComponent<UnitCombat>();
            if(targetCombat != null)
            {
                if(targetFaction == faction.both || targetFaction == targetCombat.ownedFaction)
                {
                    targetCombat.takeDamage(damage);
                }
            }
        }
    }
}
