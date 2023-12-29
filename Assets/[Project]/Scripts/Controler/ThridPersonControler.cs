using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class ThridPersonControler : MonoBehaviour
{
    [Header("Reference :")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _orientation;
    [SerializeField] private UiJoystick _joystick;
    [SerializeField] private Transform _mesh;
    [Header("Parametre :")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    void Update()
    {
        //! j'ai copié du code sans vraiment le comprendre nik moi
        Vector3 viewDirection = transform.position - new Vector3(_camera.position.x, transform.position.y, _camera.position.z);
        _orientation.forward = viewDirection.normalized;

        Vector3 inputDirection = _joystick.GetAxis();
        inputDirection.z = 0;

        Vector3 moveDirection = _orientation.forward * inputDirection.y + _orientation.right * inputDirection.x;

        if(moveDirection != Vector3.zero)
        {
            _mesh.forward = Vector3.Slerp(_mesh.forward, moveDirection.normalized, Time.deltaTime * _rotationSpeed);
            transform.Translate(_mesh.forward * Time.deltaTime * _moveSpeed);
        }
    }
}
