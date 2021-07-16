using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnLocations : MonoBehaviour
{
    public Transform Axe, Sword, Shield, Bow, Gun, Throw, Heal, Inst, Blunt;
    // Start is called before the first frame update
    void Start()
    {
        IngameManager.StageManager.SetSpawnLocations(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
