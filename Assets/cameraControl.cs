using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    private const float SCROLL_MULTIPLIER= 0.1f;
    private const float SCROLL_SPEED = 0.05f;
    private const float LERP = 0.2f;
    private const float EDGE_SIZE = 30f;
    private float _targetSize =2 ;
    private Vector2 _targetLocaiton;
    private Vector2 _screenVector;
    private Camera _camera;
    [SerializeField] private Transform _leftBottomCorner;
    [SerializeField] private Transform _rightTopCorner;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        _screenVector = new Vector2(Screen.width / 2, Screen.height / 2);
        _targetLocaiton = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        zoomIn(mousePosition);
        cameraScrolling(mousePosition);
    }
    

    private void zoomIn(Vector2 mousePos)
    {
        float scrollValue = Input.mouseScrollDelta.y * SCROLL_MULTIPLIER;
        if (scrollValue != 0)
        {
            _targetSize = Mathf.Clamp(_targetSize + scrollValue, 1, 2);
            if (scrollValue < 0 && _targetSize > 1f)
            {//줌인할때 마우스 중심으로 줌인
                _targetLocaiton = _camera.ScreenToWorldPoint(mousePos);
            }
        }
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetSize, LERP);
    }
    private void cameraScrolling(Vector2 mousePos)
    {
        //테두리 설정
        _rightTopCorner.position = new Vector2(1.778f, 0.999f) * (2 - _targetSize);
        _leftBottomCorner.position = -_rightTopCorner.position;

        if (mousePos.x > Screen.width - EDGE_SIZE || Input.GetKey(KeyCode.RightArrow))
        {
            _targetLocaiton.x += SCROLL_SPEED;
        }
        else if (mousePos.x < EDGE_SIZE || Input.GetKey(KeyCode.LeftArrow))
        {
            _targetLocaiton.x -= SCROLL_SPEED;
        }
        if (mousePos.y > Screen.height - EDGE_SIZE || Input.GetKey(KeyCode.UpArrow))
        {
            _targetLocaiton.y += SCROLL_SPEED;
        }
        else if (mousePos.y < EDGE_SIZE || Input.GetKey(KeyCode.DownArrow))
        {
            _targetLocaiton.y -= SCROLL_SPEED;
        }
        _targetLocaiton.x = Mathf.Clamp(_targetLocaiton.x, _leftBottomCorner.position.x, _rightTopCorner.position.x);
        _targetLocaiton.y = Mathf.Clamp(_targetLocaiton.y, _leftBottomCorner.position.y, _rightTopCorner.position.y);

        transform.position = Vector3.Lerp(transform.position, _targetLocaiton, LERP);
    }
}
