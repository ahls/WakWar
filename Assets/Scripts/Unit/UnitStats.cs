﻿using UnityEngine;
﻿using System.Collections;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    #region 변수


    //플레이어 소유주
    public bool playerOwned { get; set; } = false;
    public faction ownedFaction { get; set; } = faction.enemy;

    //이동속도
    public float moveSpeed { get; set; } = 0.05f;
    

    [SerializeField] private Rigidbody2D _rigid;
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

        //테스트라인
        if (playerOwned)
        {
            playerUnitInit("test");
        }
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
        GetComponent<UnitCombat>().ownedFaction = faction.player;
        selectionCircle.SetActive(false);
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


    public void equip()
    {

    }
}
