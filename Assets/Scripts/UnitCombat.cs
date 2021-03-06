using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    #region 변수

    //공격관련
    public int attackDamage { get; set; }
    public float attackRage { get; set; }
    public float attackSpeed { get; set; }

    //방어력
    public int armor { get; set; }

    //타입
    public WeaponType weaponType;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
