using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasite_Behaviour : MonoBehaviour
{
    private const float INFECT_DURATION = 10f;
    private const float INFECT_RADIUS = 0.02f;
    private const float INFECT_ALLOWED_HEIGHT = 0.2f;
    private const float INIT_VERTICAL_FORCE = 0.1f;
    private const float INIT_HORIZONTAL_FORCE = 0.02f;
    private const float BASE_X = 0.171f;
    private const float BASE_Y = 0.126f;
    private const float GRAVITY = 0.005f;
    [SerializeField] private GameObject _bone;
    [SerializeField] private Animator _animator;
    private Collider2D _collider;
    private float _height;
    private float _currentVelocity;
    private bool _jumping;
    private Vector3 _jumpDirection;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider2D>();

    }

    private void FixedUpdate()
    {
        if(_jumping)
        {
            if(_height >= INFECT_ALLOWED_HEIGHT)
            {
                //감염 가능 높이보다 높거나 같을경우, 근처를 확인해서 감염 가능한지 확인
                CheckForUnits();
            }
            if(_height <= 0)
            {//착지
                _collider.enabled = true;
                _jumping = false;
                _height = 0;
                _bone.transform.localPosition = new Vector2(BASE_X,BASE_Y);
                _animator.SetTrigger("land");
            }
            else
            { //감염시킬수 없거나, 착지하지 않은경우, 점프 계속 진행
                transform.position += _jumpDirection;
                _height += _currentVelocity;
                _bone.transform.localPosition = new Vector3(BASE_X, _height + BASE_Y, 0);
                _currentVelocity -= GRAVITY;
              }
        }
    }
    private void CheckForUnits()
    {
        Collider2D[] unitsWithin = Physics2D.OverlapCircleAll(transform.position, INFECT_RADIUS);
        foreach(var unit in unitsWithin)
        {
            if( unit.transform != transform &&
                unit.GetComponent<UnitCombat>()?.OwnedFaction == Faction.Player)
            {
                Debug.Log("AHHH INFECTING");
                UnitCombat uc = unit.GetComponent<UnitCombat>();
                uc.ChangeFaction(Faction.Enemy);
                StartCoroutine(Disinfect(uc));
                _bone.SetActive(false);
                transform.position = new Vector2(11, 11);
                //GetComponent<UnitCombat>().Death();
            }
        }
    }
    private IEnumerator Disinfect(UnitCombat uc)
    {
        yield return new WaitForSeconds(INFECT_DURATION);

        uc.ChangeFaction(Faction.Player);
        GetComponent<UnitCombat>().Death();
    }
    public void jump()
    {
        _jumpDirection = ((Vector2)(GetComponent<UnitCombat>().AttackTarget.position - transform.position)).normalized * INIT_HORIZONTAL_FORCE;
        _jumping = true;
        _currentVelocity = INIT_VERTICAL_FORCE;
        _height =0.01f;
        _collider.enabled = false;
    }
}
