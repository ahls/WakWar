using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIDraggable : MonoBehaviour
{
    protected static Canvas _canvas;
    protected static float _canvasScale;
    // Start is called before the first frame update
    static public void SetupDraggableWindow(GameObject canvas)
    {
        _canvas = canvas.GetComponent<Canvas>();
        _canvasScale = _canvas.scaleFactor;
    }

    protected void SetSecondToLast()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 2);
    }
}
