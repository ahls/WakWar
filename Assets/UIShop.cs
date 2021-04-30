using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShop : UIPopup
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform [] _tradeSlots;
    [SerializeField] private GameObject sellingPanel;
    public override PopupID GetPopupID() { return PopupID.UIShop; }

    public override void SetInfo()
    {
    }
    public override void PostInitialize()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        IngameManager.instance.SetShop(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            Global.UIPopupManager.ForceAddQueue(this);
            CreateItems();
        }
        
    }

    /// <summary>
    /// 상점층 도달하면 불름
    /// 
    /// </summary>
    public void CreateItems()
    {
        for (int i = 0; i < _tradeSlots.Length; i++)
        {
            int newItemID = 20000 + (Random.Range(0, 9) * 100) + Random.Range(0, 2);
            //아이템 생성

            GameObject newItem = Global.ResourceManager.LoadPrefab(itemPrefab.name);
            newItem.GetComponent<Item_Data>().Setup(newItemID, _tradeSlots[i]);
            newItem.GetComponent<Item_Drag>().SellingItem = true;
        }
    }
    public void ToggleSellingWindow(bool state)
    {
        sellingPanel.SetActive(state);
    }
}
