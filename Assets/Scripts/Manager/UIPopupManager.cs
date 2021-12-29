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
    UIReward
}

public class UIPopupManager : MonoBehaviour
{
    private List<UIPopup> _popupQueue = new List<UIPopup>();
    private KeyCode _inventoryKey = KeyCode.I;
    private KeyCode _optionKey = KeyCode.Escape;
    private KeyCode _panszeeKey = KeyCode.P;
    private KeyCode _wakgoodKey = KeyCode.K;
    public void Start()
    {
        Global.instance.SetUIPopupManager(this);
    }
    private void Update()
    {
        if(Input.GetKeyDown(_inventoryKey))
        {
            ToggleUI(PopupID.UIInventory);
        }
        else if (Input.GetKeyDown(_optionKey))
        {
        }
        else if (Input.GetKeyDown(_panszeeKey))
        {

            ToggleUI(PopupID.UIUnitWindow);
        }
        else if (Input.GetKeyDown(_wakgoodKey))
        {
            ToggleUI(PopupID.UIWakWindow);
        }
    }
    /// <summary>
    /// 창 열기
    /// </summary>
    /// <param name="id"></param>
    /// <param name="param"></param>
    public void Push(PopupID id, object param = null)
    {
        var findPopup = FindPopup(id);
        if (findPopup != null)
        {
            Pop(findPopup);
        }

        var currentPopup = Global.ObjectManager.SpawnObject(id.ToString(), true);
        currentPopup.transform.SetParent(this.transform);
        currentPopup.transform.SetAsLastSibling();
        currentPopup.transform.localPosition = Vector3.zero;

        var uiPopupScript = currentPopup.GetComponent<UIPopup>();
        if (param != null)
        {
            uiPopupScript.Param = param; 
        }
        uiPopupScript.PostInitialize();
        _popupQueue.Add(uiPopupScript);
    }

    /// <summary>
    /// 창 닫기
    /// </summary>
    /// <param name="id"></param>
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

    public void LoadUIs()
    {
        foreach (var popup in (PopupID[])System.Enum.GetValues(typeof(PopupID)))
        {
            if (popup != PopupID.UIItemToolTip && popup != PopupID.UIReward)
            {
                Push(popup);
            }
        }
    }
    public void PopAll()
    {
        foreach (var popup in (PopupID[])System.Enum.GetValues(typeof(PopupID)))
        {
            Pop(popup);
        }
    }
    public void ToggleUI(PopupID popupID)
    {
        if (FindPopup(popupID) == null)
        {
            Global.UIPopupManager.Push(popupID);
        }
        else
        {
            Pop(popupID);
        }
    }
}
