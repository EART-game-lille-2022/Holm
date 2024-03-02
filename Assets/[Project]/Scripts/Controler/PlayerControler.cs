using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections.Generic;
using Cinemachine;

[Serializable]
public enum PlayerState
{
    None,
    Grounded,
    Flying,
}

public class PlayerControler : MonoBehaviour
{
    //TODO raycast pour voire si le joueur est bloqué contre un mur = nerf la force pour avancer

    //TODO RETOUR CHRIS : on peut remonter trop fascilement en restant vers le haut
    //TODO RETOUR CHRIS : TROP de perte de vitesse donc on peut pas remonter avec notre gain de vitesse apres une "chute"
    //TODO RETOUR CHRIS : super flight : plus t'es rapide 


    [Header("Reference :")]
    // [SerializeField] private UiJoystick _joystick;
    [SerializeField] private Transform _orientation;
    [SerializeField] private CameraControler _cameraControler;
    [SerializeField] private Collider _collider;
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private List<TrailRenderer> _trailList;

    [Header("Global Parameter :")]
    [SerializeField] private float _minFovVelocity = 30;
    [SerializeField] private float _maxFovVelocity = 70;
    [SerializeField] private float _maxFov = 100;
    [SerializeField] private float _minFov = 70;

    [Header("Ground Parametre :")]
    [SerializeField] private float _groundMoveSpeed = 40;
    [SerializeField] private float _jumpForce = 20;
    [SerializeField] private float _fallingForce = 60;
    [SerializeField] private Vector3 _groundCenterOfMass = new Vector3(0, -.5f, 0);
    [SerializeField] private PhysicMaterial _groundPhysicMaterial;

    [Header("Fly Parametre :")]
    [SerializeField] private float _upForce = 30;
    [SerializeField] private float _liftForce = 3;
    [SerializeField] private Vector3 _flyCenterOfMass = Vector3.zero;
    [SerializeField] private float stallingThresold = 70;
    [SerializeField] private float noseFallingForce;
    [SerializeField] private float _minAngleRatioMultiplier = -1;
    [SerializeField] private float _maxAngleRatioMultiplier = 5;
    [SerializeField] private PhysicMaterial _flyPhysicMaterial;

    public float _velocityMagnitude;
    private Vector3 _playerInput;
    private float xAngle;
    private float yAngle;
    private float stallTimer;
    private bool isStalling;
    private Vector3 _positionToAddForce;
    private GroundCheck _groundCheck;
    private Vector3 _groundOrientationDirection;
    private Vector3 _playerTopHeadPos;
    private PlayerState _currentState;
    private Rigidbody _rigidbody;



    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    void FixedUpdate()
    {
        _velocityMagnitude = _rigidbody.velocity.magnitude;
        _virtualCam.m_Lens.FieldOfView =
        Mathf.Lerp(_minFov, _maxFov, Mathf.InverseLerp(_minFovVelocity, _maxFovVelocity, _velocityMagnitude));

        RecenterPlayerUp();

        ComputeOrientation();
        if (_currentState == PlayerState.Grounded)
        {
            GroundControler();
        }

        if (_currentState == PlayerState.Flying)
        {
            FlyControler();
        }
    }

    void RecenterPlayerUp()
    {
        if (_currentState == PlayerState.Grounded)
        {
            _playerTopHeadPos = transform.TransformPoint(Vector3.up);

            _groundOrientationDirection = transform.position + Vector3.up - _playerTopHeadPos;
            // print(_groundOrientationDirection.magnitude);
            _rigidbody.AddForceAtPosition(_groundOrientationDirection * 10, transform.position + Vector3.up, ForceMode.Acceleration);
        }
    }

    public void ChangeState(PlayerState stateToSet)
    {
        if (stateToSet == _currentState)
            return;

        _currentState = stateToSet;

        // print("State to set :" + _currentState);
        switch (stateToSet)
        {
            case PlayerState.Grounded:
                _cameraControler.SetCameraParameter(1.5f, true);

                // _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.centerOfMass = _groundCenterOfMass;
                _collider.material = _groundPhysicMaterial;
                transform.up = Vector3.up;

                foreach (var item in _trailList)
                    item.enabled = false;

                _currentState = PlayerState.Grounded;
                break;

            case PlayerState.Flying:
                _cameraControler.SetCameraParameter(0, false);

                Quaternion startOrientation = transform.rotation;
                Quaternion targetOrientation = Quaternion.LookRotation(-Vector3.up, transform.forward);

                _rigidbody.centerOfMass = _flyCenterOfMass;
                _collider.material = _flyPhysicMaterial;

                //TODO animé l'épaiseur dur trail pour son apprarition
                foreach (var item in _trailList)
                    item.enabled = true;

                DOTween.To((time) =>
                {
                    //! 
                    
                    //! Rotate le rb pour passer en vol
                    /* _rigidbody.rotation =  */
                    transform.rotation = Quaternion.Slerp(startOrientation, targetOrientation, time);
                }, 0, 1, .5f);
                break;
        }
    }

