using UnityEngine;
﻿using System.Collections;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    #region 변수


    //플레이어 소유주
    public bool Selectable { get; set; } = false;
    public faction ownedFaction { get; set; }

    //이동속도
    public float moveSpeed { get; set; } = 0.01f;
    

    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private GameObject selectionCircle;
    [SerializeField] private Text PlayerNameText;
    [SerializeField] private Transform DoNotRotate;
    private Animator _animator;
    
    private Vector3 _targetPos;
    private Vector3 _direction;
    private float _moveTime;

    private IEnumerator _moveCoroutine;
    public bool _isMoving = false;
    private int stuckCounter = 0;
    private float stuckDisplacement = 0.005f; 
    private Vector2 lastPosition;
    #endregion

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            Move();
            stuckCheck();
        }
       
    }

    public void playerUnitInit(string playerName)
    {
        Selectable = true;
        selectionCircle.SetActive(false);
        PlayerNameText.text = playerName;
        GetComponent<UnitCombat>().ownedFaction = ownedFaction;
    }

    public void MoveToTarget(Vector3 target)
    {
        _targetPos = new Vector3(target.x, target.y , 0f);
        _direction = _targetPos - this.transform.position;
        _direction = _direction.normalized;

        var distance = Vector2.Distance(this.transform.position, _targetPos);

        _isMoving = true;

        //애니메이션 부분
        _animator.SetBool("Move", true);
        transform.rotation = _direction.x < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        DoNotRotate.localEulerAngles=_direction.x < 0 ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);
    }

    private void Move()
    {
        if (Vector2.Distance(this.transform.position, _targetPos) > 0.1f)
        {
            _rigid.MovePosition(Vector3.MoveTowards(this.transform.position, _targetPos, moveSpeed));
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
        _moveTime = 0;
        _targetPos = Vector3.zero;
        _direction = Vector3.zero;
    }

    public void setSelectionCircleState(bool value)
    {
        if (!Selectable)
        {
            return;
        }

        selectionCircle.SetActive(value);
    }

    private void stuckCheck()
    {
        if ((lastPosition - (Vector2)transform.position).magnitude < stuckDisplacement)
        {
            if (stuckCounter == 10)
            {//안움직인지 10/50 초 가 지나면
                stuckCounter = 0;
                _animator.SetBool("Move", false);   
                _isMoving = false;
            }
            else
            {
                stuckCounter++;
            }
        }
        else
        {
            stuckCounter = 0;
        }
        lastPosition = transform.position;
    }
}
