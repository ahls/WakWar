using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _btnMargin;
    private GameObject _notifyText;
    private GameObject _notifyWindow;

    private void Start()
    {
        Global.instance.SetUIManager(this);
        Item_Draggable.initItems(_canvas.GetComponent<Canvas>());
        //_notifyText = Global.ResourceManager.LoadPrefab("NotifyText");
    }

    public void PushNotiMsg(string text, float lifeTime)
    {
        var notiMsgObject = Instantiate(Global.ResourceManager.LoadPrefab("NotifyText"), _canvas.transform);
        notiMsgObject.SetActive(true);
        notiMsgObject.GetComponent<UINotifyText>().SetInfo(text, lifeTime);
    }
}