using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLLoader : MonoBehaviour
{
    public const string WEAPON_PATH = "weapons";
    public const string ITEM_PATH = "items";
    public const string STAGE_PATH = "stage_xml";
    public const string DIALOG_PATH = "dialog_xml";
    // Start is called before the first frame update
    void Awake()
    {
        WeaponContainer.Load(WEAPON_PATH);
        ItemContainer.Load(ITEM_PATH);
        StageXML.Load(STAGE_PATH);
        DialogueXML.Load(DIALOG_PATH);
    }

}
