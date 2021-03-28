using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponLoader : MonoBehaviour
{
    public const string path = "weapons";

    // Start is called before the first frame update
    void Start()
    {
        WeaponDB.Load(path);
    }

}
