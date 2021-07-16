using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuBar : MonoBehaviour
{
    public void PushUIWakWindow()
    {
        if (Global.UIPopupManager.FindPopup(PopupID.UIWakWindow) == null)
        {
            Global.UIPopupManager.Push(PopupID.UIWakWindow);
        }
        else
        {
            Global.UIPopupManager.Pop(PopupID.UIWakWindow);
        }
    }
    public void PushUIUnitWindow()
    {
        if (Global.UIPopupManager.FindPopup(PopupID.UIUnitWindow) == null)
        {
            Global.UIPopupManager.Push(PopupID.UIUnitWindow);
        }
        else
        {
            Global.UIPopupManager.Pop(PopupID.UIUnitWindow);
        }
    }

    public void PushUIInventory()
    {
        if (Global.UIPopupManager.FindPopup(PopupID.UIInventory) == null)
        {
            Global.UIPopupManager.Push(PopupID.UIInventory);
        }
        else
        {
            Global.UIPopupManager.Pop(PopupID.UIInventory);
        }
    }
}
