using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _sensivity = 1;
    private Vector3 _inputAxis;
    bool _playerCanMoveCamera;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(!_playerCanMoveCamera)
            _cameraTarget.transform.forward = transform.up;

        // _cameraTarget.Rotate(new Vector3(_inputAxis.y, _inputAxis.x, 0) * _sensivity * Time.deltaTime);
        _cameraTarget.transform.eulerAngles += _inputAxis * Time.deltaTime * _sensivity;
        _cameraTarget.transform.eulerAngles = new Vector3(_cameraTarget.transform.eulerAngles.x, _cameraTarget.transform.eulerAngles.y, 0);
    }

    public void SetCameraParameter(float shoulderOffset, bool canPlayerMoveCamera)
    {
        Cinemachine3rdPersonFollow cm = _virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        cm.ShoulderOffset = new Vector3(0, shoulderOffset, 0);

        _cameraTarget.transform.forward = transform.forward;

        _playerCanMoveCamera = canPlayerMoveCamera;
        // Camera.main.GetComponent<CinemachineBrain>().enabled = _playerCanMoveCamera;
        // Camera.main.transform.parent = _playerCanMoveCamera ? transform : null;
    }

    void OnLook(InputValue value)
    {
        if(!_playerCanMoveCamera)
        {
            _inputAxis = Vector2.zero;
            return;
        }
        
        _inputAxis.x = -value.Get<Vector2>().y;
        _inputAxis.y = value.Get<Vector2>().x;
    }
}
