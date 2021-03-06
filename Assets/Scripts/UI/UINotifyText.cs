using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotifyText : MonoBehaviour
{
    [SerializeField] private Text _text;
    private float _lifeTime;

    public void SetInfo(string text, float lifeTime)
    {
        _text.text = text;
        _lifeTime = lifeTime;

        StartCoroutine(ShowMsg());
    }

    private IEnumerator ShowMsg()
    {
        yield return new WaitForSeconds(_lifeTime);

        Destroy(this.gameObject);
    }
}
