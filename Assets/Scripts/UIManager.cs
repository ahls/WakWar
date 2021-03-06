﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _btnMargin;

    [SerializeField] private GameObject _notifyText;
    [SerializeField] private GameObject _notifyWindow;

    private void Start()
    {
        Global.instance.SetUIManager(this);
    }

    public void PushNotiMsg(string text, float lifeTime)
    {
        var notiMsgObject = Instantiate(_notifyText, _canvas.transform);
        notiMsgObject.SetActive(true);
        notiMsgObject.GetComponent<UINotifyText>().SetInfo(text, lifeTime);
    }
}