using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xmlLoader : MonoBehaviour
{
    public const string weaponPath = "weapons";
    public const string itemPath = "items";

    // Start is called before the first frame update
    void Start()
    {
        WeaponDB.Load(weaponPath);
        ItemDB.Load(itemPath);
    }

}
