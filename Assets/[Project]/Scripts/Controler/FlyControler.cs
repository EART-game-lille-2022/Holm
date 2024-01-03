using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Pti tuto qui carry : https://www.youtube.com/watch?v=fThb5M2OBJ8&t=461s&ab_channel=b3agz

public class FlyControler : MonoBehaviour
{
    [Header("Reference :")]
    [SerializeField] private UiJoystick _joystick;
    [SerializeField] private Transform _meshRoot;
    [Header("Parametre :")]
    [SerializeField] private float _throttleIncrement = .1F;
    [SerializeField] private float _maxTrust = 100f;
    [SerializeField] private float _responsivness = 10;

    public float _throttle;
    public int _throttleDirection;
    public float _yaw;
    public float _roll;
    public float _pitch;

    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        _rigidbody.AddForce(transform.forward * _throttle * _maxTrust);
        _rigidbody.AddTorque(Vector3.right * _pitch * _responsivness);
        _rigidbody.AddTorque(Vector3.up * _yaw * _responsivness);
    }

    void HandleInput()
    {
        // print(_joystick.GetDirection());
        _pitch = _joystick.GetDirection().y;
        _yaw = _joystick.GetDirection().x;

        if(_throttleDirection != 0)
            _throttle += Time.deltaTime * _throttleIncrement * _throttleDirection;
        
        _throttle = Mathf.Clamp(_throttle, 0, 1);
    }

    public void SetThrottleDirectionValue(int value)
    {
        _throttleDirection = value;
    }

    public void Activate()
    {
        enabled = true;
        _rigidbody.freezeRotation = false;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _meshRoot.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    public void Disable()
    {
        enabled = false;
    }
}
