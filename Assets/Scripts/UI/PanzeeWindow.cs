using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanzeeWindow : MonoBehaviour
{
    public static PanzeeWindow instance;
    [SerializeField] Transform[] slotLists;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] RectTransform contentBox;
    private int[] contentCounter = new int[4] { 0,0,0,1};
    private float contentBoxHeight = 80;
    private bool collapsed = false;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            addToList("thinggy", gameObject, WeaponType.Shooter);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            addToList("11", gameObject, WeaponType.Warrior);
        }
    }

    public void addToList(string name, GameObject unit, WeaponType unitClass)
    {
        Transform tempParent;
        switch (unitClass)
        {
            case WeaponType.Warrior:
                tempParent = slotLists[0];
                contentCounter[0]++;
                break;
            case WeaponType.Shooter:
                tempParent = slotLists[1];
                contentCounter[1]++;
                break;
            case WeaponType.Supporter:
                tempParent = slotLists[2];
                contentCounter[2]++;
                break;
            default:
                return;
        }
        contentBoxResize();
        GameObject newSlot = Global.ResourceManager.LoadPrefab(slotPrefab.name);
        newSlot.GetComponent<panzeeInventory>().setup(name,unit,tempParent);
    }


    private void contentBoxResize()
    {
        contentCounter[3] = Mathf.Max(contentCounter);
        contentBox.sizeDelta = new Vector2(0, contentBoxHeight * contentCounter[3]);
    }
    public void collapse()
    {
        collapsed = !collapsed;
        contentBoxHeight = collapsed ? 85 : 160;
        for (int listIndex = 0; listIndex < 3; listIndex++)
        {
            foreach(Transform slot in slotLists[listIndex].transform)
            {
                slot.GetComponent<panzeeInventory>().collapse(collapsed);
            }
        }
        contentBoxResize();
    }
}
