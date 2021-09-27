using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIShop : UIPopup
{
    [SerializeField] private Transform [] _tradeSlots;
    [SerializeField] private GameObject sellingPanel;
    [SerializeField] private Transform _enchantSpot;
    [SerializeField] private Button _enchantButton;
    private int _enchantPrice;
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
        if(IngameManager.UIShop == null)
        {
            IngameManager.UIShop = this;
            Pop();
        }
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
            int newItemID = 20000 + (UnityEngine.Random.Range(0, 9) * 100) + UnityEngine.Random.Range(0, 2);
            //아이템 생성

            GameObject newItem = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
            newItem.GetComponent<Item_Data>().Setup(newItemID, _tradeSlots[i]);
            newItem.GetComponent<Item_Drag>().SellingItem = true;
        }
    }
    public void ToggleSellingWindow(bool state)
    {
        sellingPanel.SetActive(state);
    }

    public void EnableEnchantButton(int price)
    {
        _enchantPrice = price;
        _enchantButton.interactable = true;
    }
    public void DisableEnchantButton()
    {
        _enchantPrice = 0;
        _enchantButton.interactable = false;
        Debug.Log("Disable Enchant is called");
    }
    public void OnEnchantPressed()
    {
        if(IngameManager.UIInventory.AddMoney(_enchantPrice))
        {//돈이 충분하면
            AddEnchant(_enchantSpot.GetComponent<Item_Slot>().OccupyingItem.GetComponent<Item_Data>());
        }
        else
        {//돈이 모자르면
            Global.UIManager.PushNotiMsg("돈이 부족합니다.", 2);
        }
    }
    public void AddEnchant(Item_Data itemData)
    {
        List<Type> enchants = EnchantDB.GetEnchants();
        itemData.Enchant = (EnchantBase)Activator.CreateInstance(enchants[UnityEngine.Random.Range(0, enchants.Count)]);
        //itemData.Enchant = new Enchant_Dmg();
    }
}
