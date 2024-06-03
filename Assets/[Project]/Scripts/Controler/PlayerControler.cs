using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections.Generic;

[Serializable]
public enum PlayerState
{
    None,
    Grounded,
    Flying,
    Falling,
}

[Serializable]
public class FlyControlerParameter
{
    [SerializeField] public float upForce = 30;
    [SerializeField] public float liftForce = 3;
    [SerializeField] public float yLiftMultiplier = 3;
    [SerializeField] public float passiveYawMult = 1;
    [SerializeField] public float velocityConcervationRate = 2;
    [Space]
    [SerializeField] public float stallingVelocityThresold = 5;
    [SerializeField] public float noseFallingForce = 1;
    [SerializeField] public float stallingFallingForce = 1;
    [SerializeField] public float flyTimeBeforeCanStall = 3;
    [Space]
    [SerializeField] public float minAngleRatioMultiplier = -1;
    [SerializeField] public float maxAngleRatioMultiplier = 5;
}

public class PlayerControler : MonoBehaviour
{
    //TODO RETOUR CHRIS : super flight : plus t'es rapide plus controle son sensible
    public static bool IS_BASIC_CTRL = false;
    [Space]
    [Header("Reference :")]
    [SerializeField] private CameraControler _cameraControler;
    [SerializeField] private List<TrailRenderer> _trailList;

    [Header("Ground Parametre :")]
    [SerializeField] private float _groundMoveSpeed = 40;
    [SerializeField] private float _jumpForce = 20;
    [SerializeField] private float _groundDrag = 0;
    [SerializeField] private Vector3 _groundCenterOfMass = new Vector3(0, -.5f, 0);
    [SerializeField] private PhysicMaterial _groundPhysicMaterial;

    [Header("Fly Parametre :")]
    [SerializeField] private float _flyDrag = 3;
    [Space]
    [SerializeField] private float _orientationRecoverySpeed = 2;
    [SerializeField] private FlyControlerParameter _basicControler;
    [Space]
    [SerializeField] private FlyControlerParameter _advancedControler;

    [SerializeField] private PhysicMaterial _flyPhysicMaterial;

