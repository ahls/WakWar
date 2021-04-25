using UnityEngine;
﻿using System.Collections;
using UnityEngine.UI;
using Pathfinding;
public class UnitStats : MonoBehaviour
{
    #region 변수
    private UnitCombat _unitCombat;

    //선택관련
    public bool Selectable { get; set; } = false;
    public Faction OwnedFaction { get; set; }
    [SerializeField] private GameObject _selectionCircle;
    [SerializeField] private Text _playerNameText;



    //이동관련
    private AIPath _aiPath;
    private AIDestinationSetter _aiDestSetter;
    public float MoveSpeed { get; set; } = 0.01f;
    private Rigidbody2D _rigid;
    private Vector3 _targetPos;
    private Vector3 _direction;
    public bool _isMoving = false;
    
    //그래픽 관련
    [SerializeField] private Transform _rotatingPart;
    private Animator _animator;
    public float runningSpeed = 0.75f;//애니메이션 재생 속도 
    //이동 멈춤 관련
    //private int _stuckCounter = 0;
    //private const float STUCK_DISPLACEMENT = 0.0001f; 
    //private Vector2 _lastPosition;





    #endregion

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _unitCombat = GetComponent<UnitCombat>();
        _rigid = GetComponent<Rigidbody2D>();
        _aiPath = GetComponent<AIPath>();
        _aiDestSetter = GetComponent<AIDestinationSetter>();

    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            //Move();
            //StuckCheck();
            
            if (_aiPath.reachedDestination)
            {

                _animator.SetBool("Move", false);
                _isMoving = false;
            }
        }
       
    }

    public void PlayerUnitInit(string playerName)
    {
        Selectable = true;
        _selectionCircle.SetActive(false);
        _playerNameText.text = playerName;
        GetComponent<UnitCombat>().OwnedFaction = OwnedFaction;
    }
    /*
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
    }*/
    public void MoveToTarget(Vector2 target, bool removeCurrentTarget = true)
    {
        _aiPath.destination = target;
        _isMoving = true;
        _unitCombat.ActionStat = UnitCombat.ActionStats.Move;
        _aiPath.SearchPath();
        //애니메이션 부분
        _animator.SetBool("Move", true);
        _animator.speed = _aiPath.maxSpeed * runningSpeed;
        RotateDirection(_aiPath.destination.x - transform.position.x);
    }
    public void SetMoveToTarget(Vector2 target)
    {
        _aiPath.destination = target;
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
    /*
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
    */
    public void RotateDirection(float xVelocity)
    {
        var originalScale = _rotatingPart.localScale;


        if (xVelocity > 0)
        {
            float yScale = -Mathf.Abs(originalScale.y);
            _rotatingPart.localScale = new Vector3(1, yScale, 1);
        }
        else if(xVelocity < 0)
        {
            float yScale = Mathf.Abs(originalScale.y);
            _rotatingPart.localScale = new Vector3(1, yScale, 1);
        }

    }
}
