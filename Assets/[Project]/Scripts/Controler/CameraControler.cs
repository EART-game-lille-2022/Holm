using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private float _sensivity = 1;
    [Space]
    [SerializeField] private float _xUpCap;
    [SerializeField] private float _xDownCap;
    [SerializeField] private float _currentX;
    [SerializeField] private Transform _dynamicCameraTarget;
    [SerializeField] private Transform _inGameCameraTarget;


    [Header("Ground Parametre :")]
    [SerializeField] private float _groundShoulderOffset;

    [Header("Fly Parametre :")]
    [SerializeField] private float _flyShoulderOffset;
    [SerializeField] private float _cameraTrakingSpeed = 3f;

    private Vector3 _inputAxis;
    private Vector3 _inputTargetEulerAngles;
    private Vector3 _inputTargetOrientation;
    private PlayerState _currentPlayerState = PlayerState.None;
    private float _velocityMag;
    private Rigidbody _playerRigidbody;
    private CinemachineVirtualCamera _virtualCam;
    private Transform _cameraTarget;
    private CameraEffect _cameraEffect;
    private bool _playCameraEffect = true;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();

        _cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").transform;
        _virtualCam = GameObject.FindGameObjectWithTag("CameraSetup").GetComponentInChildren<CinemachineVirtualCamera>();
        _cameraEffect = GameObject.FindGameObjectWithTag("CameraSetup").GetComponentInChildren<CameraEffect>();

        ResetCameraTarget();
    }

    private void Update()
    {
        _velocityMag = _playerRigidbody.velocity.magnitude;
        _cameraEffect.SetCameraFovWithVelocity(_velocityMag);
        _cameraEffect.ShakeCameraWithVelocity(_velocityMag);

        if (!GameManager.instance.CanPlayerMove)
            return;

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

    public void EnableCameraEffect(bool value)
    {
        _playCameraEffect = value;
    }

    public void SetCameraTaret(Transform newTarget)
    {
        _virtualCam.Follow = newTarget;
        _virtualCam.LookAt = newTarget;
    }

    public void LerpCameraToPlayer(Transform menuTrarget, Action toDoAfter = null)
    {
        _dynamicCameraTarget.position = menuTrarget.position;
        _dynamicCameraTarget.rotation = menuTrarget.rotation;

        _virtualCam.Follow = _dynamicCameraTarget;
        _virtualCam.LookAt = _dynamicCameraTarget;

        DOTween.To((time) =>
        {
            print("YAAAAAAAAAAAA");
            _dynamicCameraTarget.position = Vector3.Lerp(menuTrarget.position, _inGameCameraTarget.position, time);
            _dynamicCameraTarget.rotation = Quaternion.Lerp(menuTrarget.rotation, _inGameCameraTarget.rotation, time);
        }, 0, 1, 5)
        .OnComplete(() =>
        {
            ResetCameraTarget();
            if (toDoAfter != null)
                toDoAfter();
        });
    }

    public void ResetCameraTarget()
    {
        _cameraTarget = _inGameCameraTarget;

        _virtualCam.Follow = _cameraTarget;
        _virtualCam.LookAt = _cameraTarget;
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
