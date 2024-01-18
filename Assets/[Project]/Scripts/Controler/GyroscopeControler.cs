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
        
        Input.gyro.enabled = true;

        _currentAccelerometre = Input.acceleration;
        _deltaAccelerometre += _currentAccelerometre - _lastFrameAccelerometre;
        _lastFrameAccelerometre = _currentAccelerometre;
        
        GyroModifyCamera();
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



    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Orientation: " + Screen.orientation);
        GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
        GUILayout.Label("iphone width/font: " + Screen.width + " : " + GUI.skin.label.fontSize);
    }
    
    public float gamepadSensivity = 10;
    // The Gyroscope is right-handed.  Unity is left handed.
    // Make the necessary change to the camera.
    void GyroModifyCamera()
    {
        // transform.localRotation = GyroToUnity(Input.gyro.attitude);
        transform.localRotation = Quaternion.Euler( Input.GetAxis("Vertical")*gamepadSensivity, 0, -Input.GetAxis("Horizontal")*gamepadSensivity );
    }

    
    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
