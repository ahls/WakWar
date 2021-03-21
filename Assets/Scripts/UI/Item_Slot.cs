using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_Slot : MonoBehaviour, IDropHandler
{
    public int currentNumber { get; set; } = 0;
    public void OnDrop(PointerEventData eventData)
    {
        Item_Draggable draggedItem = eventData.pointerDrag.GetComponent<Item_Draggable>();
        
        if(draggedItem != null)
        {
            if (currentNumber == 0)
            {
                draggedItem.placeItem(transform);
            }
            //draggedItem.dropItem(transform, transform.GetSiblingIndex());
//            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
