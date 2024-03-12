using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private float _sensivity = 1;
    [Space]
    [SerializeField] private float _xUpCap;
    [SerializeField] private float _xDownCap;
    [SerializeField] private float _currentX;


    [Header("Ground Parametre :")]
    [SerializeField] private float _groundShoulderOffset;

    [Header("Fly Parametre :")]
    [SerializeField] private float _flyShoulderOffset;
    [SerializeField] private float _cameraTrakingSpeed = 3f;

    private Vector3 _inputAxis;
    private Vector3 _inputTargetEulerAngles;
    private Vector3 _inputTargetOrientation;
    private Transform _startTarget;
    private Transform _startLookAt;
    private PlayerState _currentPlayerState = PlayerState.None;
    private float _velocityMag;
    private Rigidbody _rigidbody;
    private CinemachineVirtualCamera _virtualCam;
    private Transform _cameraTarget;
    private CameraEffect _cameraEffect;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").transform;
        _virtualCam = GameObject.FindGameObjectWithTag("CameraSetup").GetComponentInChildren<CinemachineVirtualCamera>();
        _cameraEffect = GameObject.FindGameObjectWithTag("CameraSetup").GetComponentInChildren<CameraEffect>();
        _startTarget = _cameraTarget;
        _startLookAt = _virtualCam.LookAt;
    }


    private void Update()
    {
        if (!GameManager.instance.CanPlayerMove)
            return;

        _velocityMag = _rigidbody.velocity.magnitude;
        _cameraEffect.SetCameraFovWithVelocity(_velocityMag);
        _cameraEffect.ShakeCameraWithVelocity(_velocityMag);

        if (_currentPlayerState == PlayerState.Flying)
        {
            _cameraTarget.transform.forward = Vector3.Slerp(_cameraTarget.transform.forward, transform.up, Time.deltaTime * _cameraTrakingSpeed);
            return;
        }

        ComputeInputValue();
        _cameraTarget.transform.eulerAngles = _inputTargetEulerAngles;

        //! Reset Z axis
        _cameraTarget.transform.eulerAngles =
        new Vector3(_cameraTarget.transform.eulerAngles.x, _cameraTarget.transform.eulerAngles.y, 0);
    }

    private void ComputeInputValue()
    {
        _currentX += _inputAxis.x * Time.deltaTime * _sensivity;
        _currentX = Mathf.Clamp(_currentX, _xDownCap, _xUpCap);

        _inputTargetEulerAngles.x = _currentX;
        _inputTargetEulerAngles.y += _inputAxis.y * Time.deltaTime * _sensivity;

        //! Convert angle to direction
        Quaternion a = Quaternion.Euler(_inputTargetEulerAngles);
        _inputTargetOrientation = a * Vector3.forward;
    }

    public void SetCameraParameter(PlayerState playerState)
    {
        Cinemachine3rdPersonFollow cm = _virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        _currentPlayerState = playerState;

        Vector3 targetStartOrientation = _cameraTarget.transform.forward;
        Vector3 targetEndOrientation = Vector3.zero;

        float shoulderOffsetStart = cm.ShoulderOffset.y;
        float shoulderOffsetEnd = 0f;


        DOTween.To((time) =>
        {
            switch (_currentPlayerState)
            {
                case PlayerState.Grounded:
                    shoulderOffsetEnd = _groundShoulderOffset;
                    break;

                case PlayerState.Flying:
                    shoulderOffsetEnd = _flyShoulderOffset;
                    break;
            }

            cm.ShoulderOffset = new Vector3(0, Mathf.Lerp(shoulderOffsetStart, shoulderOffsetEnd, time), 0);
            targetEndOrientation = _currentPlayerState == PlayerState.Grounded ? _inputTargetOrientation : transform.up;
            _cameraTarget.transform.forward = Vector3.Slerp(targetStartOrientation, targetEndOrientation, time);
        }, 0, 1, 1)
        .SetUpdate(UpdateType.Late, false)
        .OnComplete(() => _currentPlayerState = playerState);
    }



    public void SetCameraTaret(Transform newTarget, Transform newLookAt)
    {
        _virtualCam.Follow = newTarget;
        _virtualCam.LookAt = newLookAt;
    }

    public void ResetCameraTarget()
    {
        _virtualCam.Follow = _startTarget;
        _virtualCam.LookAt = _startLookAt;
    }

    private void OnLook(InputValue value)
    {
        if (_currentPlayerState == PlayerState.Flying)
        {
            _inputAxis = Vector2.zero;
            return;
        }

        _inputAxis.x = -value.Get<Vector2>().y;
        _inputAxis.y = value.Get<Vector2>().x;
    }
}
