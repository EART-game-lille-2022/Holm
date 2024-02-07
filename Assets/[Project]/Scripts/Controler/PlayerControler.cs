using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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

    [Header("Reference :")]
    // [SerializeField] private UiJoystick _joystick;
    [SerializeField] private GyroscopeControler _gyroControler;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private PlayerState _currentState;
    [SerializeField] private Transform _orientation;
    [SerializeField] private CameraControler _cameraControler;
    [SerializeField] private Collider _collider;

    [Header("Ground Parametre :")]
    [SerializeField] private float _groundMoveSpeed = 40;
    [SerializeField] private float _jumpForce = 20;
    [SerializeField] private float _fallingForce = 60;    
    [SerializeField] private Vector3 _groundCenterOfMass = new Vector3(0, -.5f, 0);
    [SerializeField] private PhysicMaterial _groundPhysicMaterial;

    [Header("Fly Parametre :")]
    [SerializeField] private float _upForce = 30;
    [SerializeField] private float _liftForce = 3;
    [SerializeField] private float _windResistance = 10;
    [SerializeField] private Vector3 _flyCenterOfMass = Vector3.zero;
    [SerializeField] private float _minAngleRatioMultiplier = -1;
    [SerializeField] private float _maxAngleRatioMultiplier = 5;
    [SerializeField] private float _maxDownFallingForce = 15;
    [SerializeField] private PhysicMaterial _flyPhysicMaterial;
    [Space]
    [Space]
    [Space]
    public Vector3 _playerInput;
    public float minUpForce;
    public float velocityMag;
    public float xAngle;
    public float yAngle;
    public float noseFallingForce;
    public float stallTimer;
    public bool isStalling;
    public float stallingThresold = 70;
    [Space]

    Vector3 positionToAddForce;
    private GroundCheck _groundCheck;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    void FixedUpdate()
    {
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

    public void ChangeState(PlayerState stateToSet)
    {
        //TODO Animé le changement d'état
        //TODO set les trail pour ettre appais sur leur fin
        if (stateToSet == _currentState)
            return;

        _currentState = stateToSet;

            // print("State to set :" + _currentState);
        switch (stateToSet)
        {
            case PlayerState.Grounded:
                // print("Player Grounded !");
                transform.up = Vector3.up;
                
                _cameraControler.SetCameraParameter(1.5f, true);

                // _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                _rigidbody.centerOfMass = _groundCenterOfMass;
                _collider.material = _groundPhysicMaterial;
                _currentState = PlayerState.Grounded;
                break;

            case PlayerState.Flying:
                // Vector3 startOrientation = transform.up;
                // Quaternion targetOrientation = Quaternion.LookRotation( -Vector3.up, _orientation.up );
                Quaternion startOrientation = transform.rotation;
                Quaternion targetOrientation = Quaternion.LookRotation(-Vector3.up , transform.forward);
                _cameraControler.SetCameraParameter(0, false);
                // _rigidbody.constraints = RigidbodyConstraints.None;
                _rigidbody.centerOfMass = _flyCenterOfMass;
                _collider.material = _flyPhysicMaterial;

                DOTween.To((time) =>
                {
                    _rigidbody.rotation = transform.rotation = Quaternion.Slerp(startOrientation, targetOrientation, time);
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

        // print(_rigidbody.velocity.magnitude);
        if(_playerInput.magnitude == 0)
        {
            float velValue = Mathf.InverseLerp(0, 10, _rigidbody.velocity.magnitude);
            // print(velValue);

            Vector3 velOutY = _rigidbody.velocity;
            velOutY.y = 0;
            //! if mult trop grand : par en vrille :)
            _rigidbody.AddForce(-velOutY * 5, ForceMode.Acceleration);
        }
        // _rigidbody.velocity = new Vector3(moveDirection.x, _rigidbody.velocity.y, moveDirection.z);
    }

    private void FlyControler()
    {
        //TODO Ajouter de l'inercie velociter
        xAngle = Vector3.Angle(transform.up, Vector3.down) - 90;
        yAngle = Vector3.Angle(transform.right, Vector3.down) - 90;

        Vector3 velocityXZ = _rigidbody.velocity;
        velocityXZ.y = 0;
        velocityMag = velocityXZ.magnitude;


        //! Rotate le player
        if (!isStalling)
        {
            positionToAddForce = transform.TransformPoint(_playerInput);
            _rigidbody.AddForceAtPosition(-transform.forward * _liftForce * -_playerInput.magnitude, positionToAddForce
                                        , ForceMode.Acceleration);
        }


        //! force sur le yaw en fonction du roll
        float yawForce = Mathf.Lerp(0, 3, Mathf.InverseLerp(0, 90, Mathf.Abs(yAngle)));
        _rigidbody.AddForceAtPosition((yAngle > 0 ? -_orientation.right : _orientation.right) * yawForce
                                    , transform.TransformPoint(Vector3.up)
                                    , ForceMode.Acceleration);


        //! Chute en avant +/- rapide en fonction de l'inclinaison
        //TODO incrementé la force doucement si angle > 0, reset doux rapide

        //!empeche le nez de remonter tout seul
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

        if(isStalling)
        {
            _rigidbody.AddForceAtPosition(Vector3.down * noseFallingForce * 5, transform.TransformPoint(Vector3.up), ForceMode.Acceleration);
            if(xAngle < -80)
                isStalling = false;
        }

        //! Convertie l'angle en un multiplicateur en fonction de l'incilinaison
        float angleRatioMultiplier =
        Mathf.Lerp(_maxAngleRatioMultiplier, _minAngleRatioMultiplier, Mathf.InverseLerp(-90, 90, xAngle));
        //! Set la  velocité pour fly boy
        // _rigidbody.velocity = transform.up * _upForce * angleRatioMultiplier;
        _rigidbody.AddForce(transform.up * _upForce * angleRatioMultiplier, ForceMode.Acceleration);




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
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
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
