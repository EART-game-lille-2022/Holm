using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeControler : MonoBehaviour
{
    Gyroscope _gyro;
    Quaternion _gyroOrientation;
    Quaternion _lastFrameGyroOrientation;

    void Start()
    {
        _gyro = Input.gyro;
        _gyro.enabled = true;
    }

    void Update()
    {
        // print(_gyro.attitude.eulerAngles);
        _gyroOrientation *= _lastFrameGyroOrientation * Quaternion.Inverse(_gyroOrientation);
        transform.rotation = _gyroOrientation;

        _lastFrameGyroOrientation = _gyro.attitude;
    }
}
