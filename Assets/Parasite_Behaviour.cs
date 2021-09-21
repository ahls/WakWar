using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasite_Behaviour : MonoBehaviour
{
    private const float INFECT_ALLOWED_HEIGHT = 0.2f;
    private const float GRAVITY = 0.02f;
    private const float INIT_VERTICAL_FORCE = 0.1f;
    private const float INIT_HORIZONTAL_FORCE = 0.1f;
    [SerializeField] private GameObject _bone;
    private float _height;
    private float _currentVelocity;
    private bool _jumping;
    private Vector3 _jumpDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(_jumping)
        {
            if(_height >= INFECT_ALLOWED_HEIGHT)
            {

                //감염 가능 높이보다 높거나 같을경우, 근처를 확인해서 감염 가능한지 확인
            }
            else if(_height <= 0)
            {//착지
                _jumping = false;
                _height = 0;
                _bone.transform.position = Vector3.zero;
            }
            else
            { //감염시킬수 없거나, 착지하지 않은경우, 점프 계속 진행
                transform.position += _jumpDirection;
                _height += _currentVelocity;
                _bone.transform.position = new Vector3(0, _height,0);
                _currentVelocity -= GRAVITY;
              }
        }
    }
    public void jump()
    {
        _jumpDirection = ((Vector2)(GetComponent<UnitCombat>().AttackTarget.position - transform.position)).normalized * INIT_HORIZONTAL_FORCE;
        _jumping = true;
        _currentVelocity = INIT_VERTICAL_FORCE;
    }
}