    private PlayerState _currentState = PlayerState.None;
    private Vector3 _flyCenterOfMass = Vector3.zero;
    private Vector3 _playerInput;
    private float _xAngle;
    private float _yAngle;
    private float _stallTimer;
    private bool _isStalling;
    private Vector3 _positionToAddForce;
    private GroundCheck _groundCheck;
    private Vector3 _groundOrientationDirection;
    private Vector3 _playerTopHeadPos;
    private Rigidbody _rigidbody;
    private Transform _orientation;
    private Collider _collider;
    private PlayerAnimation _animation;
    private float _angleRatioMultiplier = 0;
    private float _flyingTime;
    private float _orientationFactor;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<GroundCheck>();
        _orientation = GameObject.FindGameObjectWithTag("Orientation").transform;
        _collider = GetComponent<Collider>();
        _animation = transform.parent.GetComponentInChildren<PlayerAnimation>();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.CanPlayerMove)
            return;

        // RecenterPlayerUp();
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
            _rigidbody.AddForceAtPosition(_groundOrientationDirection * 20, transform.position + Vector3.up, ForceMode.Acceleration);
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

                _rigidbody.drag = _groundDrag;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.centerOfMass = _groundCenterOfMass;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

                _collider.material = _groundPhysicMaterial;
                transform.up = Vector3.up;

                if (_trailList.Count != 0)
                    foreach (var item in _trailList)
                        item.gameObject.SetActive(false);

                _currentState = PlayerState.Grounded;
                break;

            case PlayerState.Flying:

                _rigidbody.drag = _flyDrag;
                _rigidbody.centerOfMass = _flyCenterOfMass;
                _rigidbody.constraints = RigidbodyConstraints.None;


                _collider.material = _flyPhysicMaterial;

                if (_trailList.Count != 0)
                    foreach (var item in _trailList)
                        item.gameObject.SetActive(true);

                Quaternion startOrientation = transform.rotation;
                Quaternion targetOrientation = Quaternion.LookRotation(-Vector3.up, transform.forward);

                DOTween.To((time) =>
                {
                    transform.rotation = Quaternion.Slerp(startOrientation, targetOrientation, time);
                }, 0, 1, .5f)
                .OnComplete(() =>
                {
                    _cameraControler.SetCameraParameter(_currentState);
                });
                break;
        }
    }

    private void ComputeOrientation()
    {
        Vector3 viewDirection = transform.position -
                        new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);

        viewDirection = viewDirection.normalized;
        _orientation.forward = viewDirection;
    }

    private void GroundControler()
    {
        Vector3 moveDireciton = _orientation.forward * _playerInput.y + _orientation.right * _playerInput.x;
        moveDireciton *= _groundMoveSpeed;

        if (moveDireciton != Vector3.zero)
            transform.forward = Vector3.Slerp(transform.forward, moveDireciton, Time.fixedDeltaTime * 10);

        _rigidbody.MovePosition(transform.position + moveDireciton * Time.fixedDeltaTime);
    }


    private void FlyControler()
    {
        _xAngle = Vector3.Angle(transform.up, Vector3.down) - 90;
        _yAngle = Vector3.Angle(transform.right, Vector3.down) - 90;
        Vector3 velocityXZ = _rigidbody.velocity;
        velocityXZ.y = 0;

        _orientationFactor = Vector3.Dot(Vector3.up, transform.up);

        if (IS_BASIC_CTRL)
        {
            BasicControler(_basicControler);
            PushResetOrientation();
        }
        else
        {
            AdvancedControler(_advancedControler);
            Stalling(_advancedControler);
        }


        FallingNose(IS_BASIC_CTRL ? _basicControler : _advancedControler);
        PushForward(IS_BASIC_CTRL ? _basicControler : _advancedControler);

        if (_orientationFactor > -.9f & _orientationFactor < .9f)
            PassiveYawPushRotate(IS_BASIC_CTRL ? _basicControler : _advancedControler);
    }

    private void PushResetOrientation()
    {
        Vector3 posToAddForce = _yAngle > 0 ? Vector3.left : Vector3.right;
        posToAddForce = transform.TransformPoint(posToAddForce.normalized);

        float orientationSpeedFactor = Mathf.Lerp(5, 1, Mathf.InverseLerp(-1, 1, _orientationFactor));

        Vector3 forceDirection = _yAngle > 0 ? -Camera.main.transform.right : Camera.main.transform.right;

        _rigidbody.AddForceAtPosition(forceDirection * _orientationRecoverySpeed * orientationSpeedFactor
                                     , posToAddForce, ForceMode.Acceleration);
    }


    private void BasicControler(FlyControlerParameter flyCtrl)
    {
        if (_isStalling)
            return;

        Vector3 forceDirection = (Camera.main.transform.up * _playerInput.y + Camera.main.transform.right * (_playerInput.x / 2)) * flyCtrl.liftForce;

        //! Fonce en bas
        if (_orientationFactor < -.9f)
        {
            print("re compute forceDirection !");
            forceDirection = (-transform.forward * _playerInput.y + transform.right * (_playerInput.x / 2)) * flyCtrl.liftForce;
        }

        Vector3 posToAddForce = transform.TransformPoint(Vector3.up * 2);
        _rigidbody.AddForceAtPosition(forceDirection, posToAddForce, ForceMode.Acceleration);
    }

    private void AdvancedControler(FlyControlerParameter flyCtrl)
    {
        //! en avion de chasse ou bien ?!
        if (_isStalling)
            return;

        _positionToAddForce = transform.TransformPoint(new Vector3(_playerInput.x, _playerInput.y * flyCtrl.yLiftMultiplier, 0));
        _rigidbody.AddForceAtPosition(-transform.forward * flyCtrl.liftForce * -_playerInput.magnitude, _positionToAddForce
                                    , ForceMode.Acceleration);
    }

    private void PushForward(FlyControlerParameter flyCtrl)
    {
        //! Convertie l'angle en un multiplicateur en fonction de l'incilinaison

        float _velocityConcervationRate = (_xAngle > 0 && _rigidbody.velocity.magnitude > 20f) ? .5f : 1;

        _angleRatioMultiplier = Mathf.Lerp(_angleRatioMultiplier,
        Mathf.Lerp(flyCtrl.maxAngleRatioMultiplier, flyCtrl.minAngleRatioMultiplier, Mathf.InverseLerp(-90, 90, _xAngle))
        , Time.fixedDeltaTime * _velocityConcervationRate);

        // angleRatioMultiplier = Mathf.Lerp(_maxAngleRatioMultiplier, _minAngleRatioMultiplier, Mathf.InverseLerp(-90, 90, _xAngle));
        _rigidbody.AddForce(transform.up * ((flyCtrl.upForce * _angleRatioMultiplier) + 0), ForceMode.Acceleration);
    }

    private void FallingNose(FlyControlerParameter flyCtrl)
    {
        //!empeche le nez de remonter tout seul et le fait doucement chuté
        if (_xAngle > 0)
            _rigidbody.AddForceAtPosition(Vector3.down * flyCtrl.noseFallingForce, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);
        if (_xAngle < 0)
            _rigidbody.AddForceAtPosition(Vector3.down * flyCtrl.noseFallingForce * .2f, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);
    }

    private void PassiveYawPushRotate(FlyControlerParameter flyCtrl)
    {
        // print("origjoritrgjroitj");
        //! force sur le yaw en fonction du roll
        float yawForce = Mathf.Lerp(0, 3, Mathf.InverseLerp(0, 90, Mathf.Abs(_yAngle)));
        _rigidbody.AddForceAtPosition((_yAngle > 0 ? -_orientation.right : _orientation.right) * yawForce * flyCtrl.passiveYawMult
                                    , transform.TransformPoint(Vector3.up)
                                    , ForceMode.Acceleration);
    }

    private void Stalling(FlyControlerParameter flyCtrl)
    {
        if (_groundCheck._hisGrounded)
            _flyingTime = 0;
        else
            _flyingTime += Time.fixedDeltaTime;

        if (_flyingTime < flyCtrl.flyTimeBeforeCanStall)
            return;

        //! Décrochage !
        if (_rigidbody.velocity.magnitude < flyCtrl.stallingVelocityThresold)
        {
            _stallTimer += Time.deltaTime;
            _isStalling = _stallTimer > .2f;
        }
        else
            _stallTimer = 0;

        if (_isStalling)
        {
            //TODO mettre en stalling seulement en fonction de la vitesse du joueur !
            _rigidbody.AddForceAtPosition(Vector3.down * flyCtrl.stallingFallingForce, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);
            if (_xAngle < -80)
                _isStalling = false;
        }
    }


    //! Input
    private void OnMove(InputValue value)
    {
        if (!GameManager.instance.CanPlayerMove)
            return;

        Vector2 valueVector = value.Get<Vector2>();
        _playerInput = valueVector;

        _animation.SetRun(_playerInput != Vector3.zero);
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
    //! -_-

    public Vector2 GetPlayerInput()
    {
        return _playerInput;
    }

    public PlayerState GetPlayerState()
    {
        return _currentState;
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(transform.TransformPoint(Vector3.up * -2), .5f);

    //     // // Gizmos.color = Color.magenta;
    //     // // Gizmos.DrawSphere(transform.TransformPoint(Vector3.up), .5f);

    //     // // Gizmos.color = Color.red;
    //     // // Gizmos.DrawSphere(_center, .1f);

    //     // // Gizmos.color = Color.blue;
    //     // // Gizmos.DrawSphere(_hat, .1f);
    // }

    // private void OnGUI()
    // {
    //     GUI.skin.label.fontSize = Screen.width / 40;
    //     GUILayout.Label("Velocity Mag: " + _rigidbody.velocity.magnitude);
    // }
}
