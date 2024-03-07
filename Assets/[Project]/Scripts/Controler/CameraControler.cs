using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _sensivity = 1;
    [Space]
    [SerializeField] private float _xUpCap;
    [SerializeField] private float _xDownCap;
    [SerializeField] private float _currentX;
    private Vector3 _inputAxis;
    private Transform _startTarget;
    private Transform _startLookAt;
    private PlayerState _currentPlayerState = PlayerState.None;

    private void Start()
    {
        _startTarget = _cameraTarget;
        _startLookAt = _virtualCam.LookAt;
    }

    private void Update()
    {
        //TODO screenShake pendant la chute
        if (!GameManager.instance.CanPlayerMove)
            return;

        if (_currentPlayerState == PlayerState.Flying)
        {
            print("fly : return");
            _cameraTarget.transform.forward = transform.up;
            return;
        }

        _currentX += _inputAxis.x * Time.deltaTime * _sensivity;
        _currentX = Mathf.Clamp(_currentX, _xDownCap, _xUpCap);

        _cameraTarget.transform.eulerAngles = new Vector3(_currentX, _cameraTarget.transform.eulerAngles.y, 0);
        _cameraTarget.transform.eulerAngles += new Vector3(0, _inputAxis.y * Time.deltaTime * _sensivity, 0);

        //! Reset Z axis
        _cameraTarget.transform.eulerAngles =
        new Vector3(_cameraTarget.transform.eulerAngles.x, _cameraTarget.transform.eulerAngles.y, 0);
    }

    public void SetCameraParameter(float shoulderOffset, PlayerState playerState)
    {
        //TODO passer PlayerState en parametre, et internaliser les parametre lie a la cam dans la class
        Cinemachine3rdPersonFollow cm = _virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        cm.ShoulderOffset = new Vector3(0, shoulderOffset, 0);

        _cameraTarget.transform.forward = transform.forward;

        _currentPlayerState = playerState;
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
