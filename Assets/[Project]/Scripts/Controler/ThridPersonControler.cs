using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonControler : MonoBehaviour
{
    [Header("Reference :")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _orientation;
    [SerializeField] private UiJoystick _joystick;
    [SerializeField] private Transform _meshRoot;
    [Header("Parametre :")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 viewDirection = transform.position - new Vector3(_camera.position.x, transform.position.y, _camera.position.z);
        _orientation.forward = viewDirection.normalized;

        Vector3 inputDirection = _joystick.GetNormalizeDirection();

        Vector3 moveDirection = _orientation.forward * inputDirection.y + _orientation.right * inputDirection.x;

        if(moveDirection != Vector3.zero)
        {
            _meshRoot.forward = Vector3.Slerp(_meshRoot.forward, moveDirection.normalized, Time.deltaTime * _rotationSpeed);
            transform.Translate(_meshRoot.forward * _joystick.GetRatioDistance() * Time.deltaTime * _moveSpeed);
        }
    }

    public void Activate()
    {
        enabled = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        _rigidbody.freezeRotation = true;
        _rigidbody.useGravity = true;

        _meshRoot.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        _meshRoot.transform.localPosition = Vector3.up;

        _rigidbody.velocity = Vector3.down * 15;
    }

    public void Disable()
    {
        enabled = false;
    }
}
