using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class itemInfoDisplay : MonoBehaviour,IPointerExitHandler
{
    public static itemInfoDisplay instance;
    [SerializeField] private Text _name, _type, _value, _desc;
    RectTransform rectTransform;
    
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if(instance == null)
        {
            instance = this;
        }
        gameObject.SetActive(false);
    }

    public void loadInfo(Item item)
    {
        _name.text = item.name;
        _type.text = item.type.ToString();
        _value.text = item.value.ToString();
        _desc.text = item.desc;
        gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
        rectTransform.sizeDelta = new Vector2(400, 93+_desc.cachedTextGenerator.lineCount * 22.5f);

    }
    public void setLocation(Vector2 mouseLocation)
    {
        Debug.Log(mouseLocation);
        rectTransform.position = mouseLocation + new Vector2(-2,2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
