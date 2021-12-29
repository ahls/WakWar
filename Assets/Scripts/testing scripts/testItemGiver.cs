using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testItemGiver : MonoBehaviour
{
    private InputField _inputField;
    // Start is called before the first frame update
    void Start()
    {
        _inputField = GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
            {
            if(_inputField.text == "상점")
            {
                Global.UIPopupManager.Push(PopupID.UIShop);
                return;
            }
            else if(_inputField.text == "돈")
            {
                IngameManager.UIInventory.AddMoney(100);
                return;
            }
            int givenInput = int.Parse(_inputField.text);
            _inputField.text = "";
            
            if(Items.DB.ContainsKey(givenInput))
            {
                Debug.Log("치트 사용됨!");
                List<Transform> itemslot = IngameManager.UIInventory.GetEmptySlots();
                if (itemslot.Count != 0)
                {
                    GameObject newItem = Global.ObjectManager.SpawnObject(Items.PREFAB_NAME);
                    newItem.GetComponent<Item_Data>().Setup(givenInput,itemslot[0]);

                }
            }
            else
            {
                Debug.LogWarning("존재하지 않는 ID입니다");
            }
        }
    }
}
