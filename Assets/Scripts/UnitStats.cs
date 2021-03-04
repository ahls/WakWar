using UnityEngine;
﻿using System.Collections;
using UnityEngine.UI;
public class UnitStats : MonoBehaviour
{
    #region 변수
    public int healthMax => _healthMax;
    private int _healthMax = 10;
    public int moveSpeed { get; set; } = 10;
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

        playerUnitInit("test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playerUnitInit(string PlayerName)
    {
        playerOwned = true;
        selectionCircle.SetActive(false);
        PlayerNameText.text = PlayerName;
    }

    public void unitInit()
    {

    }

    public void MoveToTarget(Vector3 target)
    {
        _targetPos = new Vector3(target.x, target.y , 0f);
        _direction = _targetPos - this.transform.position;
        _direction = _direction.normalized;

        var distance = Vector2.Distance(this.transform.position, _targetPos);

        if (_isMoving)
        {
            StopCoroutine(_moveCoroutine);
        }
        
        _moveCoroutine = Move();
        StartCoroutine(_moveCoroutine);
    }

    private IEnumerator Move()
    {
        _isMoving = true;

        while (Vector2.Distance(this.transform.position, _targetPos) > 0.2f)
        {
            _rigid.MovePosition(Vector3.Lerp(this.transform.position, _targetPos, moveSpeed * Time.deltaTime));

            yield return null;
        }

        yield return null;

        _moveTime = 0;
        _targetPos = Vector3.zero;
        _direction = Vector3.zero;
        _isMoving = false;
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
