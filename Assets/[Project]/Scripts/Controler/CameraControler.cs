using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _sensivity = 1;
    [SerializeField] private float _shoulderOffset;

    private Vector3 _inputAxis;
    private bool _playerCanMoveCamera;
    private Transform _startTarget;
    private Transform _startLookAt;

    void Start()
    {
        _startTarget = _cameraTarget;
        _startLookAt = _virtualCam.LookAt;
    }

    void Update()
    {
        //TODO screenShake pendant la chute
        if (!GameManager.instance.CanPlayerMove)
            return;

        if (!_playerCanMoveCamera)
            _cameraTarget.transform.forward = transform.up;

        // _cameraTarget.Rotate(new Vector3(_inputAxis.y, _inputAxis.x, 0) * _sensivity * Time.deltaTime);
        _cameraTarget.transform.eulerAngles += _inputAxis * Time.deltaTime * _sensivity;
        _cameraTarget.transform.eulerAngles = new Vector3(_cameraTarget.transform.eulerAngles.x, _cameraTarget.transform.eulerAngles.y, 0);
    }

    public void SetCameraParameter(float shoulderOffset, bool canPlayerMoveCamera)
    {
        //TODO passer PlayerState en parametre, et internaliser les parametre lie a la cam dans la class
        Cinemachine3rdPersonFollow cm = _virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        cm.ShoulderOffset = new Vector3(0, shoulderOffset, 0);

        _cameraTarget.transform.forward = transform.forward;

        _playerCanMoveCamera = canPlayerMoveCamera;
        // Camera.main.GetComponent<CinemachineBrain>().enabled = _playerCanMoveCamera;
        // Camera.main.transform.parent = _playerCanMoveCamera ? transform : null;
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

    void OnLook(InputValue value)
    {
        if (!_playerCanMoveCamera)
        {
            _inputAxis = Vector2.zero;
            return;
        }

        _inputAxis.x = -value.Get<Vector2>().y;
        _inputAxis.y = value.Get<Vector2>().x;
    }
}
