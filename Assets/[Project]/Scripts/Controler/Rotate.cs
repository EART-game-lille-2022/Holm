using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 _axisToRotate;
    [SerializeField] private float _speed;

    void Update()
    {
        transform.Rotate(_axisToRotate * _speed);
    }
}
