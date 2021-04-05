using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xmlLoader : MonoBehaviour
{
    public const string weaponPath = "weapons";
    public const string itemPath = "items";
    public const string StagePath = "stage_xml";

    // Start is called before the first frame update
    void Awake()
    {
        WeaponContainer.Load(weaponPath);
        ItemContainer.Load(itemPath);
        StageXML.Load(StagePath);
    }

}
