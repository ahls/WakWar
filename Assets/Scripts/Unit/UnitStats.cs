using UnityEngine;
﻿using System.Collections;
using UnityEngine.UI;
using Pathfinding;
using Pathfinding.RVO;
using System.Collections.Generic;

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
    [SerializeField] private RVOController controller;
    public float MoveSpeed { get; set; } = 0.01f;
    private Rigidbody2D _rigid;
    private CircleCollider2D _collider;
    private Vector3 _targetPos;
    private Vector3 _direction;
    public bool IsMoving { get; set; } = false;

    //그래픽 관련
    [SerializeField] private Transform _rotatingPart;
    private Animator _animator;
    public float runningSpeed = 0.75f;//애니메이션 재생 속도 

    //예시 변수
    private bool _canSearchAgain = true;
    private float _nextRepath = 0;

    public float RepathRate = 1;
    public float MoveNextDist = 1;

    private Path _path = null;
    private List<Vector3> _vectorPath;
    private int wp;
    private Seeker _seeker;

    #endregion

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _unitCombat = GetComponent<UnitCombat>();
        _rigid = GetComponent<Rigidbody2D>();
        _seeker = GetComponent<Seeker>();

        if (controller.layer == RVOLayer.Ally)
        {
            _collider = GetComponent<CircleCollider2D>();
        }
    }

    private void Start()
    {/*
        if (!IngameManager.UnitManager.AllPlayerUnits.Contains(this.gameObject))
        {
            IngameManager.UnitManager.AllPlayerUnits.Add(this.gameObject);
        }*/
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
        if (_unitCombat.IsDead)
        {
            return;
        }

        _targetPos = target;
        controller.SetTarget(target, 0.5f, 0.5f);

        IsMoving = true;
        _unitCombat.ActionStat = UnitCombat.ActionStats.Move;

        if (removeCurrentTarget)
        {
            _unitCombat.AttackTarget = null;
        }

        //애니메이션 부분
        _animator.SetBool("Move", true);
        _animator.speed = runningSpeed;


        if (controller.layer == RVOLayer.Ally)
        {
            _collider.radius = 0.2f;
            _collider.isTrigger = true;
            controller.layer = RVOLayer.MovingAlly;
            controller.collidesWith = (RVOLayer)(-1) ^ RVOLayer.Ally;
        }

        RecalculatePath();
    }

    public void SetMoveToTarget(Vector2 target)
    {
        Debug.LogError("SEtMoveToTarget Implenemnt 안됌");
    }

    public void StopMoving()
    {
        IsMoving = false;
        _animator.SetBool("Move", false);
        ResetTarget();
        //Debug.Log(controller);
        if (controller.layer == RVOLayer.MovingAlly)
        {
            _collider.isTrigger = false;
            controller.layer = RVOLayer.Ally;
            controller.collidesWith = (RVOLayer)(-1) ^ RVOLayer.MovingAlly;

            StartCoroutine(ColliderSizeUP());
        }
    }

    private IEnumerator ColliderSizeUP()
    {
        _collider.radius = 0;
        while (true)
        {
            _collider.radius += 0.01f;

            if (_collider.radius >= 0.2f)
            {
                _collider.radius = 0.2f;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        _collider.radius = 0.2f;
    }

    private void Move()
    {
        if (_unitCombat.IsDead)
        {
            return;
        }

        if (Time.time >= _nextRepath && _canSearchAgain)
        {
            RecalculatePath();
        }

        Vector3 pos = transform.position;

        if (_vectorPath != null && _vectorPath.Count != 0)
        {
            while ((controller.To2D(pos - _vectorPath[wp]).sqrMagnitude < MoveNextDist * MoveNextDist && wp != _vectorPath.Count - 1) || wp == 0)
            {
                wp++;
            }

            // Current path segment goes from vectorPath[wp-1] to vectorPath[wp]
            // We want to find the point on that segment that is 'moveNextDist' from our current position.
            // This can be visualized as finding the intersection of a circle with radius 'moveNextDist'
            // centered at our current position with that segment.
            var p1 = _vectorPath[wp - 1];
            var p2 = _vectorPath[wp];

            // Calculate the intersection with the circle. This involves some math.
            var t = VectorMath.LineCircleIntersectionFactor(controller.To2D(transform.position), controller.To2D(p1), controller.To2D(p2), MoveNextDist);
            // Clamp to a point on the segment
            t = Mathf.Clamp01(t);
            Vector3 waypoint = Vector3.Lerp(p1, p2, t);

            // Calculate distance to the end of the path
            float remainingDistance = controller.To2D(waypoint - pos).magnitude + controller.To2D(waypoint - p2).magnitude;
            for (int i = wp; i < _vectorPath.Count - 1; i++) remainingDistance += controller.To2D(_vectorPath[i + 1] - _vectorPath[i]).magnitude;

            // Set the target to a point in the direction of the current waypoint at a distance
            // equal to the remaining distance along the path. Since the rvo agent assumes that
            // it should stop when it reaches the target point, this will produce good avoidance
            // behavior near the end of the path. When not close to the end point it will act just
            // as being commanded to move in a particular direction, not toward a particular point
            var rvoTarget = (waypoint - pos).normalized * remainingDistance + pos;
            // When within [slowdownDistance] units from the target, use a progressively lower speed
            var desiredSpeed = Mathf.Clamp01(remainingDistance / 0.5f) * 0.5f;
            Debug.DrawLine(transform.position, waypoint, Color.red);
            controller.SetTarget(rvoTarget, desiredSpeed, 0.5f);
        }
        else
        {
            // Stand still
            controller.SetTarget(pos, 0.5f, 0.5f);
        }

        // Get a processed movement delta from the rvo controller and move the character.
        // This is based on information from earlier frames.
        var movementDelta = controller.CalculateMovementDelta(Time.deltaTime);
        pos += movementDelta;

        transform.position = pos;

        RotateDirection(movementDelta.x);

        if (Vector2.Distance(this.transform.position, _targetPos) < 0.2f)
        {
            StopMoving();
        }
    }

    public void RecalculatePath()
    {
        _canSearchAgain = false;
        _nextRepath = Time.time + RepathRate * (Random.value + 0.5f);
        _seeker.StartPath(transform.position, _targetPos, OnPathComplete);
    }

    public void OnPathComplete(Path _p)
    {
        ABPath p = _p as ABPath;

        _canSearchAgain = true;

        if (_path != null) _path.Release(this);
        _path = p;
        p.Claim(this);

        if (p.error)
        {
            wp = 0;
            _vectorPath = null;
            return;
        }


        Vector3 p1 = p.originalStartPoint;
        Vector3 p2 = transform.position;
        p1.y = p2.y;
        float d = (p2 - p1).magnitude;
        wp = 0;

        _vectorPath = p.vectorPath;
        Vector3 waypoint;

        if (MoveNextDist > 0)
        {
            for (float t = 0; t <= d; t += MoveNextDist * 0.6f)
            {
                wp--;
                Vector3 pos = p1 + (p2 - p1) * t;

                do
                {
                    wp++;
                    waypoint = _vectorPath[wp];
                } while (controller.To2D(pos - waypoint).sqrMagnitude < MoveNextDist * MoveNextDist && wp != _vectorPath.Count - 1);
            }
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
    public void DisableMovement()
    {
        MoveToTarget(this.transform.position);
        controller.enabled = false;
    }
}
