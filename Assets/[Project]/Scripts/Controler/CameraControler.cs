using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _sensivity = 1;
    private Vector2 _lastFrameTracking;
    private Vector2 _currentFrameTracking;
    private Vector2 _deltaVector;
    private bool _isTracking;
    private Vector2 _inputVector;

    void Update()
    {
        if(_isTracking)
        {
            _currentFrameTracking = _inputVector;
            ComputeDelta();
            SetCameraRotation(new Vector3(-_deltaVector.y, _deltaVector.x, 0));
        }
        else
        {
            ResetValue();
        }

        _lastFrameTracking = _currentFrameTracking;
    }

    void SetCameraRotation(Vector3 value)
    {
        _camera.rotation = Quaternion.Euler(value * _sensivity);
    }

    void ComputeDelta()
    {
        _deltaVector += _currentFrameTracking - _lastFrameTracking;
    }

    void ResetValue()
    {
        _currentFrameTracking = Vector2.zero;
        _lastFrameTracking = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isTracking = true;
        _lastFrameTracking = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isTracking = false;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _inputVector = eventData.position;
    }
}
