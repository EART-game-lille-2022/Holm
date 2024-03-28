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
    //TODO RETOUR CHRIS : on peut remonter trop fascilement en restant vers le haut
    //TODO RETOUR CHRIS : TROP de perte de vitesse donc on peut pas remonter avec notre gain de vitesse apres une "chute"
    //TODO RETOUR CHRIS : super flight : plus t'es rapide plus controle son sensible

    [Header("Reference :")]
    [SerializeField] private CameraControler _cameraControler;
    [SerializeField] private List<TrailRenderer> _trailList;

    [Header("Global Parameter :")]
    [Space]
    [SerializeField] private float _worldYLimite = 1000;

    [Header("Ground Parametre :")]
    [SerializeField] private float _groundMoveSpeed = 40;
    [SerializeField] private float _jumpForce = 20;
    [SerializeField] private float _fallingForce = 60;
    [SerializeField] private Vector3 _groundCenterOfMass = new Vector3(0, -.5f, 0);
    [SerializeField] private PhysicMaterial _groundPhysicMaterial;

    [Header("Fly Parametre :")]
    [SerializeField] private float _upForce = 30;
    [SerializeField] private float _liftForce = 3;
    [SerializeField] private float _yLiftMultiplier = 3;
    [SerializeField] private Vector3 _flyCenterOfMass = Vector3.zero;
    [SerializeField] private float _stallingAngleThresold = 70;
    [SerializeField] private float _stallingVelocityThresold = 5;
    [SerializeField] private float _noseFallingForce = 1;
    [SerializeField] private float _minAngleRatioMultiplier = -1;
    [SerializeField] private float _maxAngleRatioMultiplier = 5;
    [SerializeField] private PhysicMaterial _flyPhysicMaterial;

    [Header("WIP :")]


    [Space]
    [Space]
    [Space]

    public float _stallingMagnitudeThresold;
    private Vector3 _playerInput;
    public float _xAngle;
    private float _yAngle;
    private float _stallTimer;
    private bool _isStalling;
    private Vector3 _positionToAddForce;
    private GroundCheck _groundCheck;
    private Vector3 _groundOrientationDirection;
    private Vector3 _playerTopHeadPos;
    private PlayerState _currentState = PlayerState.None;
    private Rigidbody _rigidbody;
    private Transform _orientation;
    private Collider _collider;
    private PlayerAnimation _animation;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<GroundCheck>();
        _orientation = GameObject.FindGameObjectWithTag("Orientation").transform;
        _collider = GetComponent<Collider>();
        _animation = GetComponentInChildren<PlayerAnimation>();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.CanPlayerMove)
        {
            // _rigidbody.useGravity = false;
            return;
        }
        // _rigidbody.useGravity = true;


        if (transform.position.y < -_worldYLimite)
        {
            transform.position = new Vector3(transform.position.x, _worldYLimite, transform.position.z);
            _rigidbody.velocity = Vector3.zero;
        }

        RecenterPlayerUp();
        ComputeOrientation();

        if (_currentState == PlayerState.Grounded)
        {
            _animation.SetGround(true);
            GroundControler();
        }

        if (_currentState == PlayerState.Flying)
        {
            _animation.SetGround(false);
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
        // print("Change State Call from : " + _currentState + " // to : " + stateToSet);
        if (stateToSet == _currentState)
            return;

        _currentState = stateToSet;

        // print("State to set :" + _currentState);
        switch (stateToSet)
        {
            case PlayerState.Grounded:
                _cameraControler.SetCameraParameter(_currentState);

                // _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.centerOfMass = _groundCenterOfMass;
                _collider.material = _groundPhysicMaterial;
                transform.up = Vector3.up;

                if (_trailList.Count != 0)
                    foreach (var item in _trailList)
                        item.gameObject.SetActive(false);

                _currentState = PlayerState.Grounded;
                break;

            case PlayerState.Flying:
                Quaternion startOrientation = transform.rotation;
                Quaternion targetOrientation = Quaternion.LookRotation(-Vector3.up, transform.forward);

                _rigidbody.centerOfMass = _flyCenterOfMass;
                _collider.material = _flyPhysicMaterial;

                if (_trailList.Count != 0)
                    foreach (var item in _trailList)
                        item.gameObject.SetActive(true);

                DOTween.To((time) =>
                {
                    transform.rotation = Quaternion.Slerp(startOrientation, targetOrientation, time);
                }, 0, 1, .5f)
                .OnComplete(() => _cameraControler.SetCameraParameter(_currentState));
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
        _xAngle = Vector3.Angle(transform.up, Vector3.down) - 90;
        _yAngle = Vector3.Angle(transform.right, Vector3.down) - 90;

        Vector3 velocityXZ = _rigidbody.velocity;
        velocityXZ.y = 0;

        //! Rotate le player
        if (!_isStalling)
        {
            //! Lift
            _positionToAddForce = transform.TransformPoint(new Vector3(_playerInput.x, _playerInput.y * _yLiftMultiplier, 0));
            _rigidbody.AddForceAtPosition(-transform.forward * _liftForce * -_playerInput.magnitude, _positionToAddForce
                                        , ForceMode.Acceleration);
        }


        //! force sur le yaw en fonction du roll
        float yawForce = Mathf.Lerp(0, 3, Mathf.InverseLerp(0, 90, Mathf.Abs(_yAngle)));
        _rigidbody.AddForceAtPosition((_yAngle > 0 ? -_orientation.right : _orientation.right) * yawForce
                                    , transform.TransformPoint(Vector3.up)
                                    , ForceMode.Acceleration);


        //!empeche le nez de remonter tout seul et le fait doucement chuté
        if (_xAngle > 0)
            _rigidbody.AddForceAtPosition(Vector3.down * _noseFallingForce, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);
        if (_xAngle < 0)
            _rigidbody.AddForceAtPosition(Vector3.down * _noseFallingForce * .2f, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);


        //! Décrochage !
        // if (_xAngle > _stallingAngleThresold || (_velocityMagnitude < _stallingMagnitudeThresold && _xAngle > 0))
        if (_rigidbody.velocity.magnitude < _stallingVelocityThresold)
        {
            _stallTimer += Time.deltaTime;
            _isStalling = _stallTimer > .2f ? true : false;
            // _isStalling = true;
        }
        else
            _stallTimer = 0;

        if (_isStalling)
        {
            //TODO mettre en stalling seulement en fonction de la vitesse du joueur !
            // print("Stall !!!");
            _rigidbody.AddForceAtPosition(Vector3.down * _noseFallingForce * 5, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);
            if (_xAngle < -80)
                _isStalling = false;
        }


        //! Convertie l'angle en un multiplicateur en fonction de l'incilinaison
        float angleRatioMultiplier =
        Mathf.Lerp(_maxAngleRatioMultiplier, _minAngleRatioMultiplier, Mathf.InverseLerp(-90, 90, _xAngle));

        _rigidbody.AddForce(transform.up * _upForce * angleRatioMultiplier, ForceMode.Acceleration);
        if (_xAngle > 0)
            _rigidbody.AddForce(transform.up, ForceMode.Acceleration);
        // print(angleRatioMultiplier);
    }

    // public float _fallingStartConservationThresold;
    // public float _fallingConservationRate;
    // public float _fallingConservationCurrentValue;
    // public float _fallingConservationMax;

    private void OnMove(InputValue value)
    {
        if (!GameManager.instance.CanPlayerMove)
            return;

        Vector2 valueVector = value.Get<Vector2>();
        _playerInput = valueVector;

        _animation.SetRun(_playerInput == Vector3.zero ? false : true);
        _animation.SetXVector(_playerInput.x);
        _animation.SetYVector(_playerInput.y);
    }

    private void OnJump(InputValue value)
    {
        if ((value.Get<float>() == 1 ? true : false) && _groundCheck.CanJump())
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
            _animation.SetJump(true);
        }
    }

    public void SetPlayerPosition(Vector3 value)
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = value + new Vector3(0, ((CapsuleCollider)_collider).height / 2, 0);
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

    // private void OnGUI()
    // {
    //     GUI.skin.label.fontSize = Screen.width / 40;

    //     GUILayout.Label("Velocity Mag: " + _rigidbody.velocity.magnitude);
    // }
}
