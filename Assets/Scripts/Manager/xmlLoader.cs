﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLLoader
{
    public const string WEAPON_PATH = "weapons";
    public const string ITEM_PATH = "items";
    public const string STAGE_PATH = "stage_xml";
    public const string DIALOG_PATH = "dialog_xml";
    public const string PROGRESS_PATH = "progress_xml";
    // Start is called before the first frame update

    public XMLLoader()
    {
        WeaponContainer.Load(WEAPON_PATH);
        foreach (var item in Weapons.DB.Keys)
        {
            Debug.Log(item.ToString() + ": " +Weapons.DB[item].name);
        }
        ItemContainer.Load(ITEM_PATH);
        StageXML.Load(STAGE_PATH);
        DialogueXML.Load(DIALOG_PATH);
        ProgressXML.Load(PROGRESS_PATH);
    }

}
