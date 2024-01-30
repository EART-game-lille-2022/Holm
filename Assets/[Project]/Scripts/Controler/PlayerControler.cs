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

    [Space]
    public Vector3 _playerInput;
    public float minUpForce;
    public float velocityMag;
    public float angle;
    public float angleRatio;
    Vector3 positionToAddForce;
    private GroundCheck _groundCheck;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    void FixedUpdate()
    {
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

        switch (_currentState)
        {
            case PlayerState.Grounded:
                // print("Player Grounded !");
                //TODO Reorientation du joueur ne fonctione pas bien en fonction de l'orientation de départ
                transform.up = _orientation.up;
                _cameraControler.SetCameraParameter(1.5f, true);

                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                _rigidbody.drag = _groundRbDrag;
                break;

            case PlayerState.Flying:
                transform.up = _orientation.forward;
                // transform.right = _orientation.right;
                _cameraControler.SetCameraParameter(0, false);

                _rigidbody.constraints = RigidbodyConstraints.None;
                _rigidbody.drag = _flyRbDrag;
                break;
        }
    }

    private void GroundControler()
    {
        print("Ground");
        Vector3 viewDirection = transform.position - new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        viewDirection = viewDirection.normalized;
        // print("viewDirection " + viewDirection);
        _orientation.forward = viewDirection;

        Vector3 moveDireciton = _orientation.forward * _playerInput.y + _orientation.right * _playerInput.x;

        if (moveDireciton != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDireciton, Time.fixedDeltaTime * 5);
            // transform.Translate(Vector3.forward * _groundMoveSpeed * Time.fixedDeltaTime * _playerInput.magnitude);
            // _rigidbody.AddForce(transform.forward * _groundMoveSpeed, ForceMode.Acceleration);
        }

        Vector3 newVelocity = transform.forward * _playerInput.magnitude * _groundMoveSpeed;
        _rigidbody.velocity = new Vector3(newVelocity.x, _rigidbody.velocity.y, newVelocity.z);

    }

    private void FlyControler()
    {
        print("Fly");
        //TODO revoire les valeur et ajuster les addforce
        angle = Vector3.Angle(transform.up, Vector3.down) - 90;
        angleRatio = angle / 90;

        Vector3 velocityXZ = _rigidbody.velocity;
        velocityXZ.y = 0;

        velocityMag = velocityXZ.magnitude;

        //! Rotate le player
        positionToAddForce = transform.TransformPoint(_playerInput);
        _rigidbody.AddForceAtPosition(-transform.forward * _liftForce * -_playerInput.magnitude, positionToAddForce
                                     , ForceMode.Acceleration);

        //! Resistance au vent en fonction d'angle
        _rigidbody.AddForce(Vector3.up * angleRatio * _windResistance * velocityMag, ForceMode.Acceleration);


        //! Chute en avant
        _rigidbody.AddForceAtPosition(Vector3.down, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);

        //! Acceleration du player
        //! -angle pour avoir un multiplier positif avec un angle negatif
        if (angle < 0 && minUpForce < 50)
            minUpForce += Time.fixedDeltaTime * 25;
        else if (minUpForce > 0)
            minUpForce -= Time.fixedDeltaTime * 25;

        // float direction = angle > 0 ? -1 : 1;
        // print(Mathf.InverseLerp(-1, 1, Math.Max(-angle, minUpForce) / 90));

        _rigidbody.AddForce(transform.up * _upForce * Mathf.InverseLerp(-1, 1, Math.Max(-angle, minUpForce) / 90)
                           , ForceMode.Acceleration);                                                //! /90 ramene a 0-1; 

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
    }
}
