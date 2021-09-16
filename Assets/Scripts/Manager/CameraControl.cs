using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public bool CameraLocked = false;
    private const float SCROLL_MULTIPLIER= 0.1f;
    private const float SCROLL_SPEED = 0.05f;
    private const float LERP_SPEED = 0.2f;
    private const float EDGE_SIZE = 10f;
    private float _targetSize =2 ;
    private Vector2 _targetLocaiton;
    private Vector2 _screenVector;
    private Camera _camera;
    private float _screenRatio;
    private float _maxX, _maxY, _minX, _minY;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _screenVector = new Vector2(Screen.width / 2, Screen.height / 2);
        _targetLocaiton = Vector2.zero;
        _screenRatio = Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraLocked)
            return;
        Vector2 mousePosition = Input.mousePosition;

        ZoomIn(mousePosition);
        CameraScrolling(mousePosition);
    }

    private void ZoomIn(Vector2 mousePos)
    {
        float scrollValue = Input.mouseScrollDelta.y * SCROLL_MULTIPLIER;
        if (scrollValue != 0)
        {
            _targetSize = Mathf.Clamp(_targetSize - scrollValue, 1, 2);
            if (scrollValue > 0 && _targetSize > 1f)
            {//줌인할때 마우스 중심으로 줌인

                _targetLocaiton = Vector2.MoveTowards(_targetLocaiton,_camera.ScreenToWorldPoint(mousePos),(_targetSize/4));
            }
        }
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetSize, LERP_SPEED);
    }
    private void OnLevelWasLoaded(int level)
    {
        _camera = Camera.main;
    }
    private void CameraScrolling(Vector2 mousePos)
    {
        //테두리 설정
        _maxX = 1.778f * (2 - _targetSize);
        _minX = -_maxX;
        _maxY = 0.999f * (2 - _targetSize);
        _minY = -_maxY;

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
        _targetLocaiton.x = Mathf.Clamp(_targetLocaiton.x, _minX, _maxX);
        _targetLocaiton.y = Mathf.Clamp(_targetLocaiton.y, _minY, _maxY);

        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _targetLocaiton, LERP_SPEED);
    }
}
