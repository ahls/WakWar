using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region 변수
    public bool ControlOn { get; set; } = true;
    private List<GameObject> _selectedUnitList = new List<GameObject>();
    private List<List<GameObject>> _unitSquads = new List<List<GameObject>>(10);
    [SerializeField] private GameObject _selectionBox;
    private float3 _startLocation;
    private short _selectedUnitCount = 0;       //유닛 선택이
    private bool _selectOneOnly = false;        //클릭인지
    private const float CLICK_THRESHOLD = 0.1f;  //확인할떄 쓰임
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        IngameManager.instance.SetUnitManager(this);

        for (int i = 0; i < 10; i++)
        {
            _unitSquads.Add(new List<GameObject>());
        }
    }

    private void Update()
    {
        if (ControlOn)
        {
            SelectUnitControl();
            UnitMoveControl();
            SquadControl();
        }
    }

    private void SelectUnitControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startLocation = CursorLocation();
            _selectionBox.SetActive(true);
            _selectionBox.transform.position = _startLocation;
        }

        if (Input.GetMouseButton(0))
        {
            _selectionBox.transform.localScale = CursorLocation() - _startLocation;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //쉬프트 안누르고 있으면 선택 해제 안함
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                foreach (GameObject selectedUnit in _selectedUnitList)
                {
                    selectedUnit.GetComponent<UnitStats>().SetSelectionCircleState(false);
                }

                _selectedUnitList.Clear();
            }

            _selectionBox.SetActive(false);
            float3 endLocation = CursorLocation();

            //오버랩박스 크기 만듬
            float2 centerOfBox = new float2((endLocation.x + _startLocation.x) / 2f, (endLocation.y + _startLocation.y) / 2f);
            float2 sizeOfBox;

            if (math.distance(endLocation, _startLocation) <= CLICK_THRESHOLD)
            {
                // 클릭싸이즈면 0.2*0.2 크기 박스로 생성 및 한마리만 잡기 설정
                sizeOfBox = new float2(CLICK_THRESHOLD, CLICK_THRESHOLD);
                _selectOneOnly = true;
            }
            else
            {
                // 클릭이 아닐경우 평범하게 생성
                sizeOfBox = new float2(math.abs(endLocation.x - _startLocation.x), math.abs(endLocation.y - _startLocation.y));
                _selectOneOnly = false;
            }
            _selectedUnitCount = 0;

            Collider2D[] UnitsInRange = Physics2D.OverlapBoxAll(centerOfBox, sizeOfBox, 0f);
            foreach (Collider2D selectingUnit in UnitsInRange)
            {
                UnitStats tempStats = selectingUnit.GetComponent<UnitStats>();

                if (tempStats != null && tempStats.Selectable)
                {
                    tempStats.SetSelectionCircleState(true);
                    _selectedUnitList.Add(selectingUnit.gameObject);
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
            foreach (GameObject selectedUnit in _selectedUnitList)
            {
                selectedUnit.GetComponent<UnitStats>().MoveToTarget(CursorLocation());
            }
        }
    }
    private void SquadControl()
    {
        for (int i = 0; i < 10; i++)
        {
            if(Input.GetKeyDown((KeyCode)i+48))
            {
                if(Input.GetKey(KeyCode.LeftControl))
                {//부대 지정
                    _unitSquads[i] = _selectedUnitList.ToList();
                    return;
                }
                //부대 선택
                foreach (var selectedUnit in _selectedUnitList)
                {
                    selectedUnit.GetComponent<UnitStats>().SetSelectionCircleState(false);
                }
                _selectedUnitList = _unitSquads[i].ToList();
                foreach (var selectedUnit in _selectedUnitList)
                {
                    if (selectedUnit.GetComponent<UnitStats>().Selectable)
                    {
                        selectedUnit.GetComponent<UnitStats>().SetSelectionCircleState(true);
                    }
                    else
                    {
                        DeselectUnit(selectedUnit);
                    }
                }
                return;
                
            }
        }
        
    }

    private float3 CursorLocation()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void DeselectUnit(GameObject unitToDeselect)
    {
        _selectedUnitList.Remove(unitToDeselect);
    }
}
