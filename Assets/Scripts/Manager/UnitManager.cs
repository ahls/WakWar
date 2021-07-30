using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitManager : MonoBehaviour
{
    #region 변수
    public bool ControlOn { get; set; } = true;
    private bool _attackMode = false;
    private bool _leftClicked, _rightClicked;
    public List<GameObject> AllPlayerUnits = new List<GameObject>();
    private List<GameObject> _selectedUnitList = new List<GameObject>();
    private List<List<GameObject>> _unitSquads = new List<List<GameObject>>(10);
    [SerializeField] private GameObject _selectionBox;
    [SerializeField] private Texture2D _attackCursor,_normalCursor, _aimingCursor;
    private Vector2 _startLocation;
    private short _selectedUnitCount = 0;       //유닛 선택이
    private bool _selectOneOnly = false;        //클릭인지
    private const float CLICK_THRESHOLD = 0.1f;  //확인할떄 쓰임
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        IngameManager.instance.SetUnitManager(this);
        Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
        for (int i = 0; i < 10; i++)
        {
            _unitSquads.Add(new List<GameObject>());
        }
    }

    private void Update()
    {
        if (ControlOn)
        {
            ClickDetection();
            SelectUnitControl();
            UnitMoveControl();
            SquadControl();
            AttackModeChecker();
            AttackOrder();
        }
    }

    private void SelectUnitControl()
    {
        if (_attackMode)
        {//공격모드 활서오하 되있으면 무시
            return;
        }
        if (_leftClicked)
        {
            _startLocation = CursorLocation();
            _selectionBox.SetActive(true);
            _selectionBox.transform.position = _startLocation;
            //쉬프트 안누르고 있으면 선택 해제 안함
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                foreach (GameObject selectedUnit in _selectedUnitList)
                {
                    selectedUnit.GetComponent<UnitStats>().SetSelectionCircleState(false);
                }

                _selectedUnitList.Clear();
            }

        }

        if (Input.GetMouseButton(0))
        {
            _selectionBox.transform.localScale = CursorLocation() - _startLocation;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _selectionBox.SetActive(false);
            Vector2 endLocation = CursorLocation();

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
        if (_rightClicked)
        {
            if (_attackMode)
            {//공격모드상태에서 우클릭 누르면 취소됨
                _attackMode = false;
                Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
                return;
            }

            int layerMask = 1 << LayerMask.NameToLayer("Enemy");

            var hitEnemy = Physics2D.Raycast(CursorLocation(), transform.forward, float.MaxValue, layerMask);
            if (hitEnemy && _selectedUnitList.Count > 0)
            {
                Debug.Log(hitEnemy.transform.name);

                foreach (var currentUnit in _selectedUnitList)
                {
                    UnitCombat currentUnitCombat = currentUnit.GetComponent<UnitCombat>();
                    currentUnitCombat.AttackTarget = hitEnemy.transform;
                    currentUnitCombat.MoveIntoRange();
                }

                return;
            }

            //Vector2 squadOffset = Vector3.zero;
            //var ang = 360f / (_selectedUnitList.Count - 1);
            //int currentUnitNumber = 0;

            GameObject leader = null;
            float minDistance = float.MaxValue;

            foreach (GameObject selectedUnit in _selectedUnitList)
            {
                var unitDistance = Vector2.Distance(selectedUnit.transform.position, CursorLocation());
                if (unitDistance < minDistance)
                {
                    leader = selectedUnit;
                    minDistance = unitDistance;
                }
            }

            //Debug.Log(leader.name);

            foreach (GameObject selectedUnit in _selectedUnitList)
            {
                //if (currentUnitNumber != 0)
                //{
                //    float angle = ang * currentUnitNumber * Mathf.Deg2Rad;
                //    squadOffset = new Vector2(Mathf.Cos(angle) * 0.2f, Mathf.Sin(angle) * 0.2f);
                //}

                selectedUnit.GetComponent<UnitStats>().MoveToTarget(CursorLocation(), true);
                selectedUnit.GetComponent<UnitCombat>().AttackGround = false;
                //currentUnitNumber++;
            }
        }
    }
    private void SquadControl()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown((KeyCode)i + 48))
            {
                if (Input.GetKey(KeyCode.LeftControl))
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


    private void AttackOrder()
    {
        if (_leftClicked && _attackMode)
        {
            if (_selectedUnitList.Count == 0) return; // 현재 선택된 유닛 없으면 리턴
            Vector2 cursorLoc = CursorLocation();
            Collider2D targetUnit = Physics2D.OverlapCircle(cursorLoc, 0.05f);
            if (targetUnit != null)
            {//범위에내 유닛이 있을경우 점사
                foreach (var currentUnit in _selectedUnitList)
                {
                    UnitCombat currentUnitCombat = currentUnit.GetComponent<UnitCombat>();
                    currentUnitCombat.AttackTarget = targetUnit.transform;
                    currentUnitCombat.MoveIntoRange();
                }
            }
            else
            {//범위내 없을경우 어택땅
                foreach (var currentUnit in _selectedUnitList)
                {
                    currentUnit.GetComponent<UnitStats>().MoveToTarget(cursorLoc,true);
                    currentUnit.GetComponent<UnitCombat>().AttackGround = true;
                }
            }
            Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
        }
        if(_attackMode && Input.GetMouseButtonUp(0))
        {
            _attackMode = false;
        }
    }
    private void AttackModeChecker()
    {
        if (Input.GetKeyDown(KeyCode.A) && _selectedUnitList.Count > 0)
        {
            _attackMode = true;
            //Cursor.SetCursor(_attackCursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        if (_attackMode)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Enemy");

            var hitEnemy = Physics2D.Raycast(CursorLocation(), transform.forward, float.MaxValue, layerMask);
            if (hitEnemy)
            {
                Cursor.SetCursor(_aimingCursor, Vector2.zero, CursorMode.Auto);
                return;
            }
            else
            {
                Cursor.SetCursor(_attackCursor, Vector2.zero, CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    private Vector2 CursorLocation()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void DeselectUnit(GameObject unitToDeselect)
    {
        if(_selectedUnitList.Contains(unitToDeselect))
        _selectedUnitList.Remove(unitToDeselect);
    }

    private void ClickDetection()
    {
        _leftClicked = Input.GetMouseButtonDown(0);
        _rightClicked = Input.GetMouseButtonDown(1);
        if(Input.GetKeyDown(KeyCode.S))
        {//스탑명령 여기에 추가
            _attackMode = false;
            foreach(var unit in _selectedUnitList)
            {
                unit.GetComponent<UnitStats>().MoveToTarget(unit.transform.position);
            }
        }
    }
    public List<GameObject> GetSelectedUnits()
    {
        return _selectedUnitList;
    }
    

    public void OnSceneChange()
    {
        foreach (var unit in AllPlayerUnits)
        {
            unit.SetActive(false);
        }
    }
    
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var unit in AllPlayerUnits)
        {
            unit.SetActive(false);
            unit.SetActive(true);
            unit.GetComponent<UnitCombat>().ChangeEquipAnimation();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
