using System.Collections;
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
}