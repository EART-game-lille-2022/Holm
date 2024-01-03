using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _cameraRoot;
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
            ResetTrackingValue();
        }

        _lastFrameTracking = _currentFrameTracking;
    }

    void ComputeDelta()
    {
        _deltaVector += _currentFrameTracking - _lastFrameTracking;
    }

    void SetCameraRotation(Vector3 value)
    {
        print("Cam set rotation : " + value);
        _cameraRoot.localRotation = Quaternion.Euler(value * _sensivity);
    }

    void ResetTrackingValue()
    {
        _currentFrameTracking = Vector2.zero;
        _lastFrameTracking = Vector2.zero;
    }

    //! Call by Panel Event
    public void OnPointerDown(PointerEventData eventData)
    {
        _isTracking = true;
        _lastFrameTracking = eventData.position;
    }

    //! Call by Panel Event
    public void OnPointerUp(PointerEventData eventData)
    {
        _isTracking = false;
    }

    //! Call by Panel Event
    public void OnPointerMove(PointerEventData eventData)
    {
        _inputVector = eventData.position;
    }
}
