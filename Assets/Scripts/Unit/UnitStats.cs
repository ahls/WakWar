using UnityEngine;
﻿using System.Collections;
using UnityEngine.UI;
using Pathfinding;
using Pathfinding.RVO;

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
    //private AIPath _aiPath;
    //private AIDestinationSetter _aiDestSetter;
    [SerializeField] private RVOController controller; 
    public float MoveSpeed { get; set; } = 0.01f;
    private Rigidbody2D _rigid;
    private Vector3 _targetPos;
    private Vector3 _direction;
    public bool IsMoving { get; set; } = false;
    
    //그래픽 관련
    [SerializeField] private Transform _rotatingPart;
    private Animator _animator;
    public float runningSpeed = 0.75f;//애니메이션 재생 속도 

    #endregion

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _unitCombat = GetComponent<UnitCombat>();
        _rigid = GetComponent<Rigidbody2D>();
        //_aiPath = GetComponent<AIPath>();
        //_aiDestSetter = GetComponent<AIDestinationSetter>();
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            Move();
        }  
    }

    public void PlayerUnitInit(string playerName)
    {
        Selectable = true;
        _selectionCircle.SetActive(false);
        _playerNameText.text = playerName;
        GetComponent<UnitCombat>().OwnedFaction = OwnedFaction;
    }
  
    public void MoveToTarget(Vector2 target, bool removeCurrentTarget = true)
    {
        _targetPos = target;
        controller.SetTarget(target, 0.5f, 0.5f);
        //_aiPath.destination = target;
        IsMoving = true;
        _unitCombat.ActionStat = UnitCombat.ActionStats.Move;
        //_aiPath.SearchPath();
        if (removeCurrentTarget)
        {
            _unitCombat.AttackTarget = null;
        }

        //애니메이션 부분
        _animator.SetBool("Move", true);
        _animator.speed = 0.5f * runningSpeed;

        //_animator.speed = _aiPath.maxSpeed * runningSpeed;
        //RotateDirection(_aiPath.destination.x - transform.position.x);
        _rigid.mass = 10;
    }

    public void SetMoveToTarget(Vector2 target)
    {
        //_aiPath.destination = target;
    }

    public void StopMoving()
    {
        IsMoving = false;
        _animator.SetBool("Move", false);
        ResetTarget();
        //_aiPath.destination = transform.position;
        //_aiPath.SearchPath();

        _rigid.mass = 1;
    }

    private void Move()
    {
        if (_targetPos == Vector3.zero)
        {
            return;
        }

        var targetDelta = controller.CalculateMovementDelta(transform.position, 10.0f * Time.deltaTime);
        var moveValue = (targetDelta.normalized * 0.5f * Time.deltaTime);

        //this.transform.position += moveValue;
        _rigid.MovePosition(this.transform.position + moveValue);

        int layerMask = 1 << LayerMask.NameToLayer("Ally");

        var hit = Physics2D.Raycast(_targetPos, transform.forward, float.MaxValue, layerMask);
        Debug.DrawRay(_targetPos, transform.forward * 15f, Color.red);

        if (Vector2.Distance(this.transform.position, _targetPos) < 0.2f)
        {
            StopMoving();
        }
    }

    public void Shoved(Vector3 shovedPower)
    {
        _rigid.AddForce(shovedPower);
    }

    private void ResetTarget()
    {
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
