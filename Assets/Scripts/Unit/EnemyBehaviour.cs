using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public string UnitName;
    static public bool EnemyAIenabled = false;
    private int _searchTimer;
    static private int _searchIndex = 0;
    public ClassType PreferredClass;


    //유닛 스탯 설정창
    [SerializeField] private int HP,Armor, Damage, AP;
    [SerializeField] private float MoveSpeed, AttackSpeed;
    [SerializeField] private string ProjectileImage, ProjectileSound, ImpactSound, DeathSound;
    [SerializeField] UnitCombat _unitCombat;
    [SerializeField] UnitStats unitStats;

    // Start is called before the first frame update
    void Awake()
    {
        _unitCombat.EnemySetup(HP, Armor, AP, Damage, Global.ResourceManager.LoadTexture(ProjectileImage), DeathSound,ProjectileSound,ImpactSound) ;
        _searchTimer = _searchIndex++;
        _searchIndex = _searchIndex & 64;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void Search()
    {

        Transform BestTarget = null;
        List<Transform> listInRange = new List<Transform>();

        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, TotalRange + SearchRange);
        if (PreferredClass != ClassType.Null)
        {//선호대상이 있는경우
            List<Transform> preferredList = new List<Transform>();

            foreach (Collider2D selected in inRange)
            {
                UnitCombat selectedCombat = selected.GetComponent<UnitCombat>();
                if (selectedCombat != null && selectedCombat != this)
                {
                    if (selectedCombat.OwnedFaction == TargetFaction)
                    {
                        if (selectedCombat.UnitClassType == PreferredTarget)
                        {
                            preferredList.Add(selected.transform);
                        }
                        else
                        {
                            listInRange.Add(selected.transform);
                        }


                    }
                }
            }
            BestTarget = ReturnClosestUnit(preferredList);
            if (BestTarget == null)
            {
                BestTarget = ReturnClosestUnit(listInRange);
            }

        }
        else
        {//선호대상이 없는경우
            foreach (Collider2D selected in inRange)
            {
                UnitCombat selectedCombat = selected.GetComponent<UnitCombat>();
                if (selectedCombat != null && selectedCombat != this)
                {
                    if (selectedCombat.OwnedFaction == TargetFaction)
                    {
                        listInRange.Add(selected.transform);

                    }
                }
            }
            BestTarget = ReturnClosestUnit(listInRange);
        }


        if (BestTarget != null)
        {
            AttackTarget = BestTarget;
            ActionStat = ActionStats.Attack;
        }

    }
}
