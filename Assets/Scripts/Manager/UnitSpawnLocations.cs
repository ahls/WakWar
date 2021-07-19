using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnLocations : MonoBehaviour
{
    public bool NextSequenceWhenLoaded = true;
    public Transform Axe, Sword, Shield, Bow, Gun, Throw, Heal, Inst, Blunt, Wak;

    // Start is called before the first frame update
    void Start()
    {
        IngameManager.StageManager.SetSpawnLocations(this);
        if(NextSequenceWhenLoaded)
        {
            IngameManager.ProgressManager.NextSequence();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
