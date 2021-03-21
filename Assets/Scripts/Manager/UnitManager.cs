using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region 변수
    public bool ControlOn { get; set; } = true;
    private List<GameObject> _selectedUnitList = new List<GameObject>();
    [SerializeField] private GameObject _selectionBox;
    private float3 _startLocation;
    private short _selectedUnitCount = 0;       //유닛 선택이
    private bool _selectOneOnly = false;        //클릭인지
    private const float clickThreshold = 0.1f;  //확인할떄 쓰임
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        IngameManager.instance.SetUnitManager(this);
    }

    private void Update()
    {
        if (ControlOn)
        {
            SelectUnitControl();
            UnitMoveControl();
        }
    }

    private void SelectUnitControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startLocation = cursorLocation();
            _selectionBox.SetActive(true);
            _selectionBox.transform.position = _startLocation;
        }

        if (Input.GetMouseButton(0))
        {
            _selectionBox.transform.localScale = cursorLocation() - _startLocation;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //쉬프트 안누르고 있으면 선택 해제 안함
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                foreach (GameObject selectedUnit in _selectedUnitList)
                {
                    selectedUnit.GetComponent<UnitStats>().setSelectionCircleState(false);
                }

                _selectedUnitList.Clear();
            }

            _selectionBox.SetActive(false);
            float3 endLocation = cursorLocation();

            //오버랩박스 크기 만듬
            float2 centerOfBox = new float2((endLocation.x + _startLocation.x) / 2f, (endLocation.y + _startLocation.y) / 2f);
            float2 sizeOfBox;

            if (math.distance(endLocation, _startLocation) <= clickThreshold)
            {
                // 클릭싸이즈면 0.2*0.2 크기 박스로 생성 및 한마리만 잡기 설정
                sizeOfBox = new float2(clickThreshold, clickThreshold);
                _selectOneOnly = true;
            }
            else
            {
                // 클릭이 아닐경우 평범하게 생성
                sizeOfBox = new float2(math.abs(endLocation.x - _startLocation.x), math.abs(endLocation.y - _startLocation.y));
                _selectOneOnly = false;
            }
            _selectedUnitCount = 0;

            Collider2D[] thingsInSelcetion = Physics2D.OverlapBoxAll(centerOfBox, sizeOfBox, 0f);
            foreach (Collider2D thing in thingsInSelcetion)
            {
                UnitStats tempStats = thing.GetComponent<UnitStats>();

                if (tempStats != null && tempStats.playerOwned)
                {
                    tempStats.setSelectionCircleState(true);
                    _selectedUnitList.Add(thing.gameObject);
                    _selectedUnitCount++;

                    if (_selectOneOnly && _selectedUnitCount == 1)
                    {
                        break;
                    }
                }
            }
        }
    }

    private void UnitMoveControl()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Global.UIManager.PushNotiMsg("Vow" , 2.0f);

            foreach (GameObject selectedUnit in _selectedUnitList)
            {
                selectedUnit.GetComponent<UnitStats>().MoveToTarget(cursorLocation());
            }
        }
    }

    private float3 cursorLocation()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
