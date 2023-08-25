using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 7;

    public Vector2 Rotation => _rotation;
    public float Pinch => _pinch;

    public bool IsRotating => _isRotating;
    public bool IsPinched => _isPinched;

    private bool _isRotating;
    private bool _isPinched;

    private Vector2 _rotation;
    private float _pinch;

    private float _lastDistanceBetweenTouches;


#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    private void Awake()
    {
        _sensitivity *= 100;
    }
#endif

    private void Update()
    {
        HandleRotation();
        HandlePinch();
    }

    private void HandleRotation()
    {
        _isRotating = false;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetMouseButton(0))
        {
            _isRotating = true;
            var horizontal = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
            var vertical = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

            _rotation = new Vector2(horizontal, vertical);
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                _isRotating = true;

                var horizontal = touch.deltaPosition.x * _sensitivity * Time.deltaTime;
                var vertical = touch.deltaPosition.y * _sensitivity * Time.deltaTime;

                _rotation = new Vector2(horizontal, vertical);
            }
        }
#endif
    }

    private void HandlePinch()
    {
        _pinch = 0;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        _pinch = (Input.GetAxis("Mouse ScrollWheel")) * 0.5f + 1;
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 2)
        {
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began)
            {
                _lastDistanceBetweenTouches = Vector2.Distance(touch0.position, touch1.position);
            }

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float currentDistanceBetweenTouches = Vector3.Distance(touch0.position, touch1.position);

                _pinch = currentDistanceBetweenTouches / _lastDistanceBetweenTouches;

                _lastDistanceBetweenTouches = currentDistanceBetweenTouches;
            }
        }
#endif
        _isPinched = _pinch != 0;
    }
}
