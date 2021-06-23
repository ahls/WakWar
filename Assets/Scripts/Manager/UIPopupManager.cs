using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupID
{
    UIWakWindow,
    UIInventory,
    UIUnitWindow,
    UIItemToolTip,
    UIShop,
}

public class UIPopupManager : MonoBehaviour
{
    private List<UIPopup> _popupQueue = new List<UIPopup>();

    public void Start()
    {
        Global.instance.SetUIPopupManager(this);
    }

    public void Push(PopupID id, object param = null)
    {
        var findPopup = FindPopup(id);
        if (findPopup != null)
        {
            Pop(findPopup);
        }

        var currentPopup = Global.ResourceManager.LoadPrefab(id.ToString(), true);
        currentPopup.transform.SetParent(this.transform);
        currentPopup.transform.SetAsLastSibling();
        currentPopup.transform.localPosition = Vector3.zero;

        var uiPopupScript = currentPopup.GetComponent<UIPopup>();
        if (param != null)
        {
            Debug.Log(param);
            Debug.Log(uiPopupScript.Param);
            uiPopupScript.Param = param; 
        }
        uiPopupScript.PostInitialize();
        _popupQueue.Add(uiPopupScript);
    }

    public void Pop(PopupID id)
    {
        var findPopup = _popupQueue.Find(x => x.GetPopupID() == id);

        if (findPopup != null)
        {
            findPopup.Pop();
        }
    }

    public void Pop(UIPopup uiPopup)
    {
        _popupQueue.Remove(uiPopup);
    }

    public UIPopup FindPopup(PopupID id)
    {
        foreach (var popup in _popupQueue)
        {
            if (popup.GetPopupID() == id)
            {
                return popup;
            }
        }

        return null;
    }
    public void ForceAddQueue(UIPopup popup)
    {
        _popupQueue.Add(popup);
    }
}
