using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class CameraControler : MonoBehaviour
{
    [Header("Reference :")]
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [Space]
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private float _sensivity = 1;
    private Vector2 _lastFrameTracking;
    private Vector2 _currentFrameTracking;
    private Vector2 _deltaVector;
    private Vector2 _inputVector;
    private bool _isPlayerFlying;
    public bool IsPlayerflying
    {
        set
        {
            var thirdPerson = _virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            _isPlayerFlying = value;
            if(_isPlayerFlying)
            {
                thirdPerson.ShoulderOffset.y = .5f;
                Camera.main.transform.parent = transform.parent;
                Camera.main.GetComponent<CinemachineBrain>().enabled = false;
                _deltaVector = Vector2.zero;
            }
            else
            {
                thirdPerson.ShoulderOffset.y = 2;
                Camera.main.transform.parent = null;
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;
            }
        }
    }

    

    void Update()
    {
        if(_isPlayerFlying)
        {
            AerialControl();
            return;
        }

        GroundControl();
    }

    void AerialControl()
    {
        _cameraRoot.localRotation = Quaternion.Euler(Vector3.zero);
    }

    void GroundControl()
    {
        _currentFrameTracking = _inputVector;

        ComputeDelta();
        SetCameraRotation(new Vector3(-_deltaVector.y, _deltaVector.x, 0));

        _lastFrameTracking = _currentFrameTracking;
    }

    void ComputeDelta()
    {
        _deltaVector += _currentFrameTracking - _lastFrameTracking;
    }

    public void ComputeDeltaWithOrientation(Vector2 value, Vector2 lastframeValue)
    {
        print("Value : " + value + "  LastFrameValue : " + lastframeValue);
        _deltaVector += value - lastframeValue;
    }

    void SetCameraRotation(Vector3 value)
    {
        // print("Cam set rotation : " + value);
        _cameraRoot.localRotation = Quaternion.Euler(value * _sensivity);
    }

    public void ResetDelta()
    {
        _deltaVector = Vector2.zero;
        _cameraRoot.localRotation = Quaternion.Euler(Vector2.zero);
    }

    //! Call by Panel Event
    public void OnPointerDown(PointerEventData eventData)
    {
        _lastFrameTracking = eventData.position;
    }

    //! Call by Panel Event
    public void OnPointerMove(PointerEventData eventData)
    {
        _inputVector = eventData.position;
    }
}
