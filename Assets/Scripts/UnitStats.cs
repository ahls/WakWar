using UnityEngine;
﻿using System.Collections;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    #region 변수
    public int healthMax => _healthMax;
    private int _healthMax = 10;
    public float moveSpeed { get; set; } = 0.05f;
    public int attackSpeed { get; set; }
    public bool playerOwned { get; set; }

    private int healthCurrent;

    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject selectionCircle;
    [SerializeField] private Text PlayerNameText;

    private Vector3 _targetPos;
    private Vector3 _direction;
    private float _moveTime;

    private IEnumerator _moveCoroutine;
    private bool _isMoving = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        healthCurrent = healthMax;
        healthBar.maxValue = healthMax;
        healthBarUpdate();

        //테스트라인
        playerUnitInit("test");
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            Move();
        }
    }

    public void playerUnitInit(string PlayerName)
    {
        playerOwned = true;
        selectionCircle.SetActive(false);
        Debug.Log($"기존에 써져있던 이름: {PlayerNameText.text} ### 새로 전해받은 플레이어이름: {PlayerName}");
        PlayerNameText.text = PlayerName;
    }

    public void MoveToTarget(Vector3 target)
    {
        _targetPos = new Vector3(target.x, target.y , 0f);
        _direction = _targetPos - this.transform.position;
        _direction = _direction.normalized;

        var distance = Vector2.Distance(this.transform.position, _targetPos);

        _isMoving = true;
    }

    private void Move()
    {
        if (Vector2.Distance(this.transform.position, _targetPos) > 0.1f)
        {
            _rigid.MovePosition(Vector3.MoveTowards(this.transform.position, _targetPos, moveSpeed));
        }
        else
        {
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
        if (!playerOwned)
        {
            return;
        }

        selectionCircle.SetActive(value);
    }

    public void takeDamage(int damageAmount)
    {
        healthCurrent -= damageAmount;
        healthBarUpdate();
    }

    private void healthBarUpdate()
    {
        healthBar.value = healthCurrent;
    }
}
