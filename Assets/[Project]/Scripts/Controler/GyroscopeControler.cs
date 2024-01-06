using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeControler : MonoBehaviour
{
    [Header("Reference :")]
    [SerializeField] private CameraControler _cameraControler;
    [Space]
    public Vector3 _currentAccelerometre;
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    public Vector3 _deltaAccelerometre;
    Vector3 _lastFrameAccelerometre;

    void Update()
    {
        _currentAccelerometre = Input.acceleration;

        _deltaAccelerometre += _currentAccelerometre - _lastFrameAccelerometre;

        _lastFrameAccelerometre = _currentAccelerometre;
    }

    public Vector3 GetRotationRate()
    {
        return _deltaAccelerometre;
    }

    //! Call by reset button
    public void ResetDelta()
    {
        _deltaAccelerometre = Vector3.zero;
    }
}
