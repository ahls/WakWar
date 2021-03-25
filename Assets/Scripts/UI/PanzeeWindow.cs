using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanzeeWindow : MonoBehaviour
{
    public static PanzeeWindow instance;
    [SerializeField] Transform[] slotLists;
    [SerializeField] GameObject slotPrefab;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addToList(string name, GameObject unit, WeaponType unitClass)
    {
        Transform tempParent;
        switch (unitClass)
        {
            case WeaponType.Warrior:
                tempParent = slotLists[0];
                break;
            case WeaponType.Shooter:
                tempParent = slotLists[1];
                break;
            case WeaponType.Supporter:
                tempParent = slotLists[2];
                break;
            default:
                return;
        }
        GameObject newSlot = Global.ResourceManager.LoadPrefab(slotPrefab.name);
        newSlot.GetComponent<panzeeInventory>().setup(name,unit,tempParent);
    }

}
