﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotifyText : MonoBehaviour
{
    [SerializeField] private Text _text;
    private float _lifeTime;

    private const float decayRate = 2f;
    private const float risingSpeed = 20f;
    private Vector2 _resetPos;

    private void Awake()
    {
        _resetPos = this.transform.position;
    }

    public void SetInfo(string text, float lifeTime)
    {
        this.transform.position = _resetPos;
        _text.color = new Color(1, 1, 1, 1);
        _text.text = text;
        _lifeTime = lifeTime;

        StartCoroutine(ShowMsg());
    }

    private bool decay()
    {
        this.transform.position += new Vector3(0, risingSpeed * (decayRate * Time.deltaTime), 0);
        _text.color -= new Color(0, 0, 0, decayRate * Time.deltaTime);

        if (_text.color.a < decayRate * Time.deltaTime)
        {
            return false;
        }

        return true;
    }

    private IEnumerator ShowMsg()
    {
        yield return new WaitForSeconds(_lifeTime);

        while (decay()) {
            yield return null;
        }

        Global.ObjectManager.ReleaseCanvasObject("NotifyText", this.gameObject);
    }
}
