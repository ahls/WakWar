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
    }
}
