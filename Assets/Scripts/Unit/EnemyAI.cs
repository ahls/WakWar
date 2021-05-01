using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private static bool _aiON = false;
    private UnitCombat _unitCombat;


    public ClassType PreferredClass;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void EnableEnemyAI(bool state)
    {
        _aiON = state;
    }
}
