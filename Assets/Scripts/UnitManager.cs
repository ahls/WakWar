using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        IngameManager.instance.SetUnitManager(this);
    }
}
