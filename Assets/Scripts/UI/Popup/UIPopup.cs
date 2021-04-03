using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIPopup : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public abstract PopupID GetPopupID();
    public abstract void SetInfo();

    protected RectTransform _rectTransform;

    private Vector3 _dragPivot = Vector3.zero;

    private void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }

    public void OnEnable()
    {
        SetInfo();
    }

    public object Param { get; set; }

    protected object GetParam()
    {
        return Param;
    }

    protected T GetParam<T>() where T : class
    {
        T param = Param as T;
        if (param == null)
        {
            return null;
        }

        return param;
    }

    public void Pop()
    {
        Global.UIPopupManager.Pop(this);
        Global.ObjectPoolManager.CanvasObjectPooling(this.GetPopupID().ToString(), this.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = Input.mousePosition + _dragPivot;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetAsLastSibling();
        _dragPivot = this.transform.position - Input.mousePosition;
        IngameManager.UnitManager.ControlOn = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IngameManager.UnitManager.ControlOn = true;
    }
}
