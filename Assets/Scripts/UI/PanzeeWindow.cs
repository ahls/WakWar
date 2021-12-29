using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanzeeWindow : UIPopup
{
    [SerializeField] private Transform[] slotLists;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private RectTransform contentBox;
    private int[] contentCounter = new int[4] { 0,0,0,1};
    private float contentBoxHeight = 80;
    private bool collapsed = false;
    // Start is called before the first frame update
    private void Start()
    {
        if(IngameManager.UIPanzeeWindow == null)
        {
            IngameManager.UIPanzeeWindow = this;
            Pop();
        }
    }


    public void addToList(string name, GameObject unit, ClassType unitClass)
    {
        Transform tempParent;
        switch (unitClass)
        {
            case ClassType.Warrior:
                tempParent = slotLists[0];
                contentCounter[0]++;
                break;
            case ClassType.Shooter:
                tempParent = slotLists[1];
                contentCounter[1]++;
                break;
            case ClassType.Supporter:
                tempParent = slotLists[2];
                contentCounter[2]++;
                break;
            default:
                return;
        }
        contentBoxResize();
        GameObject newSlot = Global.ObjectManager.SpawnObject(slotPrefab.name);
        newSlot.GetComponent<panzeeInventory>().Setup(name,unit,tempParent);
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
                slot.GetComponent<panzeeInventory>().Collapse(collapsed);
            }
        }
        contentBoxResize();
    }

    public override PopupID GetPopupID(){return PopupID.UIUnitWindow;}

    public override void SetInfo()
    {
    }

    public override void PostInitialize()
    {
    }
}
