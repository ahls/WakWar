using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupID
{
    UIWakWindow,
    UIInventory,
    UIUnitWindow,
}

public class UIPopupManager : MonoBehaviour
{
    private Queue<GameObject> _popupQueue = new Queue<GameObject>();

    public void Start()
    {
        Global.instance.SetUIPopupManager(this);
    }

    public void Push(PopupID id)
    {

    }
}
