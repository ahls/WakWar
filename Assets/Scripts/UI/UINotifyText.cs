using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotifyText : MonoBehaviour
{
    [SerializeField] private Text _text;
    private float _lifeTime;
    private float decayRate;

    public void SetInfo(string text, float lifeTime)
    {
        _text.text = text;
        _lifeTime = lifeTime;
        decayRate = 1 / (50 * _lifeTime);
        StartCoroutine(ShowMsg());
    }

    private void FixedUpdate()
    {
        decay();
    }

    private void decay()
    {
        //text.color 는 레퍼런스가 아니라 카피를 하는거같아요.. 그래서 미리 빼둘수가 없습니다.
        _text.rectTransform.position += Vector3.up;
        _text.color -= new Color(0, 0, 0, decayRate); 
    }

    private IEnumerator ShowMsg()
    {
        yield return new WaitForSeconds(_lifeTime);

        Destroy(this.gameObject);
    }
}
