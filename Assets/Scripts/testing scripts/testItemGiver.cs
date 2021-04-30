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
            int givenInput = int.Parse(_inputField.text);
            _inputField.text = "";
            if(Items.DB.ContainsKey(givenInput))
            {
                Debug.Log("아이템 추가!");
                List<Transform> itemslot = IngameManager.UIInventory.getEmptySlots(1);
                if (itemslot.Count != 0)
                {
                    GameObject newItem = Global.ResourceManager.LoadPrefab(Items.PREFAB_NAME);
                    newItem.GetComponent<Item_Data>().Setup(givenInput,itemslot[0]);

                }
            }
            else
            {
                Debug.LogError("존재하지 않는 ID입니다");
            }
        }
    }
}