    private void ComputeOrientation()
    {
        Vector3 viewDirection = transform.position - new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        viewDirection = viewDirection.normalized;

        _orientation.forward = viewDirection;
    }

    private void GroundControler()
    {
        Vector3 moveDireciton = _orientation.forward * _playerInput.y + _orientation.right * _playerInput.x;
        moveDireciton *= _groundMoveSpeed;

        if (moveDireciton != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDireciton, Time.fixedDeltaTime * 10);
            // transform.Translate(Vector3.forward * _groundMoveSpeed * Time.fixedDeltaTime * _playerInput.magnitude);
            // _rigidbody.AddForce(transform.forward * _groundMoveSpeed, ForceMode.Acceleration);
        }

        _rigidbody.AddForce(moveDireciton, ForceMode.Acceleration);
        _rigidbody.AddForce(Vector3.down * _fallingForce, ForceMode.Acceleration);


        //! Slow down the player on ground
        if (_playerInput.magnitude == 0)
        {
            float velValue = Mathf.InverseLerp(0, 10, _rigidbody.velocity.magnitude);
            // print(velValue);

            Vector3 velOutY = _rigidbody.velocity;
            velOutY.y = 0;
            _rigidbody.AddForce(-velOutY * 5, ForceMode.Acceleration);
        }
    }

    private void FlyControler()
    {
        //TODO fonction de "reset" l'orientation du joueur en maintenant "B" par exemple

        //TODO Ajouter de l'inercie velociter
        xAngle = Vector3.Angle(transform.up, Vector3.down) - 90;
        yAngle = Vector3.Angle(transform.right, Vector3.down) - 90;

        Vector3 velocityXZ = _rigidbody.velocity;
        velocityXZ.y = 0;


        //! Rotate le player
        if (!isStalling)
        {
            // _positionToAddForce = transform.TransformPoint(new Vector2(_playerInput.x, 0));
            _positionToAddForce = transform.TransformPoint(_playerInput);

            //! Z
            _rigidbody.AddForceAtPosition(-transform.forward * _liftForce * -_playerInput.magnitude, _positionToAddForce
                                        , ForceMode.Acceleration);

            //! X
            // Vector3 xToAddForce = new Vector3(transform.position.x + _playerInput.x, transform.position.y, transform.position.z);
            // _rigidbody.AddForceAtPosition(-transform.forward * _liftForce * -_playerInput.magnitude, xToAddForce
            //                             , ForceMode.Acceleration);
        }

        //! force sur le yaw en fonction du roll
        float yawForce = Mathf.Lerp(0, 3, Mathf.InverseLerp(0, 90, Mathf.Abs(yAngle)));
        _rigidbody.AddForceAtPosition((yAngle > 0 ? -_orientation.right : _orientation.right) * yawForce
                                    , transform.TransformPoint(Vector3.up)
                                    , ForceMode.Acceleration);

        //!empeche le nez de remonter tout seul et le fait doucement chuté
        if (xAngle > 0)
            _rigidbody.AddForceAtPosition(Vector3.down * noseFallingForce, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);
        if (xAngle < 0)
            _rigidbody.AddForceAtPosition(Vector3.down * noseFallingForce * .2f, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);

        //! Décrochage !
        if (xAngle > stallingThresold)
        {
            stallTimer += Time.deltaTime;
            isStalling = stallTimer > .2f ? true : false;
        }
        else
            stallTimer = 0;

        if (isStalling)
        {
            _rigidbody.AddForceAtPosition(Vector3.down * noseFallingForce * 5, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);
            if (xAngle < -80)
                isStalling = false;
        }

        //! Convertie l'angle en un multiplicateur en fonction de l'incilinaison
        float angleRatioMultiplier =
        Mathf.Lerp(_maxAngleRatioMultiplier, _minAngleRatioMultiplier, Mathf.InverseLerp(-90, 90, xAngle));
        _rigidbody.AddForce(transform.up * _upForce * angleRatioMultiplier, ForceMode.Acceleration);
    }

    private void OnMove(InputValue value)
    {
        Vector2 valueVector = value.Get<Vector2>();
        _playerInput = valueVector;
    }

    private void OnJump(InputValue value)
    {
        if (value.Get<float>() == 1 ? true : false && _groundCheck.IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_positionToAddForce, .5f);

        // Gizmos.color = Color.magenta;
        // Gizmos.DrawSphere(transform.TransformPoint(Vector3.up), .5f);

        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(_center, .1f);

        // Gizmos.color = Color.blue;
        // Gizmos.DrawSphere(_hat, .1f);
    }
}
