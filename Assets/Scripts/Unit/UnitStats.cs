using UnityEngine;
﻿using System.Collections;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    #region 변수


    //플레이어 소유주
    public bool Selectable { get; set; } = false;
    public Faction OwnedFaction { get; set; }
    private UnitCombat _unitCombat;
    //이동속도
    public float MoveSpeed { get; set; } = 0.01f;
    

    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private GameObject _selectionCircle;
    [SerializeField] private Text _playerNameText;
    [SerializeField] private Transform _rotatingPart;
    private Animator _animator;
    
    private Vector3 _targetPos;
    private Vector3 _direction;
    //private float _moveTime;

    //private IEnumerator _moveCoroutine;
    public bool _isMoving = false;
    private int _stuckCounter = 0;
    private const float STUCK_DISPLACEMENT = 0.005f; 
    private Vector2 _lastPosition;
    #endregion

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _unitCombat = GetComponent<UnitCombat>();
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            Move();
            StuckCheck();
        }
       
    }

    public void PlayerUnitInit(string playerName)
    {
        Selectable = true;
        _selectionCircle.SetActive(false);
        _playerNameText.text = playerName;
        GetComponent<UnitCombat>().OwnedFaction = OwnedFaction;
    }

    public void MoveToTarget(Vector2 target,bool removeCurrentTarget = true)
    {
        _targetPos = target;
        _direction = (Vector2)(_targetPos - transform.position);
        _direction = _direction.normalized;
        var distance = Vector2.Distance(this.transform.position, _targetPos);
        _isMoving = true;
        _unitCombat.ActionStat = UnitCombat.ActionStats.Move;
        if(removeCurrentTarget)
        {
            _unitCombat.AttackTarget = null;
        }

        //애니메이션 부분
        _animator.SetBool("Move", true);
        _animator.speed = MoveSpeed * 100f;
        RotateDirection(_direction.x > 0);
    }

    private void Move()
    {
        if (Vector2.Distance(this.transform.position, _targetPos) > 0.001f)
        {
            _rigid.MovePosition(Vector2.MoveTowards(this.transform.position, _targetPos, MoveSpeed));
        }
        else
        {
            _animator.SetBool("Move", false);
            _isMoving = false;

            ResetTarget();
        }
    }

    private void ResetTarget()
    {
        //_moveTime = 0;
        _targetPos = Vector3.zero;
        _direction = Vector3.zero;
    }

    public void SetSelectionCircleState(bool value)
    {
        if (!Selectable)
        {
            return;
        }

        _selectionCircle.SetActive(value);
    }

    private void StuckCheck()
    {
        if ((_lastPosition - (Vector2)transform.position).magnitude < STUCK_DISPLACEMENT && _unitCombat.ActionStat == UnitCombat.ActionStats.Move)
        {
            if (_stuckCounter == 10)
            {//안움직인지 10/50 초 가 지나면
                _stuckCounter = 0;
                _animator.SetBool("Move", false);   
                _isMoving = false;
                _unitCombat.ActionStat = UnitCombat.ActionStats.Idle;
            }
            else
            {
                _stuckCounter++;
            }
        }
        else
        {
            _stuckCounter = 0;
        }
        _lastPosition = transform.position;
    }
    public void RotateDirection(bool facingRight)
    {
        //Debug.Log("rotating is being called" + facingRight+ " for " + gameObject.name);
        float currentZ = _rotatingPart.localEulerAngles.z;
        _rotatingPart.localEulerAngles = facingRight ? new Vector3(0, 180, currentZ) : new Vector3(0, 0, currentZ);
    }
}
