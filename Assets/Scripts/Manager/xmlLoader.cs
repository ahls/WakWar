﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xmlLoader : MonoBehaviour
{
    public const string weaponPath = "weapons";
    public const string itemPath = "items";

    // Start is called before the first frame update
    void Start()
    {
        WeaponContainer.Load(weaponPath);
        ItemContainer.Load(itemPath);
        foreach (var item in Items.DB.Keys)
        {
            Debug.Log(item);
        }
    }

}
