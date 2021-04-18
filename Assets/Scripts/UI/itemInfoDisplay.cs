using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInfoDisplay : UIPopup, IPointerExitHandler
{
    public override PopupID GetPopupID() { return PopupID.UIItemToolTip; }

    [SerializeField] private Text _name, _type, _value, _desc,_enchant;
    RectTransform rectTransform;

    public class Param
    {
        public Item item;
        public Vector2 position;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void SetInfo()
    {
        Param param = GetParam() as Param;

        _name.text = param.item.name;
        _type.text = param.item.type.ToString();
        _value.text = param.item.value.ToString();
        _desc.text = param.item.desc;
        gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
        rectTransform.sizeDelta = new Vector2(400, 93 + _desc.cachedTextGenerator.lineCount * 22.5f);

        SetLocation(param.position);
    }

    public void SetLocation(Vector2 mouseLocation)
    {
        Debug.Log(mouseLocation);
        rectTransform.position = mouseLocation + new Vector2(-2,2);
        rectTransform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Pop();
    }
}
