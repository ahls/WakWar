﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private BossAttackPattern [] attackPatterns;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
               
    }
    public void BossAttack(int patternIndex)
    {
        attackPatterns[patternIndex].patternCalled();
    }
}
