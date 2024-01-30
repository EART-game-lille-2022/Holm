using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum PlayerState
{
    None,
    Grounded,
    Flying,
}

public class PlayerControler : MonoBehaviour
{
    [Header("Reference :")]
    // [SerializeField] private UiJoystick _joystick;
    [SerializeField] private GyroscopeControler _gyroControler;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private PlayerState _currentState;
    [SerializeField] private Transform _orientation;
    [SerializeField] private CameraControler _cameraControler;

    [Header("Ground Parametre :")]
    [SerializeField] private float _groundMoveSpeed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private float _groundRbDrag;

    [Header("Fly Parametre :")]
    [SerializeField] private float _upForce = 10;
    [SerializeField] private float _liftForce = 5;
    [SerializeField] private float _windResistance = 2;
    [SerializeField] private float _flyRbDrag;
    [SerializeField] private float _minAngleRatioMultiplier;
    [SerializeField] private float _maxAngleRatioMultiplier;
    [SerializeField] private float _minDownFallingForce;
    [SerializeField] private float _maxDownFallingForce;
    
    [Space]
    public Vector3 _playerInput;
    public float minUpForce;
    public float velocityMag;
    public float xAngle;
    public float yAngle;
    public float angleRatio;
    Vector3 positionToAddForce;
    private GroundCheck _groundCheck;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<GroundCheck>();

        ChangeState(PlayerState.Flying);
    }

    void FixedUpdate()
    {
        Vector3 viewDirection = transform.position - new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        viewDirection = viewDirection.normalized;

        _orientation.forward = viewDirection;

        if (_currentState == PlayerState.Grounded)
        {
            GroundControler();
        }

        if (_currentState == PlayerState.Flying)
        {
            FlyControler();
        }
    }

    public void ChangeState(PlayerState stateToSet)
    {
        if (stateToSet == _currentState)
            return;

        _currentState = stateToSet;
        print("State to set :" + _currentState);
        switch (_currentState)
        {
            case PlayerState.Grounded:
                // print("Player Grounded !");
                //TODO Reorientation du joueur ne fonctione pas bien en fonction de l'orientation de départ
                transform.up = _orientation.up;
                _cameraControler.SetCameraParameter(1.5f, true);

                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                // _rigidbody.drag = _groundRbDrag;
                break;

            case PlayerState.Flying:
                transform.up = _orientation.forward;
                // transform.right = _orientation.right;
                _cameraControler.SetCameraParameter(0, false);

                _rigidbody.constraints = RigidbodyConstraints.None;
                // _rigidbody.drag = _flyRbDrag;
                break;
        }
    }

    private void GroundControler()
    {


        Vector3 moveDireciton = _orientation.forward * _playerInput.y + _orientation.right * _playerInput.x;

        if (moveDireciton != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDireciton, Time.fixedDeltaTime * 10);
            // transform.Translate(Vector3.forward * _groundMoveSpeed * Time.fixedDeltaTime * _playerInput.magnitude);
            // _rigidbody.AddForce(transform.forward * _groundMoveSpeed, ForceMode.Acceleration);
        }

        Vector3 newVelocity = transform.forward * _playerInput.magnitude * _groundMoveSpeed;
        _rigidbody.velocity = new Vector3(newVelocity.x, _rigidbody.velocity.y, newVelocity.z);
    }

    private void FlyControler()
    {
        // print("Fly");
        //TODO revoire les valeur et ajuster les addforce
        xAngle = Vector3.Angle(transform.up, Vector3.down) - 90;
        yAngle = Vector3.Angle(transform.right, Vector3.down) - 90;

        Vector3 velocityXZ = _rigidbody.velocity;
        velocityXZ.y = 0;
        velocityMag = velocityXZ.magnitude;

        //! Rotate le player
        //! peut etre "objetifié" pour avoir deux ailes
        positionToAddForce = transform.TransformPoint(_playerInput);
        _rigidbody.AddForceAtPosition(-transform.forward * _liftForce * -_playerInput.magnitude, positionToAddForce
                                     , ForceMode.Acceleration);


        //? rota sur le yaw en fonction du roll
        float yawForce = Mathf.Lerp(0, 5, Mathf.InverseLerp(0, 90, Mathf.Abs(yAngle)));
        _rigidbody.AddForceAtPosition((yAngle > 0 ? -_orientation.right : _orientation.right) * yawForce, transform.TransformPoint(Vector3.up)
                                    , ForceMode.Acceleration);

        //! Chute en avant +/- rapide en fonction de l'inclinaison
        float downFallingForce = Mathf.Lerp(_minDownFallingForce, _maxDownFallingForce, Mathf.Max(0, xAngle) / 90);
        if(xAngle > 0)
            _rigidbody.AddForceAtPosition(Vector3.down  * downFallingForce, transform.TransformPoint(Vector3.up)
                                        , ForceMode.Acceleration);


        //! Convertie l'angle en un multiplicateur en fonction de l'incilinaison
        float angleRatioMultiplier = 
        Mathf.Lerp(_maxAngleRatioMultiplier, _minAngleRatioMultiplier, Mathf.InverseLerp(-90, 90, xAngle));


        //! Set la  velocité pour fly boy
        _rigidbody.velocity = transform.up * _upForce * angleRatioMultiplier;



        //! Resistance au vent en fonction d'angle
        // _rigidbody.AddForce(Vector3.up * angleRatio * _windResistance * velocityMag, ForceMode.Acceleration);


        //! Acceleration du player
        //! -angle pour avoir un multiplier positif avec un angle negatif
        // if (angle < 0 && minUpForce < 50)
        //     minUpForce += Time.fixedDeltaTime * 25;
        // else if (minUpForce > 0)
        //     minUpForce -= Time.fixedDeltaTime * 25;

        // float direction = angle > 0 ? -1 : 1;
        
        // print(Mathf.InverseLerp(-1, 1, Math.Max(-angle, minUpForce) / 90));

        //! Force pour avancer
        // _rigidbody.AddForce(transform.up * _upForce * Math.Max(-angle, minUpForce) / 90
        //                    , ForceMode.Acceleration);                                                //! /90 ramene a 0-1; 

        // float fallingForce = Mathf.InverseLerp(60, 90, angle);
        // _rigidbody.AddForce(Vector3.down * Physics.gravity.magnitude * (angle > 60 ? angle : fallingForce), ForceMode.Impulse);
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
            _rigidbody.velocity += Vector3.up * _jumpForce;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(positionToAddForce, .5f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.TransformPoint(Vector3.up), .5f);

    }
}
