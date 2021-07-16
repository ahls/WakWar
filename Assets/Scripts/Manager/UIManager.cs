using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _btnMargin;
    [SerializeField] private GameObject _menuBar;
    [SerializeField] private GameObject _readyButton;
    private GameObject _notifyText;
    private GameObject _notifyWindow;

    private void Start()
    {
        Global.instance.SetUIManager(this);
        UIDraggable.SetupDraggableWindow(_canvas);
        //_notifyText = Global.ObjectManager.SpawnObject("NotifyText");
    }

    public void PushNotiMsg(string text, float lifeTime)
    {
        var notiMsgObject = Global.ObjectManager.SpawnObject("NotifyText", true);
        notiMsgObject.transform.SetParent(_canvas.transform);
        notiMsgObject.SetActive(true);
        notiMsgObject.GetComponent<UINotifyText>().SetInfo(text, lifeTime);
    }
    public void ToggleMenu(bool state)
    {
        _menuBar.SetActive(state);
    }
    public void ShowReadyButton()
    {
        _readyButton.SetActive(true);
    }
    public void OnHitReadyButton()
    {
        IngameManager.ProgressManager.NextSequence();
    }

}